using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class KeyTypeAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "x-key-type";

        public KeyTypeAttribute(Type keyType) : base(Name, keyType.FullName)
        {
        }
    }

    public static class KeyTypeAttributeJsonSchemaExtensions
    {
        /// <summary>
        /// Get the plural name of a schema. This will always return something either the value of the
        /// x-plural-title extension data or the schema.Title with an s appended.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static Type GetKeyType(this JsonSchema4 schema)
        {
            Object val = null;
            if(schema.ExtensionData?.TryGetValue(KeyTypeAttribute.Name, out val) != true)
            {
                return typeof(Guid);
            }
            var keyType = val.ToString();
            foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var type = assembly.GetType(keyType, false);
                if(type != null)
                {
                    return type;
                }
            }
            return Type.GetType(keyType); //Fallback to this, it probably won't work and will throw an error
        }
    }
}
