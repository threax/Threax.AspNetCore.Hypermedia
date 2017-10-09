using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    class MessageException : Exception
    {
        public MessageException(String message)
            :base(message)
        {
        }
    }
}
