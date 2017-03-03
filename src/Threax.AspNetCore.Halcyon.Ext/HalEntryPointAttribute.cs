using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    /// <summary>
    /// This attribute marks a class as an entry point. This instructs the codegen
    /// to include any special entry point code for this class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class HalEntryPointAttribute : Attribute
    {
    }
}
