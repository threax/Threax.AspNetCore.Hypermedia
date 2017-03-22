using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext.ValueProviders
{
    public class ValueProviderException : Exception
    {
        public ValueProviderException(String message)
            :base(message)
        {
            
        }
    }
}
