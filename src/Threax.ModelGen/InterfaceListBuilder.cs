using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    static class InterfaceListBuilder
    {
        public static String Build(IEnumerable<String> interfaces)
        {
            var sb = new StringBuilder();
            foreach(var face in interfaces)
            {
                if (!String.IsNullOrEmpty(face))
                {
                    if (sb.Length > 0 && sb[sb.Length - 1] != ',')
                    {
                        sb.Append(", ");
                    }
                    sb.Append(face);
                }
            }
            if(sb.Length > 0)
            {
                sb.Insert(0, " : ");
            }
            return sb.ToString();
        }
    }
}
