using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AddNamespacesAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "x-add-namespaces";

        public AddNamespacesAttribute(String ns) : base(Name, ns)
        {
            
        }
    }

    public static class AddNamespacesAttributeJsonSchemaExtensions
    {
        public static String GetExtraNamespaces(this JsonSchema4 schema, String prefix = null)
        {
            Object val = null;
            schema.ExtensionData?.TryGetValue(AddNamespacesAttribute.Name, out val);
            return val != null ? prefix + val : default(String);
        }
    }
}
