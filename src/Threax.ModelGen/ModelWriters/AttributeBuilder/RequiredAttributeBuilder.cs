using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    public class RequiredAttributeBuilder : AttributeBuilderChain
    {
        public RequiredAttributeBuilder(IAttributeBuilder next = null) : base(next)
        {
        }

        public override void BuildAttributes(StringBuilder sb, string name, IWriterPropertyInfo prop, string spaces = "        ")
        {
            if (prop.HasRequiredAttribute)
            {
                sb.AppendLine(GetRequired(GetRequiredError(name), spaces));
            }
            base.BuildAttributes(sb, name, prop, spaces);
        }

        public virtual String GetRequiredError(String name)
        {
            return $"{NameGenerator.CreatePretty(name)} must have a value.";
        }

        public virtual String GetRequired(String errorMessage, String spaces)
        {
            return $@"{spaces}[Required(ErrorMessage = ""{errorMessage}"")]";
        }
    }
}
