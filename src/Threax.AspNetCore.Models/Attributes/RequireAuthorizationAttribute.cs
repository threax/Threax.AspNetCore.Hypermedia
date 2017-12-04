using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RequireAuthorizationAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "x-require-auth";

        public RequireAuthorizationAttribute(Type roleType, String roleName) : base(Name, $"{roleType.Name}.{roleName}")
        {
        }

        public RequireAuthorizationAttribute(String roleName) : base(Name, $"\"{roleName}\"")
        {
        }
    }

    public static class RequireAuthorizationAttributeJsonSchemaExtensions
    {
        /// <summary>
        /// Get the string to use for roles in an attribute string. Can return null if nothing is to be used.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static String GetAuthorizationRoleString(this JsonSchema4 schema)
        {
            Object val = null;
            schema.ExtensionData?.TryGetValue(RequireAuthorizationAttribute.Name, out val);
            return val?.ToString();
        }
    }
}
