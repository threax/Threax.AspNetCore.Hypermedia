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
        private PhpOptions phpOptions;

        public PhpClientWriter(IClientGenerator clientGenerator, PhpOptions phpOptions)
        {
            this.clientGenerator = clientGenerator;
            this.phpOptions = phpOptions;
        }

        public async Task CreateClient(TextWriter writer)
        {
            var interfacesToWrite = new InterfaceManager();

            writer.WriteLine($"<?php");
            writer.WriteLine();

            if (phpOptions.Namespace != null)
            {
                writer.WriteLine($"namespace {phpOptions.Namespace};");
            }

            writer.WriteLine(
            @"use threax\halcyonclient\HalEndpointClient;
use threax\halcyonclient\CurlHelper;"
            );

            await WriteClient(interfacesToWrite, writer);

            //PHP does not include any interfaces / classes for the data

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

    public function __construct(string $url, CurlHelper $fetcher) {{
        $this->url = $url;
        $this->fetcher = $fetcher;
    }}

    public function load(): {client.Name}{ResultClassSuffix} {{
        if ($this->instance === NULL) {{
            $this->instance = {client.Name}{ResultClassSuffix}::Load($this->url, $this->fetcher);
        }}

        return $this->instance;
    }}
}}");
                }

                writer.WriteLine($@"
class {client.Name}{ResultClassSuffix} {{
    private $client;");

                if (client.IsEntryPoint)
                {
                    writer.WriteLine($@"
    public static function Load(string $url, CurlHelper $fetcher): {client.Name}{ResultClassSuffix} {{
        $result = HalEndpointClient::Load($url, $fetcher);
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
        $r = $this->client->{fullLoadFunc}(""{link.Rel}""{outArgs});");

                        //If the returns are set return r, otherwise void out the promise so we don't leak on void functions
                        if (returnOpen != null && returnClose != null)
                        {
                            writer.Write($@"
        return {returnOpen}$r{returnClose};");
                        }

                        writer.WriteLine($@"
    }}

    public function can{upperFuncName}(): boolean {{
        return $this->client->hasLink(""{link.Rel}"");
    }}

    public function linkFor{upperFuncName}() {{
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
            //This is a lame way to convert to pascal case, but there isn't really any choice.
            //The users have a lot of control over this variable name, so they can adjust as needed.
            var pascalPropertyName = Char.ToUpperInvariant(propertyName[0]) + propertyName.Substring(1);

            if (collectionType == null)
            {
                //No collection type, write out an "any" client.
                writer.WriteLine($@"
    private ${propertyName}Strong = NULL;
    public function get{pascalPropertyName}(): array {{
        if ($this->{propertyName}Strong === NULL) {{
            $embeds = $this->client->getEmbed(""{embedName}"");
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
    public function get{pascalPropertyName}(): array {{
        if ($this->{propertyName}Strong === NULL) {{
            $embeds = $this->client->getEmbed(""{embedName}"");
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
