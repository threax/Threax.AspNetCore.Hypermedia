using Halcyon.HAL.Attributes;
using NJsonSchema;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.TypeScript;
using NJsonSchema.CodeGeneration.TypeScript.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;

namespace Threax.AspNetCore.Halcyon.ClientGen
{
    public class PhpClientWriter
    {
        private IClientGenerator clientGenerator;
        private const String ResultClassSuffix = "Result";
        private const string VoidReturnType = ": void";

        public PhpClientWriter(IClientGenerator clientGenerator)
        {
            this.clientGenerator = clientGenerator;
        }

        public async Task CreateClient(TextWriter writer)
        {
            var interfacesToWrite = new InterfaceManager();

            writer.WriteLine(
            @"use threax\halcyonclient\HalEndpointClient;
use threax\halcyonclient\CurlHelper;"
            );

            await WriteClient(interfacesToWrite, writer);

            //Write interfaces, kind of weird, no good docs for this
            //var settings = new TypeScriptGeneratorSettings()
            //{
            //    TypeStyle = TypeScriptTypeStyle.Interface,
            //    MarkOptionalProperties = true,
            //    DateTimeType = TypeScriptDateTimeType.String,
            //    EnumNameGenerator = new EnumValueEnumNameGenerator(),
            //};

            ////Gather up everything to write, skip duplicate instances of the same thing
            //Dictionary<String, CodeArtifact> codeArtifacts = new Dictionary<String, CodeArtifact>();
            //ExtensionCode lastExtensionCode = null;
            //foreach (var item in interfacesToWrite.Interfaces)
            //{
            //    //Remove any properties from item that are hal embeds
            //    var propertiesToRemove = item.Value.Properties.Where(i => i.Value.IsHalEmbedded()).ToList();
            //    foreach (var remove in propertiesToRemove)
            //    {
            //        item.Value.Properties.Remove(remove.Key);
            //    }

            //    var resolver = new TypeScriptTypeResolver(settings);
            //    resolver.RegisterSchemaDefinitions(new Dictionary<String, JsonSchema4>() { { item.Key, item.Value } }); //Add all discovered generators

            //    var generator = new TypeScriptGenerator(item.Value, settings, resolver);
            //    var artifacts = generator.GenerateTypes();
            //    foreach(var artifact in artifacts.Artifacts)
            //    {
            //        if (!codeArtifacts.ContainsKey(artifact.TypeName))
            //        {
            //            codeArtifacts.Add(artifact.TypeName, artifact);
            //        }
            //    }
            //    lastExtensionCode = artifacts.ExtensionCode;
            //}

            ////Write the classes officially
            ////From TypeScriptGenerator.cs GenerateFile, (NJsonSchema 9.10.49)
            //var model = new FileTemplateModel(settings)
            //{
            //    Types = ConversionUtilities.TrimWhiteSpaces(string.Join("\n\n", CodeArtifactCollection.OrderByBaseDependency(codeArtifacts.Values).Select(p => p.Code))),
            //    ExtensionCode = (TypeScriptExtensionCode)lastExtensionCode
            //};

            //var template = settings.TemplateFactory.CreateTemplate("TypeScript", "File", model);
            //var classes = ConversionUtilities.TrimWhiteSpaces(template.Render());
            //writer.WriteLine(classes);

            //Write out common interfaces we reuse in all clients
            writer.WriteLine(@"
class HalEndpointDocQuery {
    public $includeRequest;
    public $includeResponse;
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
class {client.Name}Injector {{
    private $url;
    private $fetcher;
    private $instance = NULL;

    constructor(string $url, CurlHelper $fetcher) {{
        $this->url = $url;
        $this->fetcher = $fetcher;
    }}

    public load(): {client.Name}{ResultClassSuffix} {{
        if ($this->$instance === NULL) {{
            $this->$instance = {client.Name}{ResultClassSuffix}.Load(this.url, this.fetcher);
        }}

        return $this->$instance;
    }}
}}");
                }

                writer.WriteLine($@"
class {client.Name}{ResultClassSuffix} {{
    private $client");

                if (client.IsEntryPoint)
                {
                    writer.WriteLine($@"
    public static function Load(string $url, CurlHelper $fetcher): {client.Name}{ResultClassSuffix} {{
        $result = HalEndpointClient::Load($url, $fetcher)
        return new {client.Name}{ResultClassSuffix}($result);
    }}");
                }

                //Write accessor for data

                writer.WriteLine($@"
    public function __construct(HalEndpointClient $client) {{
        $this->client = $client;
    }}

    public function getData() {{
        return $this->client->getData();
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
                    var loadFuncType = "load";

                    //Only take a request or upload, prefer requests
                    if (link.EndpointDoc.RequestSchema != null)
                    {
                        interfacesToWrite.Add(link.EndpointDoc.RequestSchema);
                        linkRequestArg = $"$data";
                    }

                    if (link.EndpointDoc.ResponseSchema != null)
                    {
                        if (link.EndpointDoc.ResponseSchema.IsRawResponse())
                        {
                            linkReturnType = $"";
                            loadFuncType = "loadRaw";
                        }
                        else
                        {
                            interfacesToWrite.Add(link.EndpointDoc.ResponseSchema);
                            linkReturnType = $": {link.EndpointDoc.ResponseSchema.Title}{ResultClassSuffix}";
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
                        outArgs += ", $data";
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
    public function {lowerFuncName}({inArgs}){linkReturnType} {{
        $r = $this->client->{fullLoadFunc}(""{link.Rel}""{outArgs})");

                        //If the returns are set return r, otherwise void out the promise so we don't leak on void functions
                        if (returnOpen != null && returnClose != null)
                        {
                            writer.Write($@"
        return {returnOpen}$r{returnClose};");
                        }
                        else //Returning the respose directly
                        {
                            writer.Write(";");
                        }

                        writer.WriteLine($@"
    }}

    public function can{upperFuncName}(): boolean {{
        return $this->client->hasLink(""{link.Rel}"");
    }}

    public function linkFor{upperFuncName}(): hal.HalLink {{
        return $this->client->getLink(""{link.Rel}"");
    }}");
                    }

                    if (link.EndpointDoc.HasDocumentation)
                    {
                        //Write link docs
                        writer.WriteLine($@"
    public function get{upperFuncName}Docs(HalEndpointDocQuery $query = NULL) {{
        return $this->client->loadLinkDoc(""{link.Rel}"", $query)->getData();
    }}

    public function has{upperFuncName}Docs(): boolean {{
        return $this->client->hasLinkDoc(""{link.Rel}"");
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
    private ${propertyName}Strong = NULL;
    public get{propertyName}(): array {{
        if ($this->{propertyName}Strong === NULL) {{
            $embeds = $this->client->GetEmbed(""{embedName}"");
            $this->{propertyName}Strong = $embeds->getAllClients();
        }}
        return $this->{propertyName}Strong;
    }}");
            }
            else
            {
                //Collection type found, write out results for each data entry.
                writer.WriteLine($@"
    private ${propertyName}Strong = NULL;
    public get{propertyName}(): array {{
        if ($this->{propertyName}Strong === NULL) {{
            $embeds = $this->client->GetEmbed(""{embedName}"");
            $clients = $embeds->getAllClients();
            $this->{propertyName}Strong = [];
            foreach ($clients as $client) {{
                array_push($this->{propertyName}Strong, new {collectionType}{ResultClassSuffix}($client));
            }}
        }}
        return $this->{propertyName}Strong;
    }}");
            }
        }
    }
}
