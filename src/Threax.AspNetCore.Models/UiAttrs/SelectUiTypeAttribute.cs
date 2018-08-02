using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Models
{
    public class SelectUiOptions : PropertyUiInfo
    {
        public SelectUiOptions(string type) : base(type)
        {
        }

        public override string CreateAttribute()
        {
            return $@"[SelectUiType({AddSharedProperties(false)})]";
        }
    }

    /// <summary>
    /// Use this to change the ui type of a property to a select. Useful if you are going
    /// to provide values for this property with a value provider and want them displayed in
    /// a select drop down.
    /// </summary>
    public class SelectUiTypeAttribute : UiTypeAttribute
    {
        public const String UiName = "select";

        public SelectUiTypeAttribute() : base(new SelectUiOptions(UiName))
        {
        }
    }
}
