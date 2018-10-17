using System;
using System.Collections.Generic;
using System.Text;

namespace Halcyon
{
    public interface ICustomVardicPropertyProvider
    {
        void AddCustomVardicProperties(IDictionary<string, object> vardic);
    }
}
