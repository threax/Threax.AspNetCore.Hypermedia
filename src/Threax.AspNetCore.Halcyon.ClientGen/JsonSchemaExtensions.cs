using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Threax.AspNetCore.Halcyon.ClientGen
{
    public static class JsonSchemaExtensions
    {
        public static string FindSchemaNameInParent(this JsonSchema4 schema, String typeNameHint)
        {
            //If there is a parent schema, go to it to see if we can find the original type name since the hint will be butchered
            var parent = schema.ParentSchema;
            while (parent != null)
            {
                //Find this schema in the parent definitions
                foreach (var def in parent?.Definitions ?? Enumerable.Empty<KeyValuePair<String, JsonSchema4>>())
                {
                    if (def.Value == schema)
                    {
                        return def.Key;
                    }
                }
                parent = parent.ParentSchema;
            }

            //If that didn't work we might have a property that matches a schema but its not the same instance
            //For this do a search up the parents for a definition that removes the parent name from the type hint name
            //This is less likely to work than the above and may cause problems
            //Only do this for JsonProperty instances
            if (schema is JsonProperty)
            {
                var fullParentName = "";
                parent = schema.ParentSchema;
                while (parent != null)
                {
                    fullParentName += parent.Title;
                    parent = parent.ParentSchema;
                }

                if (!String.IsNullOrEmpty(fullParentName))
                {
                    var partialName = typeNameHint.Replace(fullParentName, ""); //Remove the full parent name from the type name hint
                    parent = schema.ParentSchema;
                    while (parent != null)
                    {
                        if (parent.Definitions?.ContainsKey(partialName) == true)
                        {
                            return partialName;
                        }
                        parent = parent.ParentSchema;
                    }
                }
            }

            return null;
        }
    }
}
