using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Models
{
    public class DateUiOptions : PropertyUiInfo
    {
        public DateUiOptions(string type) : base(type)
        {
        }

        public override string CreateAttribute()
        {
            return $@"[DateUiType({AddSharedProperties(false)})]";
        }
    }

    /// <summary>
    /// Use this to change the ui type of a property to a date. This will
    /// cause uis to allow just date input instead of date and time.
    /// </summary>
    public class DateUiTypeAttribute : UiTypeAttribute
    {
        public const String UiName = "date";

        public DateUiTypeAttribute() : base(new DateUiOptions(UiName))
        {
        }
    }
}
