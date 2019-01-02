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
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CreateGeneratedFilesAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "x-create-generated-files";

        public CreateGeneratedFilesAttribute(bool create) : base(Name, create)
        {
        }
    }

    public static class CreateGeneratedFilesAttributeJsonSchemaExtensions
    {
        /// <summary>
        /// Returns true if the schema / property should create an entity.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static bool CreateGeneratedFiles(this JsonSchema4 schema)
        {
            Object val = null;
            schema.ExtensionData?.TryGetValue(CreateGeneratedFilesAttribute.Name, out val);
            return val as bool? == true;
        }
    }
}
