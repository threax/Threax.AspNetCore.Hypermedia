using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    /// <summary>
    /// This attribute will make the controllers require the specified role for access.
    /// There are 2 constructors for this class, it is reccomended to use the one that takes 2 arguments.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RequireAuthorizationAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "x-require-auth";

        /// <summary>
        /// Setup a role using a role class and property. This will write your role as RoleType.RoleName instead
        /// of directly writing the role string. This will allow your code to adapt if you change the role names.
        /// The reccomended way to call this is [RequireAuthorization(typeof(Roles), nameof(Roles.TargetRole))]
        /// </summary>
        /// <param name="roleType"></param>
        /// <param name="roleName"></param>
        public RequireAuthorizationAttribute(Type roleType, String roleName) : base(Name, $"{roleType.Name}.{roleName}")
        {
        }

        /// <summary>
        /// Require a specific hard coded role name. This constructor is not reccomended, use the other one.
        /// </summary>
        /// <param name="roleName"></param>
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
