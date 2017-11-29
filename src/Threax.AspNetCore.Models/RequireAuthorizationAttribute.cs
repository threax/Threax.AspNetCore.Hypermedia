using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RequireAuthorizationAttribute : JsonSchemaExtensionDataAttribute
    {
        public const String Name = "x-require-auth";

        public RequireAuthorizationAttribute(Type roleType, String roleName) : base(Name, $"{roleType.Name}.{roleName}")
        {
        }

        public RequireAuthorizationAttribute(String roleName) : base(Name, $"\"{roleName}\"")
        {
        }
    }
}
