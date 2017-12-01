using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.TestGenerators
{
    class ModelCreateArgs : AbstractTypeWriter
    {
        public override void CreateProperty(StringBuilder sb, string name, IWriterPropertyInfo info)
        {
            sb.Append($", {info.ClrType} {name} = default({info.ClrType})");
        }
    }
}
