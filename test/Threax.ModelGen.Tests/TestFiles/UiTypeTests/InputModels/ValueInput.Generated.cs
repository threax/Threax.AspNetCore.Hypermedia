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
        [CheckboxUiType]
        public bool Checkbox { get; set; }

        [DateUiType]
        public DateTime DateOnly { get; set; }

        [HiddenUiType]
        public String Hidden { get; set; }

        [PasswordUiType]
        public String Password { get; set; }

        [SelectUiType]
        public String Select { get; set; }

        [TextAreaUiType]
        public String TextArea { get; set; }

        [UiType("custom")]
        public String CustomType { get; set; }

    }
}
