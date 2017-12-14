using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Models
{
    /// <summary>
    /// Use this to change the ui type of a property to a select. Useful if you are going
    /// to provide values for this property with a value provider and want them displayed in
    /// a select drop down.
    /// </summary>
    public class SelectUiTypeAttribute : UiTypeAttribute
    {
        public const String UiName = "select";

        public SelectUiTypeAttribute() : base(UiName)
        {
        }
    }
}
