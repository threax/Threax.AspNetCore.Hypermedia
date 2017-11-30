using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    class QueryArgsWriter : ITypeWriter
    {
        protected String args;

        public QueryArgsWriter(String args)
        {
            this.args = args;
        }

        public virtual string StartType(string name, string pluralName)
        {
            return "";
        }

        public string EndType(string name, string pluralName)
        {
            return "";
        }

        public virtual String CreateProperty(String type, String name, bool isValueType)
        {
            return $"        public {type} {name} {{ get; set; }}";
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
