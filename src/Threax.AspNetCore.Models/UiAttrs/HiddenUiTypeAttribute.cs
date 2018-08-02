using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Models
{
    public class HiddenUiOptions : PropertyUiInfo
    {
        public HiddenUiOptions(string type) : base(type)
        {
        }

        public override string CreateAttribute()
        {
            return $@"[HiddenUiType({AddSharedProperties(false)})]";
        }
    }

    /// <summary>
    /// Use this to change the ui type of a property to a hidden. This way you can include
    /// a piece of data in an editing object without displaying it to the user.
    /// </summary>
    public class HiddenUiTypeAttribute : UiTypeAttribute
    {
        public const String UiName = "hidden";

        public HiddenUiTypeAttribute() : base(new HiddenUiOptions(UiName))
        {
        }
    }
}
