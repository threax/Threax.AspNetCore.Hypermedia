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
            Dictionary<String, JsonSchema4> interfacesToWrite = new Dictionary<string, JsonSchema4>();

writer.WriteLine(
@"import * as hal from 'hr.halcyon.EndpointClient';
import { Fetcher } from 'hr.fetcher';"
);

            WriteClient(interfacesToWrite, writer);

            foreach (var write in interfacesToWrite.Values)
            {
                var generator = new TypeScriptGenerator(write);
                generator.Settings.TypeStyle = TypeScriptTypeStyle.Interface;
                generator.Settings.MarkOptionalProperties = true;
                var classes = generator.GenerateType(write.Title);
                writer.WriteLine(classes.Code);
            }
        }

        private void WriteClient(Dictionary<string, JsonSchema4> interfacesToWrite, TextWriter writer)
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
                if (!interfacesToWrite.ContainsKey(client.Name))
                {
                    interfacesToWrite.Add(client.Name, client.Schema);
                }

                if (client.IsCollectionView)
                {
                    writer.WriteLine($@"
    private strongItems;");

                    var collectionType = client.CollectionType;
                    if(collectionType == null)
                    {
                        //No collection type, write out an "any" client.
writer.WriteLine($@"
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

                    //Extract any interfaces that need to be written
                    if (link.EndpointDoc.QuerySchema != null)
                    {
                        if (!interfacesToWrite.ContainsKey(link.EndpointDoc.QuerySchema.Title))
                        {
                            interfacesToWrite.Add(link.EndpointDoc.QuerySchema.Title, link.EndpointDoc.QuerySchema);
                        }
                        linkQueryArg = $"query: {link.EndpointDoc.QuerySchema.Title}";
                        if (link.EndpointDoc.QuerySchema.IsArray())
                        {
                            linkQueryArg += "[]";
                        }
                    }

                    if (link.EndpointDoc.RequestSchema != null)
                    {
                        if (!interfacesToWrite.ContainsKey(link.EndpointDoc.RequestSchema.Title))
                        {
                            interfacesToWrite.Add(link.EndpointDoc.RequestSchema.Title, link.EndpointDoc.RequestSchema);
                        }
                        linkRequestArg = $"data: {link.EndpointDoc.RequestSchema.Title}";
                        if (link.EndpointDoc.RequestSchema.IsArray())
                        {
                            linkRequestArg += "[]";
                        }
                    }

                    if (link.EndpointDoc.ResponseSchema != null)
                    {
                        if (!interfacesToWrite.ContainsKey(link.EndpointDoc.ResponseSchema.Title))
                        {
                            interfacesToWrite.Add(link.EndpointDoc.ResponseSchema.Title, link.EndpointDoc.ResponseSchema);
                        }
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
                            func = "LoadLinkWithQueryAndBody";
                            bothArgs = true;
                        }
                    }

                    if (linkRequestArg != "")
                    {
                        inArgs += linkRequestArg;
                        outArgs += ", data";
                        if (!bothArgs)
                        {
                            func = "LoadLinkWithBody";
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
