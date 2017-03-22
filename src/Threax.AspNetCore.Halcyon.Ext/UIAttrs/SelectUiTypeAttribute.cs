using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext.UIAttrs
{
    public class SelectUiTypeAttribute : UiTypeAttribute
    {
        public SelectUiTypeAttribute() : base("select")
        {
        }
    }
}
