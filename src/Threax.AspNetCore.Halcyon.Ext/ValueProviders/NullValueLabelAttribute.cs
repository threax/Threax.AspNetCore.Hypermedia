using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Ext.ValueProviders
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Property)]
    public class NullValueLabelAttribute : Attribute
    {
        public NullValueLabelAttribute()
        {
            this.Label = "None";
        }

        public NullValueLabelAttribute(String label)
        {
            this.Label = label;
        }

        public String Label { get; private set; }
    }
}
