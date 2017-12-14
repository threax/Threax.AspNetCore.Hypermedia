using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    /// <summary>
    /// Determine if the tracking properties should be included on the model.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TrackingAttribute : JsonSchemaExtensionDataAttribute
    {
        internal class Settings
        {
            public bool Created { get; set; }

            public bool Modified { get; set; }
        }

        internal const String Name = "x-tracking";

        public TrackingAttribute(bool created = true, bool modified = true) : base(Name, new Settings() { Created = created, Modified = modified })
        {
        }
    }

    public static class TrackingAttributeJsonExtensions
    {
        public static bool AllowCreated(this JsonSchema4 schema)
        {
            Object val = null;
            if (schema.ExtensionData?.TryGetValue(TrackingAttribute.Name, out val) == true)
            {
                var settings = val as TrackingAttribute.Settings;
                if (settings != null)
                {
                    return settings.Created;
                }
            }
            return true;
        }

        public static bool AllowModified(this JsonSchema4 schema)
        {
            Object val = null;
            if (schema.ExtensionData?.TryGetValue(TrackingAttribute.Name, out val) == true)
            {
                var settings = val as TrackingAttribute.Settings;
                if (settings != null)
                {
                    return settings.Modified;
                }
            }
            return true;
        }
    }
}
