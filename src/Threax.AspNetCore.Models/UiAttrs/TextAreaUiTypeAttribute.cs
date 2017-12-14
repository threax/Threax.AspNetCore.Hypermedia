using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Models
{
    /// <summary>
    /// Use this to change the ui type of a property to a textarea.
    /// </summary>
    public class TextAreaUiTypeAttribute : UiTypeAttribute
    {
        public const String UiName = "textarea";

        public TextAreaUiTypeAttribute() : base(UiName)
        {
        }
    }
}
