using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Models
{
    public class TextUiOptions : PropertyUiInfo
    {
        public TextUiOptions(string type) : base(type)
        {
        }

        public bool Autocomplete { get; set; }

        public override string CreateAttribute()
        {
            return $@"[TextUiType({AddSharedProperties(false)})]";
        }

        protected override IEnumerable<string> GetSharedProperties()
        {
            return base.GetSharedProperties().Concat(TextSharedProps());
        }

        private IEnumerable<String> TextSharedProps()
        {
            if (Autocomplete)
            {
                yield return "Autocomplete = true";
            }
        }
    }

    /// <summary>
    /// Use this to change the ui type of a property to text. This will
    /// cause any uis to put up a normal input box. You can optionally
    /// specify autocomplete if you have values to provide for auto completion.
    /// </summary>
    public class TextUiTypeAttribute : UiTypeAttribute
    {
        public const String UiName = "text";

        public TextUiTypeAttribute() : base(new TextUiOptions(UiName))
        {
        }

        public bool Autocomplete
        {
            get
            {
                return ((TextUiOptions)this.Value).Autocomplete;
            }
            set
            {
                ((TextUiOptions)this.Value).Autocomplete = value;
            }
        }
    }
}
