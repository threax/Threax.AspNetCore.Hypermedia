using System;

namespace Threax.ModelGen
{
    public interface ITypeWriter
    {
        string AddUsings(String ns);
        string AddDisplay(string name);
        string AddMaxLength(int length, string errorMessage);
        string AddRequired(string errorMessage);
        string CreateProperty(string name, IWriterPropertyInfo info);
        string EndType(String name, String pluralName);
        string StartType(String name, String pluralName);
        String AddTypeDisplay(String name);
        string EndNamespace();
        string StartNamespace(string name);
    }

    public abstract class AbstractTypeWriter : ITypeWriter
    {
        public virtual string AddDisplay(string name)
        {
            return "";
        }

        public virtual string AddMaxLength(int length, string errorMessage)
        {
            return "";
        }

        public virtual string AddRequired(string errorMessage)
        {
            return "";
        }

        public virtual string AddTypeDisplay(string name)
        {
            return "";
        }

        public virtual string AddUsings(string ns)
        {
            return "";
        }

        public virtual string CreateProperty(string name, IWriterPropertyInfo info)
        {
            return "";
        }

        public virtual string EndNamespace()
        {
            return "";
        }

        public virtual string EndType(string name, string pluralName)
        {
            return "";
        }

        public virtual string StartNamespace(string name)
        {
            return "";
        }

        public virtual string StartType(string name, string pluralName)
        {
            return "";
        }
    }
}