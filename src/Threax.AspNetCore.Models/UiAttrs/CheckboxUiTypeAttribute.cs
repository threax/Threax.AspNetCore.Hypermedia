using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Models
{
    public class CheckboxUiOptions : PropertyUiInfo
    {
        public CheckboxUiOptions(string type) : base(type)
        {
        }

        public bool SelectAll { get; set; }

        public override string CreateAttribute()
        {
            return $@"[CheckboxUiType({AddSharedProperties(false)})]";
        }

        protected override IEnumerable<string> GetSharedProperties()
        {
            return base.GetSharedProperties().Concat(CheckboxSharedProperties());
        }

        private IEnumerable<String> CheckboxSharedProperties()
        {
            if (SelectAll)
            {
                yield return "SelectAll = true";
            }
        }
    }

    /// <summary>
    /// Use this to change the ui type of a property to a checkbox. Useful if you are going
    /// to provide values for this property with a value provider and want them displayed in
    /// a check box.
    /// </summary>
    public class CheckboxUiTypeAttribute : UiTypeAttribute
    {
        public const String UiName = "checkbox";

        public CheckboxUiTypeAttribute()
            : base(new CheckboxUiOptions(UiName))
        {
        }

        /// <summary>
        /// This is here for backward compatibility
        /// </summary>
        /// <param name="selectAll"></param>
        public CheckboxUiTypeAttribute(bool selectAll) 
            : base(new CheckboxUiOptions(UiName)
                       {
                           SelectAll = selectAll
                       })
        {
        }

        public bool SelectAll
        {
            get
            {
                return ((CheckboxUiOptions)this.Value).SelectAll;
            }
            set
            {
                ((CheckboxUiOptions)this.Value).SelectAll = value;
            }
        }
    }
}
