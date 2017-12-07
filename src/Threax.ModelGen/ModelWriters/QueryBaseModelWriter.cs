using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    class QueryBaseModelWriter : BaseModelWriter
    {
        public QueryBaseModelWriter(string classSuffix, IAttributeBuilder propAttributeBuilder, IAttributeBuilder classAttrBuilder = null) : base(classSuffix, propAttributeBuilder, classAttrBuilder)
        {
        }

        public override void CreateProperty(StringBuilder sb, string name, IWriterPropertyInfo info)
        {
            base.CreateProperty(sb, name, new MakeQueryPropertiesNullable(info));
        }

        class MakeQueryPropertiesNullable : IWriterPropertyInfo
        {
            private IWriterPropertyInfo wrapped;

            public MakeQueryPropertiesNullable(IWriterPropertyInfo wrapped)
            {
                this.wrapped = wrapped;
            }

            public bool IsValueType => wrapped.IsValueType;

            public string ClrType
            {
                get
                {
                    if (wrapped.IsValueType && !wrapped.IsRequiredInQuery)
                    {
                        return wrapped.ClrType + "?";
                    }
                    return wrapped.ClrType;
                }
            }

            public bool IsRequiredInQuery => wrapped.IsRequiredInQuery;

            public bool ShowOnQueryUi => wrapped.ShowOnQueryUi;

            public string DisplayName => wrapped.DisplayName;

            public bool HasRequiredAttribute => wrapped.HasRequiredAttribute;

            public int? MaxLength => wrapped.MaxLength;

            public int? Order => wrapped.Order;

            public bool OnAllModelTypes => wrapped.OnAllModelTypes;

            public string NullValueLabel => wrapped.NullValueLabel;
        }
    }
}
