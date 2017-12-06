using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.TestGenerators
{
    class ModelEqualityAssert : ITypeWriter
    {
        private String className;

        public void StartType(StringBuilder sb, string name, string pluralName)
        {
            this.className = name;
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
            if (info.OnAllModelTypes)
            {
                sb.AppendLine($"           Assert.Equal(expected.{name}, actual.{name});");
            }
            else
            {
                var propInterface = IdInterfaceWriter.GetPropertyInterfaceName(className, name);
                sb.AppendLine($"           if(expected is {propInterface} && actual is {propInterface})");
                sb.AppendLine($"           {{");
                sb.AppendLine($"               Assert.Equal((expected as {propInterface}).{name}, (actual as {propInterface}).{name});");
                sb.AppendLine($"           }}");
            }
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
