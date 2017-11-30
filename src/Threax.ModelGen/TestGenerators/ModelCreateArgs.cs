using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.TestGenerators
{
    class ModelCreateArgs : ITypeWriter
    {
        public string StartType(string name, string pluralName)
        {
            return "";
        }

        public string EndType(string name, string pluralName)
        {
            return "";
        }

        public string CreateProperty(string type, string name, bool isValueType)
        {
            return $"                                         , {type} {name} = default({type})";
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
