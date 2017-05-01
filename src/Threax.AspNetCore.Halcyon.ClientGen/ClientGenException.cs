using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Halcyon.ClientGen
{
    public class ClientGenException : Exception
    {
        public ClientGenException(String message)
            :base(message)
        {

        }
    }
}
