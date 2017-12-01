using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    public class NoAttributeBuilder : IAttributeBuilder
    {
        public void BuildAttributes(StringBuilder sb, string name, IWriterPropertyInfo prop, string spaces = "        ")
        {
            
        }
    }

    public class AttributeBuilder : IAttributeBuilder
    {
        public virtual void BuildAttributes(StringBuilder sb, String name, IWriterPropertyInfo prop, String spaces = "        ")
        {
            if (BuildMaxLength && prop.MaxLength.HasValue)
            {
                sb.AppendLine(GetMaxLength(prop.MaxLength.Value, GetMaxLengthError(name, prop.MaxLength.Value), spaces));
            }

            if (BuildRequired && prop.HasRequiredAttribute)
            {
                sb.AppendLine(GetRequired(GetRequiredError(name), spaces));
            }

            if (BuildDisplay && !String.IsNullOrEmpty(prop.DisplayName))
            {
                sb.AppendLine(GetDisplay(prop.DisplayName, spaces));
            }
        }

        public bool BuildMaxLength { get; set; } = true;

        public bool BuildRequired { get; set; } = true;

        public bool BuildDisplay { get; set; } = true;

        public static String GetMaxLengthError(String name, int length)
        {
            return $"{NameGenerator.CreatePretty(name)} must be less than {length} characters.";
        }

        public static String GetMaxLength(int length, String errorMessage, String spaces)
        {
            return $@"{spaces}[MaxLength({length}, ErrorMessage = ""{errorMessage}"")]";
        }

        public static String GetRequiredError(String name)
        {
            return $"{NameGenerator.CreatePretty(name)} must have a value.";
        }

        public static String GetRequired(String errorMessage, String spaces)
        {
            return $@"{spaces}[Required(ErrorMessage = ""{errorMessage}"")]";
        }

        public static String GetDisplay(String displayName, String spaces)
        {
            return $@"{spaces}[Display(Name = ""{displayName}"")]";
        }
    }
}
