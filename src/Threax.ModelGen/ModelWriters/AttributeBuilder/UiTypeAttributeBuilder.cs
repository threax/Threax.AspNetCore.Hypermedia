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
            if (!String.IsNullOrEmpty(prop.UiType))
            {
                sb.AppendLine(GetAttribute(prop.UiType, spaces));
            }
            base.BuildAttributes(sb, name, prop, spaces);
        }

        public static String GetAttribute(String uiType, String spaces)
        {
            switch (uiType)
            {
                case CheckboxUiTypeAttribute.UiName:
                    return $@"{spaces}[CheckboxUiType]";
                case DateUiTypeAttribute.UiName:
                    return $@"{spaces}[DateUiType]";
                case HiddenUiTypeAttribute.UiName:
                    return $@"{spaces}[HiddenUiType]";
                case PasswordUiTypeAttribute.UiName:
                    return $@"{spaces}[PasswordUiType]";
                case SelectUiTypeAttribute.UiName:
                    return $@"{spaces}[SelectUiType]";
                case TextAreaUiTypeAttribute.UiName:
                    return $@"{spaces}[TextAreaUiType]";
            }
            return $@"{spaces}[UiType(""{uiType}"")]";
        }
    }
}
