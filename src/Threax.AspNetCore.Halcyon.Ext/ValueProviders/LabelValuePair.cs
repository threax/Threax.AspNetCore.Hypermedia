using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext.ValueProviders
{
    public class LabelValuePair<T>
    {
        public LabelValuePair()
        {

        }

        public LabelValuePair(String label, T value)
        {
            this.Label = label;
            this.Value = value;
        }

        public String Label { get; set; }

        public T Value { get; set; }
    }

    public class LabelValuePair : LabelValuePair<String>
    {
        public LabelValuePair()
        {

        }

        public LabelValuePair(String label, String value)
            :base(label, value)
        {
            
        }
    }
}
