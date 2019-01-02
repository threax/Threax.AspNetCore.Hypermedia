using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;
using Threax.ModelGen.ModelWriters;

namespace Threax.ModelGen
{
    public static class ModelCollectionGenerator
    {
        public static String Get(JsonSchema4 schema, String ns, bool generated)
        {
            if (schema.CreateWithAutoQueryCollection())
            {
                if (generated)
                {
                    return AutoModelCollectionGenerator.Get(schema, ns);
                }
                else
                {
                    return AutoModelCollectionGenerator.GetComplete(schema, ns);
                }
            }
            else
            {
                if (!generated)
                {
                    throw new NotImplementedException("Non generated template not available for manual mode collections. Please use an auto collection.");
                }
                return ManualModelCollectionGenerator.Get(schema, ns);
            }
        }

        public static String GetFileName(JsonSchema4 schema, bool generated)
        {
            var genStr = generated ? ".Generated" : "";
            return $"ViewModels/{schema.Title}Collection{genStr}.cs";
        }

        public static String GetUserPartial(JsonSchema4 schema, String ns, String generatedSuffix = ".Generated")
        {
            if (schema.CreateWithAutoQueryCollection())
            {
                return AutoModelCollectionGenerator.GetUserPartial(schema, ns, generatedSuffix);
            }
            else
            {
                return ManualModelCollectionGenerator.GetUserPartial(schema, ns, generatedSuffix);
            }
        }
    }
}
