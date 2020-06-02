﻿using Threax.NJsonSchema;
using Threax.NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    /// <summary>
    /// Do not generate an input model for this class or do not include the marked property in the input model.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class NoMappingProfileAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "x-no-mapping-profile";

        public NoMappingProfileAttribute(bool remove = true) : base(Name, remove)
        {
        }
    }

    public static class NoMappingProfileAttributeJsonSchemaExtensions
    {
        /// <summary>
        /// Returns true if the schema / property should create an input model.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static bool CreateMappingProfile(this JsonSchema4 schema)
        {
            Object val = null;
            schema.ExtensionData?.TryGetValue(NoMappingProfileAttribute.Name, out val);
            return val as bool? != true;
        }
    }
}
