using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    class QueryPropertiesWriter : AbstractTypeWriter
    {
        public QueryPropertiesWriter()
        {
            
        }

        public override String CreateProperty(String name, IWriterPropertyInfo info)
        {
            var question = info.IsValueType ? "?" : String.Empty;
            return $"        public {info.ClrType}{question} {name} {{ get; set; }}";
        }
    }
}
