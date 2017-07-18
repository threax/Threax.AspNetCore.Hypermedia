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
    /// that go into an "x-values" attribute by default. This version is good
    /// if you need to do additional async work otherwise use the LabelValuePairProviderSync
    /// to have less boilerplate code.
    /// </summary>
    public abstract class LabelValuePairProvider : IValueProvider
    {
        public LabelValuePairProvider()
        {
            
        }

        public async Task AddExtensions(JsonProperty schemaProp, ValueProviderArgs args)
        {
            var name = "x-values";
            if(args.ValueProviderAttr.PropertyName != null)
            {
                name = args.ValueProviderAttr.PropertyName;
            }

            var sources = await GetSources();

            if (schemaProp.ExtensionData == null)
            {
                schemaProp.ExtensionData = new Dictionary<String, Object>();
            }

            schemaProp.ExtensionData.Add(name, sources);
        }

        /// <summary>
        /// Get the LabelValuePairs for the data. You can make async calls here. If you know the data
        /// without needing async calls use LabelValuePairProviderSync instead.
        /// </summary>
        /// <returns></returns>
        protected abstract Task<IEnumerable<ILabelValuePair>> GetSources();
    }

    /// <summary>
    /// Helper base class to create extension data with key value pairs
    /// that go into an "x-values" attribute by default. This version is better
    /// to use if you won't need an async context to get your data.
    /// </summary>
    public abstract class LabelValuePairProviderSync : LabelValuePairProvider
    {
        public LabelValuePairProviderSync()
        {

        }

        /// <summary>
        /// Seal task based GetSources().
        /// </summary>
        /// <returns></returns>
        protected override sealed Task<IEnumerable<ILabelValuePair>> GetSources()
        {
            return Task.FromResult(GetSourcesSync());
        }

        /// <summary>
        /// Return LabelValuePairs from sync data. If you need to make async calls use the LabelValuePairProvider
        /// class instead.
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<ILabelValuePair> GetSourcesSync();
    }
}
