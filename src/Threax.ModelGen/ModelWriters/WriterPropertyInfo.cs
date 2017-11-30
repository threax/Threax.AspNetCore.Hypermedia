using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    public interface IWriterPropertyInfo
    {
        bool IsValueType { get; }

        String ClrType { get; }
    }

    public class TypeWriterPropertyInfo<T> : IWriterPropertyInfo
    {
        private Type type;

        public TypeWriterPropertyInfo()
        {
            this.type = typeof(T);
        }

        public bool IsValueType => type.IsValueType;

        public string ClrType => type.Name;
    }

    public class SchemaWriterPropertyInfo : IWriterPropertyInfo
    {
        private JsonProperty prop;

        public SchemaWriterPropertyInfo(JsonProperty prop)
        {
            this.prop = prop;
        }

        public bool IsValueType => prop.IsClrValueType();

        public string ClrType => prop.GetClrType();
    }
}
