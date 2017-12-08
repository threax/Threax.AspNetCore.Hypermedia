using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    public static class NameGenerator
    {
        public static void CreatePascalAndCamel(String modelName, out String pascal, out String camel)
        {
            pascal = CreatePascal(modelName);
            camel = CreateCamel(modelName);
        }

        public static string CreatePascal(string modelName)
        {
            if (modelName == null)
            {
                return "Null";
            }
            if (String.IsNullOrWhiteSpace(modelName))
            {
                return "Empty";
            }
            var modelSuffix = modelName.Length > 0 ? modelName.Substring(1) : "";
            return modelName[0].ToString().ToUpperInvariant() + modelSuffix;
        }

        public static string CreateCamel(string modelName)
        {
            if (modelName == null)
            {
                return "null";
            }
            if (String.IsNullOrWhiteSpace(modelName))
            {
                return "empty";
            }
            var modelSuffix = modelName.Length > 0 ? modelName.Substring(1) : "";
            return EscapeKeyword(modelName[0].ToString().ToLowerInvariant() + modelSuffix);
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

        /// <summary>
        /// Escape a single word if it is a c# keyword by prepending @.
        /// </summary>
        /// <param name="word">The word to escape.</param>
        /// <returns>Word back or the escaped version if needed.</returns>
        public static String EscapeKeyword(String word)
        {
            if (keywords.Contains(word))
            {
                return "@" + word;
            }
            return word;
        }

        private static HashSet<String> keywords = new HashSet<String>()
        {
"abstract",
"as",
"base",
"bool",
"break",
"byte",
"case",
"catch",
"char",
"checked",
"class",
"const",
"continue",
"decimal",
"default",
"delegate",
"do",
"double",
"else",
"enum",
"event",
"explicit",
"extern",
"false",
"finally",
"fixed",
"float",
"for",
"forech",
"goto",
"if",
"implicit",
"in",
"int",
"interface",
"internal",
"is",
"lock",
"long",
"namespace",
"new",
"null",
"object",
"operator",
"out",
"override",
"params",
"private",
"protected",
"public",
"readonly",
"ref",
"return",
"sbyte",
"sealed",
"short",
"sizeof",
"stackalloc",
"static",
"string",
"struct",
"switch",
"this",
"throw",
"true",
"try",
"typeof",
"uint",
"ulong",
"unchecked",
"unsafe",
"ushort",
"using",
"virtual",
"volatile",
"void",
"while"
        };
    }
}
