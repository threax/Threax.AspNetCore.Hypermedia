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

        public override String CreateProperty(String name, IWriterPropertyInfo info)
        {
            return
$@"                if ({name} != null)
                {{
                    query = query.Where(i => i.{name} == {name});
                }}";
        }
    }
}
