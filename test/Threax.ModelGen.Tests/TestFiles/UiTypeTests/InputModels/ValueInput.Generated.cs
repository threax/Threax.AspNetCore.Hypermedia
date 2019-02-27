using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;
using Test.Models;
using Threax.AspNetCore.Halcyon.Ext.ValueProviders;

namespace Test.InputModels 
{
    [HalModel]
    public partial class ValueInput : IValue
    {
        [CheckboxUiType()]
        public bool Checkbox { get; set; }

        [CheckboxUiType(SelectAll = true)]
        public bool CheckboxSelectAll { get; set; }

        [DateUiType()]
        public DateTime DateOnly { get; set; }

        [HiddenUiType()]
        public String Hidden { get; set; }

        [PasswordUiType()]
        public String Password { get; set; }

        [SelectUiType()]
        public String Select { get; set; }

        [TextAreaUiType()]
        public String TextArea { get; set; }

        [TextUiType()]
        public String Text { get; set; }

        [TextUiType(Autocomplete = true)]
        public String TextAutocomplete { get; set; }

        [DateTimeUiType(dataTimezone: DateTimeUiTypeAttribute.Timezones.UTC, displayTimezone: DateTimeUiTypeAttribute.Timezones.America_New_York)]
        public DateTime DateTimeWithTimezones { get; set; }

        [DateTimeUiType(dataTimezone: DateTimeUiTypeAttribute.Timezones.UTC)]
        public DateTime DateTimeWithDataTimezone { get; set; }

        [DateTimeUiType()]
        public DateTime DateTimeWithNoTimezone { get; set; }

        [UiType("custom")]
        public String CustomType { get; set; }

        [CheckboxUiType(OverrideComponent = "CustomComponent")]
        public bool CheckboxOverride { get; set; }

        [DateUiType(OverrideComponent = "CustomComponent")]
        public DateTime DateOnlyOverride { get; set; }

        [HiddenUiType(OverrideComponent = "CustomComponent")]
        public String HiddenOverride { get; set; }

        [PasswordUiType(OverrideComponent = "CustomComponent")]
        public String PasswordOverride { get; set; }

        [SelectUiType(OverrideComponent = "CustomComponent")]
        public String SelectOverride { get; set; }

        [TextAreaUiType(OverrideComponent = "CustomComponent")]
        public String TextAreaOverride { get; set; }

        [TextUiType(OverrideComponent = "CustomComponent")]
        public String TextOverride { get; set; }

        [UiType("custom", OverrideComponent = "CustomComponent")]
        public String CustomTypeOverride { get; set; }

        [CheckboxUiType(OverrideComponent = "CustomComponent", SelectAll = true)]
        public bool CheckboxOverrideSelectAll { get; set; }

        [TextUiType(OverrideComponent = "CustomComponent", Autocomplete = true)]
        public String TextOverrideAutocomplete { get; set; }

    }
}
