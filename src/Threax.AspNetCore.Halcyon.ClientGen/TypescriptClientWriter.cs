using NJsonSchema;
using NJsonSchema.CodeGeneration.TypeScript;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;

namespace Threax.AspNetCore.Halcyon.ClientGen
{
    public class TypescriptClientWriter
    {
        private IClientGenerator clientGenerator;
        private const String ResultClassSuffix = "Result";
        private const string VoidReturnType = ": Promise<void>";

        public TypescriptClientWriter(IClientGenerator clientGenerator)
        {
            this.clientGenerator = clientGenerator;
        }

        public void CreateClient(TextWriter writer)
        {
            var interfacesToWrite = new InterfaceManager();

writer.WriteLine(
@"import * as hal from 'hr.halcyon.EndpointClient';"
);

            WriteClient(interfacesToWrite, writer);

            //Write interfaces, kind of weird, no good docs for this
            var settings = new TypeScriptGeneratorSettings()
            {
                TypeStyle = TypeScriptTypeStyle.Interface,
                MarkOptionalProperties = true
            };

            var root = new Object(); //Dunno why it needs this, but this does work
            var resolver = new TypeScriptTypeResolver(settings, root);
            resolver.AddGenerators(interfacesToWrite.Interfaces); //Add all discovered generators

            var schema = interfacesToWrite.FirstSchema;
            if (schema != null) {
                var generator = new TypeScriptGenerator(schema, settings, resolver, root);
                var classes = generator.GenerateFile();
                writer.WriteLine(classes);
            }
            //End Write Interfaces
        }

        private void WriteClient(InterfaceManager interfacesToWrite, TextWriter writer)
        {
            foreach (var client in clientGenerator.GetEndpointDefinitions())
            {
                //Write injector
                if (client.IsEntryPoint)
                {
writer.WriteLine($@"
export class {client.Name}Injector {{
    private url: string;
    private fetcher: hal.Fetcher;
    private instance: {client.Name}{ResultClassSuffix};

    constructor(url: string, fetcher: hal.Fetcher) {{
        this.url = url;
        this.fetcher = fetcher;
    }}

    public load(): Promise<{client.Name}{ResultClassSuffix}> {{
        if (!this.instance) {{
            return {client.Name}{ResultClassSuffix}.Load(this.url, this.fetcher).then((r) => {{
                this.instance = r;
                return r;
            }});
        }}

        return Promise.resolve(this.instance);
    }}
}}");
                }

writer.WriteLine($@"
export class {client.Name}{ResultClassSuffix} {{
    private client: hal.HalEndpointClient;");

                if (client.IsEntryPoint)
                {
                    writer.WriteLine($@"
    public static Load(url: string, fetcher: hal.Fetcher): Promise<{client.Name}{ResultClassSuffix}> {{
        return hal.HalEndpointClient.Load({{
            href: url,
            method: ""GET""
        }}, fetcher)
            .then(c => {{
                 return new {client.Name}{ResultClassSuffix}(c);
             }});
            }}");
                }

                //Write accessor for data

writer.WriteLine($@"
    constructor(client: hal.HalEndpointClient) {{
        this.client = client;
    }}

    private strongData: {client.Name} = undefined;
    public get data(): {client.Name} {{
        this.strongData = this.strongData || this.client.GetData<{client.Name}>();
        return this.strongData;
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
    private strongItems: any[];
    public get items(): hal.HalEndpointClient[] {{
        if (this.strongItems === undefined) {{
            var embeds = this.client.GetEmbed(""values"");
            this.strongItems = embeds.GetAllClients();
        }}
        return this.strongItems;
    }}");
                    }
                    else
                    {
                        //Collection type found, write out results for each data entry.
writer.WriteLine($@"
    private strongItems: {collectionType}{ResultClassSuffix}[];
    public get items(): {collectionType}{ResultClassSuffix}[] {{
        if (this.strongItems === undefined) {{
            var embeds = this.client.GetEmbed(""values"");
            var clients = embeds.GetAllClients();
            this.strongItems = [];
            for (var i = 0; i < clients.length; ++i) {{
                this.strongItems.push(new {collectionType}{ResultClassSuffix}(clients[i]));
            }}
        }}
        return this.strongItems;
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
                        linkQueryArg = $"query: {link.EndpointDoc.QuerySchema.Title}";
                        if (link.EndpointDoc.QuerySchema.IsArray())
                        {
                            linkQueryArg += "[]";
                        }
                    }

                    //Only take a request or upload, prefer requests
                    if (link.EndpointDoc.RequestSchema != null)
                    {
                        interfacesToWrite.Add(link.EndpointDoc.RequestSchema);
                        linkRequestArg = $"data: {link.EndpointDoc.RequestSchema.Title}";
                        if (link.EndpointDoc.RequestSchema.IsArray())
                        {
                            linkRequestArg += "[]";
                        }

                        reqIsForm = link.EndpointDoc.RequestSchema.DataIsForm();
                    }

                    if (link.EndpointDoc.ResponseSchema != null)
                    {
                        if (link.EndpointDoc.ResponseSchema.IsRawResponse())
                        {
                            linkReturnType = $": Promise<hal.Response>";
                            loadFuncType = "LoadRaw";
                        }
                        else
                        {
                            interfacesToWrite.Add(link.EndpointDoc.ResponseSchema);
                            linkReturnType = $": Promise<{link.EndpointDoc.ResponseSchema.Title}{ResultClassSuffix}>";
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
                        loadFunc = "LinkWithQuery";
                        outArgs = ", query";

                        if (linkRequestArg != "")
                        {
                            inArgs += ", ";
                            if (reqIsForm)
                            {
                                loadFunc = "LinkWithQueryAndForm";
                            }
                            else
                            {
                                loadFunc = "LinkWithQueryAndBody";
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
                                loadFunc = "LinkWithForm";
                            }
                            else
                            {
                                loadFunc = "LinkWithBody";
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
    public {lowerFuncName}({inArgs}){linkReturnType} {{
        return this.client.{fullLoadFunc}(""{link.Rel}""{outArgs})");

                        //If the returns are set return r, otherwise void out the promise so we don't leak on void functions
                        if (returnOpen != null && returnClose != null) {
                            writer.WriteLine($@"
               .then(r => {{
                    return {returnOpen}r{returnClose};
                }});");
                        }
                        else if(linkReturnType == VoidReturnType) //Write a then that will hide the promise result and become void.
                        {
                            writer.Write(".then(hal.makeVoid);");
                        }
                        else //Returning the respose directly
                        {
                            writer.Write(";");
                        }

                        writer.WriteLine($@"
    }}

    public can{upperFuncName}(): boolean {{
        return this.client.HasLink(""{link.Rel}"");
    }}");
                    }

                    if (link.EndpointDoc.HasDocumentation)
                    {
                        //Write link docs
                        writer.WriteLine($@"
    public get{upperFuncName}Docs(): Promise<hal.HalEndpointDoc> {{
        return this.client.LoadLinkDoc(""{link.Rel}"")
            .then(r => {{
                return r.GetData<hal.HalEndpointDoc>();
            }});
    }}

    public has{upperFuncName}Docs(): boolean {{
        return this.client.HasLinkDoc(""{link.Rel}"");
    }}");
                    }
                }

                //Close class
writer.WriteLine("}");
            }
        }
    }
}
