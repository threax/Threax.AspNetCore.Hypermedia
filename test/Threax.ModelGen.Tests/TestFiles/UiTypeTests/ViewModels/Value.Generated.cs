using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.Tracking;
using Test.Models;
using Test.Controllers.Api;
using Threax.AspNetCore.Halcyon.Ext.ValueProviders;
using Threax.AspNetCore.Models;

namespace Test.ViewModels 
{
       public partial class Value : IValue, IValueId, ICreatedModified
       {
        public Guid ValueId { get; set; }

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

        [UiOrder(0, 2147483646)]
        public DateTime Created { get; set; }

        [UiOrder(0, 2147483647)]
        public DateTime Modified { get; set; }

    }
}
