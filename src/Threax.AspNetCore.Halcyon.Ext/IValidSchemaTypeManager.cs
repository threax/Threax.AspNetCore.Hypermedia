using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface IValidSchemaTypeManager
    {
        String ErrorMessage { get; }

        bool IsValid(Type type);
    }
}
