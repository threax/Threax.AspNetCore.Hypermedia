using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;

namespace Threax.AspNetCore.Halcyon.ClientGen
{
    public class ClientGenerator : IClientGenerator
    {
        IResultViewProvider resultViewProvider;
        IEndpointDocBuilder endpointDocBuilder;

        public ClientGenerator(IResultViewProvider resultViewProvider, IEndpointDocBuilder endpointDocBuilder)
        {
            this.resultViewProvider = resultViewProvider;
            this.endpointDocBuilder = endpointDocBuilder;
        }

        public IEnumerable<EndpointClientDefinition> GetEndpointDefinitions()
        {
            foreach(var type in resultViewProvider.GetResultViewTypes())
            {
                EndpointClientDefinition clientDef = new EndpointClientDefinition(type);
                var customAttrs = type.GetTypeInfo().GetCustomAttributes();
                foreach (var link in customAttrs)
                {
                    var actionLink = link as HalActionLinkAttribute;
                    if(actionLink != null)
                    {
                        var doc = endpointDocBuilder.GetDoc(actionLink.GroupName, actionLink.Method, actionLink.UriTemplate.Substring(1));
                        clientDef.Links.Add(new EndpointClientLinkDefinition(actionLink.Rel, doc, actionLink.DocsOnly));
                    }
                    else
                    {
                        var declaredLink = link as DeclareHalLinkAttribute;
                        if(declaredLink != null)
                        {
                            EndpointDoc doc;
                            if (declaredLink.LinkedToControllerRel)
                            {
                                doc = endpointDocBuilder.GetDoc(declaredLink.GroupName, declaredLink.Method, declaredLink.UriTemplate.Substring(1));
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

                            clientDef.Links.Add(new EndpointClientLinkDefinition(declaredLink.Rel, doc, false));
                        }
                    }
                }
                yield return clientDef;
            }
        }
    }
}
