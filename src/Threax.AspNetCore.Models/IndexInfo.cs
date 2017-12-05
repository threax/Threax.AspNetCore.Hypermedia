using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Threax.AspNetCore.Models
{
    public class IndexInfo
    {
        public Type Type { get; internal set; }

        public PropertyInfo PropertyInfo { get; internal set; }

        public IndexPropAttribute IndexAttribute { get; internal set; }
    }
}
