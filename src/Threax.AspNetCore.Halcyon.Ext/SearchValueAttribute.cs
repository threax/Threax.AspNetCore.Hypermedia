using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Threax.AspNetCore.Halcyon.Ext
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SearchValueAttribute : CustomizeSchemaAttribute, ISchemaCustomizer
    {
        class SearchValueProperty
        {
            public String Provider { get; set; }

            public String ValueProperty { get; set; }
        }

        private const String DisplayIfName = "x-search";
        private static readonly Newtonsoft.Json.Serialization.JsonProperty DummyProperty = new Newtonsoft.Json.Serialization.JsonProperty();

        public SearchValueAttribute(String searchProviderName) 
            : base(null)
        {
            this.SearchProviderName = searchProviderName;
        }

        /// <summary>
        /// The name of the property on the object that has the value that should be used to display this item. This can be null
        /// to have no current value property, but if you specify it it must exist.
        /// </summary>
        public String CurrentValuePropertyName { get; set; }

        /// <summary>
        /// The name of the search provider that will be used to search for values. What this is depends on the client side implementation.
        /// </summary>
        public String SearchProviderName { get; private set; }

        public Task Customize(SchemaCustomizerArgs args)
        {
            var schemaProp = args.SchemaProperty;

            if (schemaProp.ExtensionData == null)
            {
                schemaProp.ExtensionData = new Dictionary<String, Object>();
            }

            var searchValueProperty = new SearchValueProperty()
            {
                Provider = SearchProviderName
            };
            if (CurrentValuePropertyName != null)
            {
                var currentValueProp = args.Type.GetProperty(CurrentValuePropertyName);
                if(currentValueProp == null)
                {
                    throw new InvalidOperationException($"Cannot find a property on {args.Type.Name} named {CurrentValuePropertyName}. Please fix the SearchValueAttribute on that property.");
                }
                searchValueProperty.ValueProperty = args.SchemaGenerator.GetPropertyName(DummyProperty, currentValueProp); //Using dummy property here, the call in the superclass will look at the member info first (v9.9.10)
            }

            schemaProp.ExtensionData.Add(DisplayIfName, searchValueProperty);

            return Task.FromResult(0);
        }
    }
}
