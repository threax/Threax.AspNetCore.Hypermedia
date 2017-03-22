using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext.ValueProviders
{
    public class LabelValuePair
    {
        public LabelValuePair()
        {

        }

        public LabelValuePair(String label, String value)
        {
            this.Label = label;
            this.Value = value;
        }

        public String Label { get; set; }

        public String Value { get; set; }
    }
}
