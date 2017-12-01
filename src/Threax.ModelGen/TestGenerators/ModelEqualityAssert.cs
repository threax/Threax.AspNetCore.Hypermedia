using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.TestGenerators
{
    class ModelEqualityAssert : ITypeWriter
    {
        public void StartType(StringBuilder sb, string name, string pluralName)
        {
            sb.AppendLine( 
$@"        public static void AssertEqual(I{name} expected, I{name} actual)
        {{"
            );
        }

        public void EndType(StringBuilder sb, string name, string pluralName)
        {
            sb.AppendLine("        }");
        }

        public void CreateProperty(StringBuilder sb, string name, IWriterPropertyInfo info)
        {
            sb.AppendLine($"           Assert.Equal(expected.{name}, actual.{name});");
        }

        public void AddUsings(StringBuilder sb, string ns)
        {
            
        }

        public void EndNamespace(StringBuilder sb)
        {
            
        }

        public void StartNamespace(StringBuilder sb, string name)
        {
            
        }
    }
}
