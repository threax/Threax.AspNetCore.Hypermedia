﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext.UIAttrs
{
    /// <summary>
    /// Use this to change the ui type of a property to a password. This will
    /// cause any uis to put up password fields instead of normal input.
    /// </summary>
    public class PasswordUiTypeAttribute : UiTypeAttribute
    {
        public PasswordUiTypeAttribute() : base("password")
        {
        }
    }
}
