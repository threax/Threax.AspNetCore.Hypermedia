using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Models
{
    /// <summary>
    /// Use this to change the ui type of a property to a set of radio buttons. Useful if you are going
    /// to provide values for this property with a value provider and want them displayed
    /// using radio buttons.
    /// </summary>
    public class RadioButtonUiTypeAttribute : UiTypeAttribute
    {
        public const String UiName = "radiobutton";

        public RadioButtonUiTypeAttribute() : base(UiName)
        {
        }
    }
}
