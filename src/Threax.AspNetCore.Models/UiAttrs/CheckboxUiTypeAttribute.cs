using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Models
{
    /// <summary>
    /// Use this to change the ui type of a property to a checkbox. Useful if you are going
    /// to provide values for this property with a value provider and want them displayed in
    /// a check box.
    /// </summary>
    public class CheckboxUiTypeAttribute : UiTypeAttribute
    {
        public const String UiName = "checkbox";

        public CheckboxUiTypeAttribute() : base(UiName)
        {
        }
    }
}
