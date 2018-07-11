﻿using NJsonSchema;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.CSharp;
using NJsonSchema.CodeGeneration.CSharp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;

namespace Threax.AspNetCore.Halcyon.ClientGen
{
    public class CSharpClientWriter
    {
        private IClientGenerator clientGenerator;
        private const String ResultClassSuffix = "Result";
        private const string VoidReturnType = "Task";
        private CSharpOptions options;

        public CSharpClientWriter(IClientGenerator clientGenerator, CSharpOptions options)
        {
            this.clientGenerator = clientGenerator;
            this.options = options;
        }

        public async Task CreateClient(TextWriter writer)
        {
            var interfacesToWrite = new InterfaceManager();

writer.WriteLine(
$@"using Threax.AspNetCore.Halcyon.Client;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;

namespace {options.Namespace} {{"
);

            await WriteClient(interfacesToWrite, writer);

writer.WriteLine("}");

            //Write interfaces, kind of weird, no good docs for this
            var settings = new CSharpGeneratorSettings()
            {
                Namespace = options.Namespace,
                GenerateDataAnnotations = false,
                ClassStyle = CSharpClassStyle.Poco,  
                RequiredPropertiesMustBeDefined = false,
                EnumNameGenerator = new EnumValueEnumNameGenerator(),
                ArrayType = "List" //This is imported in the using statements above (System.Collections.Generic.List)
            };

            //Gather up everything to write, skip duplicate instances of the same thing
            Dictionary<String, CodeArtifact> codeArtifacts = new Dictionary<String, CodeArtifact>();
            foreach (var item in interfacesToWrite.Interfaces)
            {
                var resolver = new CSharpTypeResolver(settings);
                resolver.RegisterSchemaDefinitions(new Dictionary<String, JsonSchema4>() { { item.Key, item.Value } }); //Add all discovered generators

                var generator = new CSharpGenerator(item.Value, settings, resolver);
                var artifacts = generator.GenerateTypes();
                foreach (var artifact in artifacts.Artifacts)
                {
                    if (!codeArtifacts.ContainsKey(artifact.TypeName))
                    {
                        codeArtifacts.Add(artifact.TypeName, artifact);
                    }
                }
            }

            //Write the classes officially
            //From TypeScriptGenerator.cs GenerateFile, (NJsonSchema 9.10.49)
            var model = new FileTemplateModel()
            {
                Namespace = settings.Namespace ?? string.Empty,
                TypesCode = ConversionUtilities.TrimWhiteSpaces(string.Join("\n\n", CodeArtifactCollection.OrderByBaseDependency(codeArtifacts.Values).Select(p => p.Code))),
            };

            var template = settings.TemplateFactory.CreateTemplate("CSharp", "File", model);
            var classes = ConversionUtilities.TrimWhiteSpaces(template.Render());
            writer.WriteLine(classes);
            //End Write Interfaces
        }

        private async Task WriteClient(InterfaceManager interfacesToWrite, TextWriter writer)
        {
            foreach (var client in await clientGenerator.GetEndpointDefinitions())
            {
                //Write injector
                if (client.IsEntryPoint)
                {
writer.WriteLine($@"
public class {client.Name}Injector 
{{
    private string url;
    private IHttpClientFactory fetcher;
    private {client.Name}{ResultClassSuffix} instance = default({client.Name}{ResultClassSuffix});

    public {client.Name}Injector(string url, IHttpClientFactory fetcher)
    {{
        this.url = url;
        this.fetcher = fetcher;
    }}

    public async Task<{client.Name}{ResultClassSuffix}> Load()
    {{
        if (this.instance == default({client.Name}{ResultClassSuffix}))
        {{
            this.instance = await {client.Name}{ResultClassSuffix}.Load(this.url, this.fetcher);
        }}
        return this.instance;
    }}
}}");
                }

writer.WriteLine($@"
public class {client.Name}{ResultClassSuffix} 
{{
    private HalEndpointClient client;");

                if (client.IsEntryPoint)
                {
                    writer.WriteLine($@"
    public static async Task<{client.Name}{ResultClassSuffix}> Load(string url, IHttpClientFactory fetcher)
    {{
        var result = await HalEndpointClient.Load(new HalLink(url, ""GET""), fetcher);
        return new {client.Name}{ResultClassSuffix}(result);
    }}");
                }

                //Write accessor for data

writer.WriteLine($@"
    public {client.Name}{ResultClassSuffix}(HalEndpointClient client) 
    {{
        this.client = client;
    }}

    private {client.Name} strongData = default({client.Name});
    public {client.Name} Data 
    {{
        get
        {{
            if(this.strongData == default({client.Name}))
            {{
                this.strongData = this.client.GetData<{client.Name}>();  
            }}
            return this.strongData;
        }}
    }}");

                //Write data interface if we haven't found it yet
                interfacesToWrite.Add(client.Schema);

                if (client.IsCollectionView)
                {
                    var collectionType = client.CollectionType;
                    if(collectionType == null)
                    {
                        //No collection type, write out an "any" client.

writer.WriteLine($@"
    private List<HalEndpointClient> strongItems;
    public List<HalEndpointClient> Items
    {{
        get
        {{
            if (this.strongItems == null) 
            {{
                var embeds = this.client.GetEmbed(""values"");
                this.strongItems = embeds.GetAllClients();
            }}
            return this.strongItems;
        }}
    }}");
                    }
                    else
                    {
                        //Collection type found, write out results for each data entry.
writer.WriteLine($@"
    private List<{collectionType}{ResultClassSuffix}> strongItems = null;
    public List<{collectionType}{ResultClassSuffix}> Items
    {{
        get
        {{
            if (this.strongItems == null) 
            {{
                var embeds = this.client.GetEmbed(""values"");
                var clients = embeds.GetAllClients();
                this.strongItems = new List<{collectionType}{ResultClassSuffix}>(clients.Select(i => new {collectionType}{ResultClassSuffix}(i)));
            }}
            return this.strongItems;
        }}
    }}");
                    }
                }

                foreach (var link in client.Links)
                {
                    String returnOpen = null;
                    String returnClose = null;
                    String linkReturnType = null;
                    var linkQueryArg = "";
                    var linkRequestArg = "";
                    var reqIsForm = false;
                    var loadFuncType = "Load";

                    //Extract any interfaces that need to be written
                    if (link.EndpointDoc.QuerySchema != null)
                    {
                        interfacesToWrite.Add(link.EndpointDoc.QuerySchema);
                        linkQueryArg = $"{link.EndpointDoc.QuerySchema.Title} query";
                        if (link.EndpointDoc.QuerySchema.IsArray())
                        {
                            linkQueryArg = $"IEnumerable<{link.EndpointDoc.QuerySchema.Title}> query";
                        }
                        else
                        {
                            linkQueryArg = $"{link.EndpointDoc.QuerySchema.Title} query";
                        }
                    }

                    //Only take a request or upload, prefer requests
                    if (link.EndpointDoc.RequestSchema != null)
                    {
                        interfacesToWrite.Add(link.EndpointDoc.RequestSchema);
                        if (link.EndpointDoc.RequestSchema.IsArray())
                        {
                            linkRequestArg = $"IEnumerable<{link.EndpointDoc.RequestSchema.Title}> data";
                        }
                        else
                        {
                            linkRequestArg = $"{link.EndpointDoc.RequestSchema.Title} data";
                        }

                        reqIsForm = link.EndpointDoc.RequestSchema.DataIsForm();
                    }

                    if (link.EndpointDoc.ResponseSchema != null)
                    {
                        if (link.EndpointDoc.ResponseSchema.IsRawResponse())
                        {
                            linkReturnType = $"Task<RawEndpointResult>";
                            loadFuncType = "LoadRaw";
                        }
                        else
                        {
                            interfacesToWrite.Add(link.EndpointDoc.ResponseSchema);
                            linkReturnType = $"Task<{link.EndpointDoc.ResponseSchema.Title}{ResultClassSuffix}>";
                            returnOpen = $"new {link.EndpointDoc.ResponseSchema.Title}{ResultClassSuffix}(";
                            returnClose = ")";
                        }
                    }

                    if(linkReturnType == null)
                    {
                        linkReturnType = VoidReturnType;
                    }

                    var loadFunc = "Link";
                    var inArgs = "";
                    var outArgs = "";
                    bool bothArgs = false;
                    if (linkQueryArg != "")
                    {
                        inArgs = linkQueryArg;
                        loadFunc = "LinkWithData";
                        outArgs = ", query";

                        if (linkRequestArg != "")
                        {
                            inArgs += ", ";
                            if (reqIsForm)
                            {
                                loadFunc = "LinkWithQueryAndData";
                            }
                            else
                            {
                                loadFunc = "LinkWithQueryAndData";
                            }
                            bothArgs = true;
                        }
                    }

                    if (linkRequestArg != "")
                    {
                        inArgs += linkRequestArg;
                        outArgs += ", data";
                        if (!bothArgs)
                        {
                            if (reqIsForm)
                            {
                                loadFunc = "LinkWithData";
                            }
                            else
                            {
                                loadFunc = "LinkWithData";
                            }
                        }
                    }

                    var funcName = link.Rel;
                    if (link.Rel == "self")
                    {
                        //Self links make refresh functions, also clear in and out args
                        funcName = "refresh";
                        inArgs = "";
                        outArgs = "";
                        loadFunc = "Link";
                    }

                    var lowerFuncName = funcName.Substring(0, 1).ToLowerInvariant() + funcName.Substring(1);
                    var upperFuncName = funcName.Substring(0, 1).ToUpperInvariant() + funcName.Substring(1);

                    var fullLoadFunc = loadFuncType + loadFunc;

                    if (!link.DocsOnly)
                    {
                        //Write link
                        writer.Write($@"
    public async {linkReturnType} {upperFuncName}({inArgs}) 
    {{
        var result = await this.client.{fullLoadFunc}(""{link.Rel}""{outArgs});");

                        //If the returns are set return r, otherwise void out the promise so we don't leak on void functions
                        if (returnOpen != null && returnClose != null) {
                            writer.WriteLine($@"
        return {returnOpen}result{returnClose};");
                        }
                        else if (linkReturnType == VoidReturnType) //Write a then that will hide the promise result and become void.
                        {
                            //Don't write anything for this
                        }
                        else //Returning the respose directly
                        {
                            writer.Write(@"
        return result;");
                        }

                        writer.WriteLine($@"
    }}

    public bool Can{upperFuncName} 
    {{
        get 
        {{
            return this.client.HasLink(""{link.Rel}"");
        }}
    }}

    public HalLink LinkFor{upperFuncName} 
    {{
        get 
        {{
            return this.client.GetLink(""{link.Rel}"");
        }}
    }}");
                    }

                    if (link.EndpointDoc.HasDocumentation)
                    {
                        //Write link docs
                        writer.WriteLine($@"
    public async Task<HalEndpointDoc> Get{upperFuncName}Docs() 
    {{
        var result = await this.client.LoadLinkDoc(""{link.Rel}"");
        return result.GetData<HalEndpointDoc>();
    }}

    public bool Has{upperFuncName}Docs() {{
        return this.client.HasLinkDoc(""{link.Rel}"");
    }}");
                    }
                }

                //Close class
                //Close class
                writer.WriteLine("}");
            }
        }
    }
}
