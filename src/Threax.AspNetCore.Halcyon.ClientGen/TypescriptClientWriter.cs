using NJsonSchema;
using NJsonSchema.CodeGeneration.TypeScript;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

        public String CreateClient()
        {
            Dictionary<String, JsonSchema4> interfacesToWrite = new Dictionary<string, JsonSchema4>();

            using (var writer = new StringWriter())
            {
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

                return writer.ToString();
            }
        }

        private void WriteClient(Dictionary<string, JsonSchema4> interfacesToWrite, TextWriter writer)
        {
            foreach (var client in clientGenerator.GetEndpointDefinitions())
            {
writer.WriteLine($@"
export class {client.Name}{ResultClassSuffix} {{
    private client: hal.HalEndpointClient;

    constructor(client: hal.HalEndpointClient) {{
        this.client = client;
    }}

    public get data(): {client.Name} {{
        return this.client.GetData<{client.Name}>();
    }}
");

                if (client.IsCollectionView)
                {
                    var collectionType = client.CollectionType.Name;
writer.WriteLine($@"
    public get items(): {collectionType}{ResultClassSuffix}[] {{
        var embeds = this.client.GetEmbed(""values"");
        var clients = embeds.GetAllClients();
        var result: {collectionType}{ResultClassSuffix}[] = [];
        for (var i = 0; i < clients.length; ++i) {{
            result.push(new {collectionType}{ResultClassSuffix}(clients[i]));
        }}
        return result;
    }}
");
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
                    }

                    if (link.EndpointDoc.RequestSchema != null)
                    {
                        if (!interfacesToWrite.ContainsKey(link.EndpointDoc.RequestSchema.Title))
                        {
                            interfacesToWrite.Add(link.EndpointDoc.RequestSchema.Title, link.EndpointDoc.RequestSchema);
                        }
                        linkRequestArg = $"data: {link.EndpointDoc.RequestSchema.Title}";
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
    }}
");
                }

                //Close class
writer.WriteLine(@"
}");
            }
        }
    }
}
