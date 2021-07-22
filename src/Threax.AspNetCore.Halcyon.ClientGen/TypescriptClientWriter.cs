using Halcyon.HAL.Attributes;
using Threax.NJsonSchema;
using Threax.NJsonSchema.CodeGeneration;
using Threax.NJsonSchema.CodeGeneration.TypeScript;
using Threax.NJsonSchema.CodeGeneration.TypeScript.Models;
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

        public async Task CreateClient(TextWriter writer)
        {
            var interfacesToWrite = new InterfaceManager();

            writer.WriteLine(
            @"import * as hal from 'htmlrapier.halcyon/src/EndpointClient';
import { Fetcher } from 'htmlrapier/src/fetcher';"
            );

            await WriteClient(interfacesToWrite, writer);

            //Write interfaces, kind of weird, no good docs for this
            var settings = new TypeScriptGeneratorSettings()
            {
                TypeStyle = TypeScriptTypeStyle.Interface,
                ForceAllPropertiesOptional = true,
                DateTimeType = TypeScriptDateTimeType.String,
                EnumNameGenerator = new EnumValueEnumNameGenerator(),
            };

            //Gather up everything to write, skip duplicate instances of the same thing
            Dictionary<String, CodeArtifact> codeArtifacts = new Dictionary<String, CodeArtifact>();
            ExtensionCode lastExtensionCode = null;
            foreach (var item in interfacesToWrite.Interfaces)
            {
                //Remove any properties from item that are hal embeds
                var propertiesToRemove = item.Value.Properties.Where(i => i.Value.IsHalEmbedded()).ToList();
                foreach (var remove in propertiesToRemove)
                {
                    item.Value.Properties.Remove(remove.Key);
                }

                var resolver = new TypeScriptTypeResolver(settings);
                resolver.RegisterSchemaDefinitions(new Dictionary<String, JsonSchema4>() { { item.Key, item.Value } }); //Add all discovered generators

                var generator = new TypeScriptGenerator(item.Value, settings, resolver);
                var artifacts = generator.GenerateTypes();
                foreach(var artifact in artifacts.Artifacts)
                {
                    if (!codeArtifacts.ContainsKey(artifact.TypeName))
                    {
                        codeArtifacts.Add(artifact.TypeName, artifact);
                    }
                }
                lastExtensionCode = artifacts.ExtensionCode;
            }
            
            //Write the classes officially
            //From TypeScriptGenerator.cs GenerateFile, (NJsonSchema 9.10.49)
            var model = new FileTemplateModel(settings)
            {
                Types = ConversionUtilities.TrimWhiteSpaces(string.Join("\n\n", CodeArtifactCollection.OrderByBaseDependency(codeArtifacts.Values).Select(p => p.Code))),
                ExtensionCode = (TypeScriptExtensionCode)lastExtensionCode
            };

            var template = settings.TemplateFactory.CreateTemplate("TypeScript", "File", model);
            var classes = ConversionUtilities.TrimWhiteSpaces(template.Render());
            writer.WriteLine(classes);

            //Write out common interfaces we reuse in all clients
            writer.WriteLine(@"
export interface HalEndpointDocQuery {
    includeRequest?: boolean;
    includeResponse?: boolean;
}");

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
export class {client.Name}Injector {{
    private instancePromise: Promise<{client.Name}{ResultClassSuffix}>;

    constructor(private url: string, private fetcher: Fetcher, private data?: any) {{}}

    public load(): Promise<{client.Name}{ResultClassSuffix}> {{
        if (!this.instancePromise) {{
            if (this.data) {{
                this.instancePromise = Promise.resolve(new {client.Name}{ResultClassSuffix}(new hal.HalEndpointClient(this.data, this.fetcher)));
            }}
            else {{
                this.instancePromise = {client.Name}{ResultClassSuffix}.Load(this.url, this.fetcher);
            }}
        }}
        return this.instancePromise;
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
                    WriteEmbedAccessor(writer, "items", "values", client.CollectionType);
                }

                //Write out any embedded properties
                foreach (var embedded in client.Schema.Properties.Where(i => i.Value.IsHalEmbedded()))
                {
                    var embeddedItem = embedded.Value.Item;
                    if (embeddedItem.HasReference)
                    {
                        var reference = embeddedItem.Reference; //Get the reference
                        var def = client.Schema.Definitions.First(i => i.Value == reference); //Find reference in definitions, njsonschema will have the objects the same, so this is a valid way to look this up
                        var typeHint = def.Key.Replace('\\', '/').Split('/').Last();
                        WriteEmbedAccessor(writer, embedded.Key, embedded.Key, typeHint);
                    }
                }

                foreach (var link in client.Links)
                {
                    String returnOpen = null;
                    String returnClose = null;
                    String linkReturnType = null;
                    var linkRequestArg = "";
                    var loadFuncType = "Load";

                    //Only take a request or upload, prefer requests
                    if (link.EndpointDoc.RequestSchema != null)
                    {
                        interfacesToWrite.Add(link.EndpointDoc.RequestSchema);
                        linkRequestArg = $"data: {link.EndpointDoc.RequestSchema.Title}";
                        if (link.EndpointDoc.RequestSchema.IsArray())
                        {
                            linkRequestArg += "[]";
                        }
                    }

                    if (link.EndpointDoc.ResponseSchema != null)
                    {
                        if (link.EndpointDoc.ResponseSchema.IsRawResponse())
                        {
                            linkReturnType = $": Promise<Response>";
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

                    if (linkReturnType == null)
                    {
                        linkReturnType = VoidReturnType;
                    }

                    var loadFunc = "Link";
                    var inArgs = "";
                    var outArgs = "";

                    if (linkRequestArg != "")
                    {
                        inArgs += linkRequestArg;
                        outArgs += ", data";
                        loadFunc = "LinkWithData";
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
                        if (returnOpen != null && returnClose != null)
                        {
                            writer.WriteLine($@"
               .then(r => {{
                    return {returnOpen}r{returnClose};
                }});");
                        }
                        else if (linkReturnType == VoidReturnType) //Write a then that will hide the promise result and become void.
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
    }}

    public linkFor{upperFuncName}(): hal.HalLink {{
        return this.client.GetLink(""{link.Rel}"");
    }}");
                    }

                    if (link.EndpointDoc.HasDocumentation)
                    {
                        //Write link docs
                        writer.WriteLine($@"
    public get{upperFuncName}Docs(query?: HalEndpointDocQuery): Promise<hal.HalEndpointDoc> {{
        return this.client.LoadLinkDoc(""{link.Rel}"", query)
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

        private static void WriteEmbedAccessor(TextWriter writer, String propertyName, String embedName, string collectionType)
        {
            if (collectionType == null)
            {
                //No collection type, write out an "any" client.

                writer.WriteLine($@"
    private {propertyName}Strong: any[];
    public get {propertyName}(): hal.HalEndpointClient[] {{
        if (this.{propertyName}Strong === undefined) {{
            var embeds = this.client.GetEmbed(""{embedName}"");
            this.{propertyName}Strong = embeds.GetAllClients();
        }}
        return this.{propertyName}Strong;
    }}");
            }
            else
            {
                //Collection type found, write out results for each data entry.
                writer.WriteLine($@"
    private {propertyName}Strong: {collectionType}{ResultClassSuffix}[];
    public get {propertyName}(): {collectionType}{ResultClassSuffix}[] {{
        if (this.{propertyName}Strong === undefined) {{
            var embeds = this.client.GetEmbed(""{embedName}"");
            var clients = embeds.GetAllClients();
            this.{propertyName}Strong = [];
            for (var i = 0; i < clients.length; ++i) {{
                this.{propertyName}Strong.push(new {collectionType}{ResultClassSuffix}(clients[i]));
            }}
        }}
        return this.{propertyName}Strong;
    }}");
            }
        }
    }
}
