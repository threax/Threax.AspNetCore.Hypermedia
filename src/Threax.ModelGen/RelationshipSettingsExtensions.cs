using Threax.NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    public static class RelationshipSettingsExtensions
    {
        public static Dictionary<String, Object> CopyExtensions(this RelationshipSettings attribute)
        {
            var dictionary = default(Dictionary<string, object>);
            if (attribute.OriginalPropertyDefinition != null)
            {
                if (attribute.OriginalPropertyDefinition.ExtensionData != null)
                {
                    dictionary = new Dictionary<string, object>(attribute.OriginalPropertyDefinition.ExtensionData);
                    if (attribute.OriginalPropertyDefinition.Type == JsonObjectType.Object && attribute.OriginalPropertyDefinition.ActualTypeSchema?.ExtensionData != null)
                    {
                        //If this is an object, add any properties from the "ActualTypeSchema" which often has extras
                        foreach (var item in attribute.OriginalPropertyDefinition.ActualTypeSchema.ExtensionData)
                        {
                            dictionary[item.Key] = item.Value;
                        }
                    }
                }
            }
            return dictionary;
        }
    }
}
