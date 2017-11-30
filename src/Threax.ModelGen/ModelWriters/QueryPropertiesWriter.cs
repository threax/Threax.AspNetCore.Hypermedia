using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    class QueryPropertiesWriter : AbstractTypeWriter
    {
        private String visibility;

        public QueryPropertiesWriter(String visibility = "public ")
        {
            this.visibility = visibility;
        }

        public override String CreateProperty(String name, IWriterPropertyInfo info)
        {
            return $"        {CreateQueryRequired(info, name)}{visibility}{info.ClrType}{CreateQueryNullable(info)} {name} {{ get; set; }}";
        }

        public static String CreateQueryRequired(IWriterPropertyInfo info, String name)
        {
            return !info.IsValueType && info.IsRequiredInQuery ? $@"[Required(ErrorMessage = ""You must provide a {name}."")]
        " : String.Empty;
        }

        public static String CreateQueryNullable(IWriterPropertyInfo info)
        {
            return info.IsValueType && !info.IsRequiredInQuery ? "?" : String.Empty;
        }
    }
}
