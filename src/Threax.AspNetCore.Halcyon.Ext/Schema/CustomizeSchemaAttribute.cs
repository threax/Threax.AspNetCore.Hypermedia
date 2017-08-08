using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Ext
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomizeSchemaAttribute : Attribute
    {
        public CustomizeSchemaAttribute(Type customizer)
        {
            this.CustomizerType = customizer;
        }

        public Type CustomizerType { get; private set; }
    }
}
