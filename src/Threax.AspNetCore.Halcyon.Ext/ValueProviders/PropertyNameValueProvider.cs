using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext.ValueProviders
{
    /// <summary>
    /// Use this attribute to make the PropertyNameValueProvider skip this property
    /// when creating label pairs.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NoPropertyLabelAttribute : Attribute
    {

    }

    /// <summary>
    /// This value provider will create label value pairs for the properties of the given type.
    /// Any properties marked with a NoPropertyLabelAttribute will be skipped. This will use the
    /// generated doc schema, so you will get the same auto naming benefits you get normally.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PropertyNameValueProvider<T> : LabelValuePairProvider
    {
        private ISchemaBuilder schemaBuilder;

        public PropertyNameValueProvider(ISchemaBuilder schemaBuilder)
        {
            this.schemaBuilder = schemaBuilder;
        }

        protected override async Task<IEnumerable<ILabelValuePair>> GetSources()
        {
            var labels = new List<ILabelValuePair>();
            JsonProperty schemaProp;
            var schema = await schemaBuilder.GetSchema(typeof(T));
            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.GetCustomAttributes(typeof(NoPropertyLabelAttribute), true).Any())
                {
                    continue;
                }

                //Only add properties that can be found in the schema
                var schemaPropName = schemaBuilder.GetPropertyName(prop);
                if (schema.Properties.TryGetValue(schemaPropName, out schemaProp))
                {
                    var label = schemaProp.Title ?? prop.Name;
                    labels.Add(new LabelValuePair<String>(label, prop.Name));
                }
            }
            return labels;
        }
    }
}
