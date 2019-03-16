using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon.Templates
{
    public static class TemplateExtensions
    {
        private static char openingDelimiter = '{';
        private static char closingDelimiter = '}';

        public static string SubstituteParams(this string uriTemplateString, IDictionary<string, object> parameters)
        {
            //This is not as robust as the older version based on Tavis.UriTemplates it only supports brackets around simple replacement variables.

            var text = uriTemplateString;
            StringBuilder output = new StringBuilder(text.Length);
            int textStart = 0;
            int bracketStart = 0;
            int bracketEnd = 0;
            int bracketCount = 0;
            int bracketCheck = 0;
            String variable;
            String bracketVariable;
            Object value;
            for (var i = 0; i < text.Length; ++i)
            {
                if (text[i] == openingDelimiter)
                {
                    //Count up opening brackets
                    bracketStart = i;
                    bracketCount = 1;
                    while (++i < text.Length && text[i] == openingDelimiter)
                    {
                        ++bracketCount;
                    }

                    //Find closing bracket chain, ignore if mismatched or whitespace
                    bracketCheck = bracketCount;
                    while (++i < text.Length)
                    {
                        if ((text[i] == closingDelimiter && --bracketCheck == 0) || Char.IsWhiteSpace(text[i]))
                        {
                            break;
                        }
                    }

                    //If the check got back to 0 we found a variable
                    if (bracketCheck == 0)
                    {
                        //Output everything up to this point
                        output.Append(text.Substring(textStart, bracketStart - textStart));
                        bracketEnd = i;
                        bracketVariable = text.Substring(bracketStart, bracketEnd - bracketStart + 1);

                        switch (bracketCount)
                        {
                            case 1:
                                variable = bracketVariable.Substring(bracketCount, bracketVariable.Length - bracketCount * 2);
                                if (parameters.TryGetValue(variable, out value))
                                {
                                    //Only output something if it is found
                                    output.Append(System.Net.WebUtility.UrlEncode(value?.ToString()));
                                    //throw new InvalidOperationException($"Tried to add a variable {variable} that was not found in the source data.");
                                }

                                break;
                            default:
                                //Anything except 1 is an error
                                throw new InvalidOperationException($"Multiple brackets found when processing '{uriTemplateString}' Only one bracket is allowed around your variables. {{example}}.");
                        }

                        textStart = i + 1;
                    }
                }
            }

            if (textStart < text.Length)
            {
                output.Append(text.Substring(textStart, text.Length - textStart));
            }
            return output.ToString();
        }
    }
}
