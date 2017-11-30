using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    class QueryCreateWriter : ITypeWriter
    {
        public QueryCreateWriter()
        {
            
        }

        public virtual string StartType(string name, string pluralName)
        {
            return "";
        }

        public string EndType(string name, string pluralName)
        {
            return "";
        }

        public virtual String CreateProperty(String name, IWriterPropertyInfo info)
        {
            return
$@"                if ({name} != null)
                {{
                    query = query.Where(i => i.{name} == {name});
                }}";
        }

        public string AddDisplay(string name)
        {
            return "";
        }

        public string AddMaxLength(int length, string errorMessage)
        {
            return "";
        }

        public string AddRequired(string errorMessage)
        {
            return "";
        }

        public string AddTypeDisplay(string name)
        {
            return "";
        }

        public string AddUsings(string ns)
        {
            return "";
        }

        public string EndNamespace()
        {
            return "";
        }

        public string StartNamespace(string name)
        {
            return "";
        }
    }
}
