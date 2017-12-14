using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Threax.ModelGen
{
    public static class TypeToSchemaGenerator
    {
        public static async Task<JsonSchema4> CreateSchema(Type type)
        {
            var schema = await JsonSchema4.FromTypeAsync(type, new NJsonSchema.Generation.JsonSchemaGeneratorSettings()
            {
                DefaultEnumHandling = EnumHandling.String,
                FlattenInheritanceHierarchy = true
            });

            //Modify the schema to make each none or object property aware of its original type
            foreach (var schemaProp in schema.Properties.Values)
            {
                var prop = type.GetProperty(schemaProp.Name);
                if (prop == null)
                {
                    throw new InvalidOperationException($"Cannot find property {schemaProp.Name} from generated schema on type {type.FullName}");
                }
                var propType = prop.PropertyType;
                schemaProp.SetClrFullTypeName(propType.FullName);

                if (schemaProp.IsType(JsonObjectType.None) || schemaProp.IsType(JsonObjectType.Object) || propType == typeof(Guid?))
                {
                    schemaProp.Type = JsonObjectType.Object;
                    schemaProp.Format = propType.GetSchemaFormat();

                    //If we allow collections check the collection type
                    if (typeof(System.Collections.IEnumerable).IsAssignableFrom(propType) && propType.GenericTypeArguments.Length > 0)
                    {
                        var enumerableType = propType.GenericTypeArguments.First();
                        schemaProp.Type = JsonObjectType.Array;
                        schemaProp.Item = new JsonProperty()
                        {
                            Type = JsonObjectType.Object,
                            Format = propType.GetSchemaFormat()
                        };
                    }

                    //For some reason enums do not get the custom attributes, so do it here
                    if (propType.IsEnum)
                    {
                        foreach (var attr in prop.GetCustomAttributes().Select(i => i as JsonSchemaExtensionDataAttribute).Where(i => i != null))
                        {
                            if (!schemaProp.ExtensionData.ContainsKey(attr.Property))
                            {
                                schemaProp.ExtensionData.Add(attr.Property, attr.Value);
                            }
                        }
                    }
                }
            }

            return schema;
        }
    }
}
