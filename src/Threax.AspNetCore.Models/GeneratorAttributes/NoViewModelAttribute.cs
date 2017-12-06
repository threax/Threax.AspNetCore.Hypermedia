using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class NoViewModelAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "x-no-view-model";

        public NoViewModelAttribute(bool remove = true) : base(Name, remove)
        {
        }
    }

    public static class NoViewModelAttributeJsonSchemaExtensions
    {
        /// <summary>
        /// Returns true if the schema / property should create a view model.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static bool CreateViewModel(this JsonSchema4 schema)
        {
            Object val = null;
            schema.ExtensionData?.TryGetValue(NoViewModelAttribute.Name, out val);
            return val as bool? != true;
        }
    }
}
