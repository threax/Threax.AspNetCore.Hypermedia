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

        bool ShowOnQueryUi { get; }

        String DisplayName { get; }

        bool HasRequiredAttribute { get; }

        int? MaxLength { get; }

        bool IsVirtual { get; }

        int? Order { get; }

        bool OnAllModelTypes { get; }

        String NullValueLabel { get; }
    }

    public class TypeWriterPropertyInfo : IWriterPropertyInfo
    {
        private Type type;

        public TypeWriterPropertyInfo(Type t)
        {
            this.type = t;
        }

        public bool IsValueType => type.IsValueType;

        public string ClrType => type.Name;

        public bool IsRequiredInQuery => false;

        public bool ShowOnQueryUi => false;

        public String DisplayName => null;

        public bool HasRequiredAttribute => false;

        public int? MaxLength => null;

        public bool IsVirtual => false;

        public int? Order { get; set; } = null;

        public bool OnAllModelTypes => true;

        public String NullValueLabel => null;
    }

    public class TypeWriterPropertyInfo<T> : TypeWriterPropertyInfo
    {
        public TypeWriterPropertyInfo() : base(typeof(T))
        {
        }
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

        public bool IsRequiredInQuery => prop.IsQueryableRequired();

        public bool ShowOnQueryUi => prop.IsQueryableShowOnUi();

        public String DisplayName => prop.Title;

        public bool HasRequiredAttribute => prop.IsRequired;

        public int? MaxLength => prop.MaxLength;

        public bool IsVirtual => prop.IsVirtual();

        public int? Order => prop.GetOrder();

        public bool OnAllModelTypes => prop.OnAllModelTypes();

        public String NullValueLabel => prop.GetNullValueLabel();
    }

    public class NoWriterInfo : IWriterPropertyInfo
    {
        public NoWriterInfo()
        {

        }

        public bool IsValueType => false;

        public string ClrType => null;

        public bool IsRequiredInQuery => false;

        public bool ShowOnQueryUi => false;

        public String DisplayName => null;

        public bool HasRequiredAttribute => false;

        public int? MaxLength => null;

        public bool IsVirtual => false;

        public int? Order => null;

        public bool OnAllModelTypes => true;

        public String NullValueLabel => null;
    }
}
