using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;

namespace Test.Models 
{
    public partial interface IValue 
    {
        bool Checkbox { get; set; }

        bool CheckboxSelectAll { get; set; }

        DateTime DateOnly { get; set; }

        String Hidden { get; set; }

        String Password { get; set; }

        String Select { get; set; }

        String TextArea { get; set; }

        String Text { get; set; }

        String TextAutocomplete { get; set; }

        DateTime DateTimeWithTimezones { get; set; }

        DateTime DateTimeWithDataTimezone { get; set; }

        DateTime DateTimeWithNoTimezone { get; set; }

        String CustomType { get; set; }

        bool CheckboxOverride { get; set; }

        DateTime DateOnlyOverride { get; set; }

        String HiddenOverride { get; set; }

        String PasswordOverride { get; set; }

        String SelectOverride { get; set; }

        String TextAreaOverride { get; set; }

        String TextOverride { get; set; }

        String CustomTypeOverride { get; set; }

        bool CheckboxOverrideSelectAll { get; set; }

        String TextOverrideAutocomplete { get; set; }

    }

    public partial interface IValueId
    {
        Guid ValueId { get; set; }
    }    

    public partial interface IValueQuery
    {
        Guid? ValueId { get; set; }

    }
}