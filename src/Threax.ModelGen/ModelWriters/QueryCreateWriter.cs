using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    class QueryCreateWriter : AbstractTypeWriter
    {
        public QueryCreateWriter()
        {
            
        }

        public override void CreateProperty(StringBuilder sb, String name, IWriterPropertyInfo info)
        {
            sb.AppendLine();
            if (info.IsRequiredInQuery)
            {
                sb.AppendLine($"                query = query.Where(i => i.{name} == {name});");
            }
            else
            {
                sb.AppendLine(
$@"                if ({name} != null)
                {{
                    query = query.Where(i => i.{name} == {name});
                }}"
                );
            }
        }
    }
}
