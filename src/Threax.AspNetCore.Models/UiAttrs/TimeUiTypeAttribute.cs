using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Models
{
    public class TimeUiOptions : PropertyUiInfo
    {
        public TimeUiOptions(string type) : base(type)
        {
        }

        public override string CreateAttribute()
        {
            return $@"[TimeUiType({AddSharedProperties(false)})]";
        }
    }

    /// <summary>
    /// Use this to change the ui type of a property to a time. This will
    /// cause uis to allow just time input instead of date and time.
    /// </summary>
    public class TimeUiTypeAttribute : UiTypeAttribute
    {
        public const String UiName = "time";

        public TimeUiTypeAttribute() : base(new TimeUiOptions(UiName))
        {
        }
    }
}
