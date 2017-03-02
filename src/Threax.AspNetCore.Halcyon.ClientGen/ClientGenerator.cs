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
                EndpointClientDefinition clientDef = new EndpointClientDefinition(type.Name + "ResultView");
                foreach(var link in type.GetTypeInfo().GetCustomAttributes().Where(i => i is HalActionLinkAttribute).Select(i => i as HalActionLinkAttribute))
                {
                    var doc = endpointDocBuilder.GetDoc(link.GroupName, link.Method, link.UriTemplate);
                    clientDef.Links.Add(new EndpointClientLinkDefinition(link.Rel, doc));
                }
                yield return clientDef;
            }
        }
    }
}
