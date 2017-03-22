using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NJsonSchema;
using Newtonsoft.Json;
using System.IO;

namespace Threax.AspNetCore.Halcyon.Ext.ValueProviders
{
    /// <summary>
    /// Helper base class to create extension data with key value pairs
    /// that go into an "x-values" attribute by default.
    /// </summary>
    public abstract class LabelValuePairProvider : IValueProvider
    {
        public LabelValuePairProvider()
        {
            
        }

        public void AddExtensions(JsonProperty schemaProp, ValueProviderArgs args)
        {
            var name = "x-values";
            if(args.ValueProviderAttr.PropertyName != null)
            {
                name = args.ValueProviderAttr.PropertyName;
            }

            var sources = GetSources();

            if (schemaProp.ExtensionData == null)
            {
                schemaProp.ExtensionData = new Dictionary<String, Object>();
            }

            schemaProp.ExtensionData.Add(name, sources);
        }

        protected abstract IEnumerable<LabelValuePair> GetSources();
    }
}
