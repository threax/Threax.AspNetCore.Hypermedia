using Halcyon.HAL.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;

namespace Threax.AspNetCore.Halcyon.ClientGen
{
    public class DefaultResultViewProvider : IResultViewProvider
    {
        private List<Assembly> searchAssemblies = new List<Assembly>();

        public DefaultResultViewProvider(IEnumerable<Assembly> assemblies)
        {
            searchAssemblies.AddRange(assemblies);
        }

        public IEnumerable<Type> GetResultViewTypes()
        {
            return AllTypes().Where(i =>
            {
                var attrs = i.GetTypeInfo().GetCustomAttributes();
                return attrs.Any(j => j is HalModelAttribute) && attrs.Any(j => j is HalActionLinkAttribute); //Return stuff that is a hal model and has at least one action link
            });
        }

        private IEnumerable<Type> AllTypes()
        {
            foreach (Assembly a in searchAssemblies)
            {
                foreach (Type t in a.GetTypes())
                {
                    yield return t;
                }
            }
        }
    }
}
