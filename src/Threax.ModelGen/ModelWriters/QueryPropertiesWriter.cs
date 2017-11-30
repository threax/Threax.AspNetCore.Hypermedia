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

        public override String CreateProperty(String name, IWriterPropertyInfo info)
        {
            var required = allowAttributes ? CreateQueryRequired(info, name) : String.Empty;
            var uiSearch = allowAttributes ? CreateQueryUiSearch(info, name) : String.Empty;
            return $"        {required}{uiSearch}{visibility}{info.ClrType}{CreateQueryNullable(info)} {name} {{ get; set; }}";
        }

        public override String AddDisplay(String name)
        {
            return $@"        [Display(Name = ""{name}"")]";
        }

        public static String CreateQueryRequired(IWriterPropertyInfo info, String name)
        {
            return !info.IsValueType && info.IsRequiredInQuery ? $@"[Required(ErrorMessage = ""You must provide a {name}."")]
        " : String.Empty;
        }

        public static String CreateQueryUiSearch(IWriterPropertyInfo info, String name)
        {
            return info.ShowOnQueryUi ? $@"[UiSearch]
        [UiOrder]
        " : String.Empty;
        }

        public static String CreateQueryNullable(IWriterPropertyInfo info)
        {
            return info.IsValueType && !info.IsRequiredInQuery ? "?" : String.Empty;
        }
    }
}
