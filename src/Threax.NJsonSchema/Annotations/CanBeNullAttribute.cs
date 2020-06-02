﻿//-----------------------------------------------------------------------
// <copyright file="CanBeNullAttribute.cs" company="NJsonSchema">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/rsuter/NJsonSchema/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System;

namespace Threax.NJsonSchema.Annotations
{
    /// <summary>Indicates that the value of the marked element is nullable.</summary>
    [AttributeUsage(
        AttributeTargets.Method |
        AttributeTargets.Parameter |
        AttributeTargets.Property |
        AttributeTargets.ReturnValue |
        AttributeTargets.Delegate |
        AttributeTargets.Field |
        AttributeTargets.Event)]
    public class CanBeNullAttribute : Attribute
    {
    }
}