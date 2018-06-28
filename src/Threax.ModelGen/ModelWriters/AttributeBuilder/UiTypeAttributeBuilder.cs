using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen.ModelWriters
{
    class UiTypeAttributeBuilder : AttributeBuilderChain
    {
        public UiTypeAttributeBuilder(IAttributeBuilder next = null) : base(next)
        {
        }

        public override void BuildAttributes(StringBuilder sb, string name, IWriterPropertyInfo prop, string spaces = "        ")
        {
            if (prop.UiType != null)
            {
                sb.AppendLine($"{spaces}{prop.UiType.CreateAttribute()}");
            }
            base.BuildAttributes(sb, name, prop, spaces);
        }
    }
}
