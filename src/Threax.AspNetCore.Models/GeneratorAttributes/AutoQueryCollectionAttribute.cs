using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    /// <summary>
    /// If this is set to false the older PagedCollectionView will be used when generating the model instead
    /// of the PagedCollectionViewWithQuery that builds the queries with reflection.
    /// By default (including if there is no attribute) this will be true to generate the auto query collections.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class AutoQueryCollectionAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "x-auto-query-collection";

        public AutoQueryCollectionAttribute(bool auto) : base(Name, auto)
        {
        }
    }

    public static class AutoQueryCollectionAttributeJsonSchemaExtensions
    {
        /// <summary>
        /// Returns true if the schema / property should create an entity.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static bool CreateWithAutoQueryCollection(this JsonSchema4 schema)
        {
            Object val = null;
            schema.ExtensionData?.TryGetValue(AutoQueryCollectionAttribute.Name, out val);
            return val as bool? != false;
        }
    }
}
