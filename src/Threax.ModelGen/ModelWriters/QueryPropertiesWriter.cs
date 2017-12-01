using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    class QueryPropertiesWriter : AbstractTypeWriter
    {
        private String visibility;
        private bool allowAttributes;

        public QueryPropertiesWriter(String visibility = "public ", bool allowAttributes = true)
        {
            this.visibility = visibility;
            this.allowAttributes = allowAttributes;
        }

        public override void CreateProperty(StringBuilder sb, String name, IWriterPropertyInfo info)
        {
            if (allowAttributes)
            {
                sb.AppendLineWithContent(CreateQueryRequired(info, name));
                sb.AppendLineWithContent(CreateQueryUiSearch(info, name));
            }

            if (!String.IsNullOrEmpty(info.DisplayName))
            {
                sb.AppendLine(AttributeBuilder.GetDisplay(info.DisplayName, "        "));
            }
            sb.AppendLine($"        {visibility}{info.ClrType}{CreateQueryNullable(info)} {name} {{ get; set; }}");
        }

        public static String CreateQueryRequired(IWriterPropertyInfo info, String name)
        {
            return !info.IsValueType && info.IsRequiredInQuery ? $@"[Required(ErrorMessage = ""You must provide a {name}."")]
        " : String.Empty;
        }

        public static String CreateQueryUiSearch(IWriterPropertyInfo info, String name)
        {
            return info.ShowOnQueryUi ? $@"        [UiSearch]
        [UiOrder]
        " : String.Empty;
        }

        public static String CreateQueryNullable(IWriterPropertyInfo info)
        {
            return info.IsValueType && !info.IsRequiredInQuery ? "?" : String.Empty;
        }
    }
}
