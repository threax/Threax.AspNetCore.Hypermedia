using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    public interface IWriterPropertyInfo
    {
        bool IsValueType { get; }

        String ClrType { get; }

        bool IsRequiredInQuery { get; }
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

        public bool IsRequiredInQuery => false;
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

        public bool IsRequiredInQuery => QueryableAttribute.IsRequired(prop);
    }
}
