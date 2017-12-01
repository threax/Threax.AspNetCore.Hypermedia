using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    static class StringBuilderExtensions
    {
        public static void AppendLineWithContent(this StringBuilder sb, String content)
        {
            if (!String.IsNullOrEmpty(content))
            {
                sb.AppendLine(content);
            }
        }
    }
}
