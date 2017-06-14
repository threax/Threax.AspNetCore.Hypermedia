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

        public TypescriptClientWriter(IClientGenerator clientGenerator)
        {
            this.clientGenerator = clientGenerator;
        }

        public void CreateClient(TextWriter writer)
        {
            var interfacesToWrite = new InterfaceManager();

writer.WriteLine(
@"import * as hal from 'hr.halcyon.EndpointClient';
import { Fetcher } from 'hr.fetcher';"
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
    private fetcher: Fetcher;
    private instance: Promise<{client.Name}{ResultClassSuffix}>;

    constructor(url: string, fetcher: Fetcher) {{
        this.url = url;
        this.fetcher = fetcher;
    }}

    public load(): Promise<{client.Name}{ResultClassSuffix}> {{
        if (!this.instance) {{
            this.instance = {client.Name}{ResultClassSuffix}.Load(this.url, this.fetcher);
        }}

        return this.instance;
    }}
}}");
                }

writer.WriteLine($@"
export class {client.Name}{ResultClassSuffix} {{
    private client: hal.HalEndpointClient;");

                if (client.IsEntryPoint)
                {
                    writer.WriteLine($@"
    public static Load(url: string, fetcher: Fetcher): Promise<{client.Name}{ResultClassSuffix}> {{
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
        if(this.strongItems === undefined){{
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
        if(this.strongItems === undefined){{
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
                    var returnClassOpen = "";
                    var returnClassClose = "";
                    var linkReturnType = "";
                    var linkQueryArg = "";
                    var linkRequestArg = "";
                    var reqIsUpload = false;

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

                    if (link.EndpointDoc.RequestSchema != null)
                    {
                        interfacesToWrite.Add(link.EndpointDoc.RequestSchema);
                        var reqType = link.EndpointDoc.RequestSchema.Title;
                        if (link.EndpointDoc.RequestSchema.Title == "IFormFile") //If this is a file upload, use file info
                        {
                            reqType = "hal.FileInfo";
                            if (link.EndpointDoc.RequestSchema.IsArray())
                            {
                                reqType += " | hal.FileInfo[]";
                            }
                            reqIsUpload = true;
                        }
                        else if (link.EndpointDoc.RequestSchema.IsArray())
                        {
                            reqType += "[]";
                        }
                        linkRequestArg = $"data: {reqType}";
                    }

                    if (link.EndpointDoc.ResponseSchema != null)
                    {
                        interfacesToWrite.Add(link.EndpointDoc.ResponseSchema);
                        linkReturnType = $": Promise<{link.EndpointDoc.ResponseSchema.Title}{ResultClassSuffix}>";
                        returnClassOpen = $"new {link.EndpointDoc.ResponseSchema.Title}{ResultClassSuffix}(";
                        returnClassClose = ")";
                    }

                    var func = "LoadLink";
                    var inArgs = "";
                    var outArgs = "";
                    bool bothArgs = false;
                    if (linkQueryArg != "")
                    {
                        inArgs = linkQueryArg;
                        func = "LoadLinkWithQuery";
                        outArgs = ", query";

                        if (linkRequestArg != "")
                        {
                            inArgs += ", ";
                            if (reqIsUpload)
                            {
                                func = "LoadLinkWithQueryAndFile";
                            }
                            else
                            {
                                func = "LoadLinkWithQueryAndBody";
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
                            if (reqIsUpload)
                            {
                                func = "LoadLinkWithFile";
                            }
                            else
                            {
                                func = "LoadLinkWithBody";
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
                        func = "LoadLink";
                    }

                    var lowerFuncName = funcName.Substring(0, 1).ToLowerInvariant() + funcName.Substring(1);
                    var upperFuncName = funcName.Substring(0, 1).ToUpperInvariant() + funcName.Substring(1);

                    if (!link.DocsOnly)
                    {
                        //Write link
                        writer.WriteLine($@"
    public {lowerFuncName}({inArgs}){linkReturnType} {{
        return this.client.{func}(""{link.Rel}""{outArgs})
               .then(r => {{
                    return {returnClassOpen}r{returnClassClose};
                }});
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
