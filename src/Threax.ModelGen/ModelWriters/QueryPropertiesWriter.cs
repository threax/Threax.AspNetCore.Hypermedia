using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    class QueryPropertiesWriter : AbstractTypeWriter
    {
        private String visibility;
        private bool allowAttributes;
        private IAttributeBuilder attributeBuilder = CreateAttributeBuilder();

        public QueryPropertiesWriter(String visibility = "public ", bool allowAttributes = true)
        {
            this.visibility = visibility;
            this.allowAttributes = allowAttributes;
        }

        public override void CreateProperty(StringBuilder sb, String name, IWriterPropertyInfo info)
        {
            if (allowAttributes)
            {
                attributeBuilder.BuildAttributes(sb, name, info, "        ");
            }
            sb.AppendLine($"        {visibility}{info.ClrType}{CreateQueryNullable(info)} {name} {{ get; set; }}");
            sb.AppendLine();
        }

        public static String CreateQueryNullable(IWriterPropertyInfo info)
        {
            return info.IsValueType && !info.IsRequiredInQuery ? "?" : String.Empty;
        }

        public static IAttributeBuilder CreateAttributeBuilder()
        {
            return new NullValueLabelAttributeBuilder(
                new PredefinedAttributeBuilder("[UiOrder]",
                    new DisplayAttributeBuilder(
                        new UiSearchAttributeBuilder(
                            new RequiredInQueryAttributeBuilder()))));
        }
    }
}
