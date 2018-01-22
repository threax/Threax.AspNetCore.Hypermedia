using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    public class RunOnFullFrameworkException : Exception
    {
        public RunOnFullFrameworkException(Exception innerException)
            :base("", innerException)
        {

        }
    }
}
