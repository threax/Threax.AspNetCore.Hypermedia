using System;
using System.Text;

namespace Threax.ModelGen
{
    public interface ITypeWriter
    {
        void AddUsings(StringBuilder sb, String ns);
        void CreateProperty(StringBuilder sb, string name, IWriterPropertyInfo info);
        void EndType(StringBuilder sb, String name, String pluralName);
        void StartType(StringBuilder sb, String name, String pluralName);
        void EndNamespace(StringBuilder sb);
        void StartNamespace(StringBuilder sb, string name);
    }

    public abstract class AbstractTypeWriter : ITypeWriter
    {
        public virtual void AddUsings(StringBuilder sb, string ns)
        {
            
        }

        public virtual void CreateProperty(StringBuilder sb, string name, IWriterPropertyInfo info)
        {
            
        }

        public virtual void EndNamespace(StringBuilder sb)
        {
            
        }

        public virtual void EndType(StringBuilder sb, string name, string pluralName)
        {
            
        }

        public virtual void StartNamespace(StringBuilder sb, string name)
        {
            
        }

        public virtual void StartType(StringBuilder sb, string name, string pluralName)
        {
            
        }
    }
}