using Halcyon.HAL.Attributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class ValidSchemaTypeManager : IValidSchemaTypeManager
    {
        public bool IsValid(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return typeInfo.GetCustomAttribute<HalModelAttribute>() != null;
        }

        public string ErrorMessage => "Declare a HalModel attribute on it to mark it valid.";
    }
}
