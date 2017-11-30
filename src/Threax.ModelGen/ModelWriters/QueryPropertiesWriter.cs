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
            var question = info.IsValueType ? "?" : String.Empty;
            return $"        {visibility}{info.ClrType}{question} {name} {{ get; set; }}";
        }
    }
}
