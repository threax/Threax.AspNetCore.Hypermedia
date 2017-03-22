using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NJsonSchema;
using Newtonsoft.Json;
using System.IO;

namespace Threax.AspNetCore.Halcyon.Ext.ValueProviders
{
    public class EnumValue
    {
        public EnumValue()
        {

        }

        public EnumValue(String title, String value)
        {
            this.Title = title;
            this.Value = value;
        }

        public String Title { get; set; }

        public String Value { get; set; }
    }

    /// <summary>
    /// Helper base class to create extension data with key value pairs
    /// that go into an "x-enumSource" attribute by default.
    /// </summary>
    public abstract class EnumValueProvider : IValueProvider
    {
        class EnumSource
        {
            public EnumSource(IEnumerable<EnumValue> source)
            {
                this.Source = source;
            }

            public IEnumerable<EnumValue> Source { get; }

            public String Title
            {
                get
                {
                    return "{{item.title}}";
                }
            }

            public String Value
            {
                get
                {
                    return "{{item.value}}";
                }
            }
        }

        public EnumValueProvider()
        {
            
        }

        public void AddExtensions(JsonProperty schemaProp, ValueProviderArgs args)
        {
            var name = "x-enumSource";
            if(args.ValueProviderAttr.PropertyName != null)
            {
                name = args.ValueProviderAttr.PropertyName;
            }

            var sources = GetSources();
            var source = new EnumSource(sources);

            if (schemaProp.ExtensionData == null)
            {
                schemaProp.ExtensionData = new Dictionary<String, Object>();
            }

            schemaProp.ExtensionData.Add(name, source);
        }

        protected abstract IEnumerable<EnumValue> GetSources();
    }
}
