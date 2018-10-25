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

        /// <summary>
        /// The label to use for null values.
        /// </summary>
        public String Label { get; private set; }

        /// <summary>
        /// Set this to true (default) to include the null value label when appropriate.
        /// Otherwise set this to false and no null value label will be included.
        /// </summary>
        public bool IncludeNullValueLabel { get; set; } = true;
    }
}
