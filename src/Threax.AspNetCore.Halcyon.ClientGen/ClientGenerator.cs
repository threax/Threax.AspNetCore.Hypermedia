﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;

namespace Threax.AspNetCore.Halcyon.ClientGen
{
    public class ClientGenerator : IClientGenerator
    {
        private IResultViewProvider resultViewProvider;
        private IEndpointDocBuilder endpointDocBuilder;
        private ISchemaBuilder schemaBuilder;

        public ClientGenerator(IResultViewProvider resultViewProvider, IEndpointDocBuilder endpointDocBuilder, ISchemaBuilder schemaBuilder)
        {
            this.resultViewProvider = resultViewProvider;
            this.endpointDocBuilder = endpointDocBuilder;
            this.schemaBuilder = schemaBuilder;
        }

        public async Task<IEnumerable<EndpointClientDefinition>> GetEndpointDefinitions()
        {
            var definitions = new List<EndpointClientDefinition>();
            foreach(var type in resultViewProvider.GetResultViewTypes())
            {
                EndpointClientDefinition clientDef = new EndpointClientDefinition(type, await schemaBuilder.GetSchema(type));
                var customAttrs = type.GetTypeInfo().GetCustomAttributes();
                foreach (var link in customAttrs)
                {
                    var actionLink = link as HalActionLinkAttribute;
                    if(actionLink != null)
                    {
                        var doc = await endpointDocBuilder.GetDoc(actionLink.GroupName, actionLink.Method, actionLink.UriTemplate.Substring(1));
                        clientDef.AddLink(new EndpointClientLinkDefinition(actionLink.Rel, doc, actionLink.DocsOnly));
                    }
                    else
                    {
                        var declaredLink = link as DeclareHalLinkAttribute;
                        if(declaredLink != null)
                        {
                            EndpointDoc doc;
                            if (declaredLink.LinkedToControllerRel)
                            {
                                doc = await endpointDocBuilder.GetDoc(declaredLink.GroupName, declaredLink.Method, declaredLink.UriTemplate.Substring(1));
                            }
                            else
                            {
                                doc = new EndpointDoc();
                            }

                            //If the link is response only, send only the response
                            if (declaredLink.ResponseOnly)
                            {
                                var oldDoc = doc;
                                doc = new EndpointDoc();
                                doc.ResponseSchema = oldDoc.ResponseSchema;
                            }

                            clientDef.AddLink(new EndpointClientLinkDefinition(declaredLink.Rel, doc, false));
                        }
                    }
                }
                definitions.Add(clientDef);
            }
            return definitions;
        }
    }
}
