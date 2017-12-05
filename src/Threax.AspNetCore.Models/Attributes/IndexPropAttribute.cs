using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class IndexPropAttribute : Attribute
    {
        public IndexPropAttribute()
        {
        }

        public bool IsClustered { get; set; } = true;

        public bool IsUnique { get; set; } = true;
    }
}
