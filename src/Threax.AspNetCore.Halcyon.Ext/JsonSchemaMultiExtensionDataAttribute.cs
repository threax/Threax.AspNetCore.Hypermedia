using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Ext
{
    /// <summary>
    /// Add multiple extension data attributes to the schema with one attribute.
    /// </summary>
    public class JsonSchemaMultiExtensionDataAttribute : Attribute
    {
        private Dictionary<String, Object> extensionValues;

        public JsonSchemaMultiExtensionDataAttribute(Dictionary<String, Object> extensionValues)
        {
            this.extensionValues = extensionValues;
        }

        public Dictionary<String, Object> ExtensionValues { get => extensionValues; }
    }
}
