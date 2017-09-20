using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    static class NameGenerator
    {
        public static void CreatePascalAndCamel(String modelName, out String pascal, out String camel)
        {
            if(modelName == null)
            {
                pascal = "Null";
                camel = "null";
                return;
            }
            if (String.IsNullOrWhiteSpace(modelName))
            {
                pascal = "Empty";
                camel = "empty";
                return;
            }
            var modelSuffix = modelName.Length > 0 ? modelName.Substring(1) : "";
            pascal = modelName[0].ToString().ToUpperInvariant() + modelSuffix;
            camel = modelName[0].ToString().ToLowerInvariant() + modelSuffix;
        }

        public static string CreatePascal(string modelName)
        {
            var modelSuffix = modelName.Length > 0 ? modelName.Substring(1) : "";
            return modelName[0].ToString().ToUpperInvariant() + modelSuffix;
        }

        public static string CreateCamel(string modelName)
        {
            var modelSuffix = modelName.Length > 0 ? modelName.Substring(1) : "";
            return modelName[0].ToString().ToLowerInvariant() + modelSuffix;
        }
    }
}
