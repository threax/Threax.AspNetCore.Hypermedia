using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Ext.ValueProviders
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Property)]
    public class NullEnumLabelAttribute : Attribute
    {
        public NullEnumLabelAttribute()
        {
            this.Label = "None";
        }

        public NullEnumLabelAttribute(String label)
        {
            this.Label = label;
        }

        public String Label { get; private set; }
    }
}
