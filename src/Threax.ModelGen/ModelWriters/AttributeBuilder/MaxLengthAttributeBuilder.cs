using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    public class MaxLengthAttributeBuilder : AttributeBuilderChain
    {
        public MaxLengthAttributeBuilder(IAttributeBuilder next = null) : base(next)
        {
        }

        public override void BuildAttributes(StringBuilder sb, string name, IWriterPropertyInfo prop, string spaces = "        ")
        {
            if (prop.MaxLength.HasValue)
            {
                sb.AppendLine(GetMaxLength(prop.MaxLength.Value, GetMaxLengthError(name, prop.MaxLength.Value), spaces));
            }
            base.BuildAttributes(sb, name, prop, spaces);
        }

        public virtual String GetMaxLengthError(String name, int length)
        {
            return $"{NameGenerator.CreatePretty(name)} must be less than {length} characters.";
        }

        public virtual String GetMaxLength(int length, String errorMessage, String spaces)
        {
            return $@"{spaces}[MaxLength({length}, ErrorMessage = ""{errorMessage}"")]";
        }
    }
}
