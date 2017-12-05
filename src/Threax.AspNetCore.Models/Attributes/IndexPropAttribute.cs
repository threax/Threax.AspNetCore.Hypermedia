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

        /// <summary>
        /// Set this to true (default is true) to specify that the index should be clustered.
        /// </summary>
        public bool IsClustered { get; set; } = true;

        /// <summary>
        /// Set this to true (default is false) to specify that the index should only contain unique values.
        /// </summary>
        public bool IsUnique { get; set; } = false;
    }
}
