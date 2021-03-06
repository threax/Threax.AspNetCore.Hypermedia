﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Models
{
    public class PasswordUiOptions : PropertyUiInfo
    {
        public PasswordUiOptions(string type) : base(type)
        {
        }

        public override string CreateAttribute()
        {
            return $@"[PasswordUiType({AddSharedProperties(false)})]";
        }
    }

    /// <summary>
    /// Use this to change the ui type of a property to a password. This will
    /// cause any uis to put up password fields instead of normal input.
    /// </summary>
    public class PasswordUiTypeAttribute : UiTypeAttribute
    {
        public const String UiName = "password";

        public PasswordUiTypeAttribute() : base(new PasswordUiOptions(UiName))
        {
        }
    }
}
