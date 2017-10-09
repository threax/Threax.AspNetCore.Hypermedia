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

        public static String CreatePretty(String name)
        {
            if (name == null || name.Length == 0)
            {
                return name;
            }

            bool currentLower = false;
            bool lastLower = false;
            var title = new StringBuilder(name.Length + 5); //Slightly larger buffer, wont have to reallocate unless there are more than 5 spaces that need to be added.
            title.Append(char.ToUpper(name[0]));
            for (int i = 1; i < name.Length; ++i) //Add a space between each transition from lower case to upper case in the name
            {
                var current = name[i];
                currentLower = char.IsLower(current);
                if (lastLower && !currentLower)
                {
                    title.Append(" ");
                    current = char.ToUpper(current);
                }
                lastLower = currentLower;
                title.Append(current);
            }

            return title.ToString();
        }
    }
}
