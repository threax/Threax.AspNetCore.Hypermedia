using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen.TestGenerators
{
    class ModelEqualityAssert : ITypeWriter
    {
        private Func<IWriterPropertyInfo, bool> compareProperties;
        private String expectedSuffix;
        private String actualSuffix;
        private String className;

        public ModelEqualityAssert(Func<IWriterPropertyInfo, bool> compareProperties, String expectedSuffix, String actualSuffix)
        {
            this.compareProperties = compareProperties;
            this.expectedSuffix = expectedSuffix;
            this.actualSuffix = actualSuffix;
        }

        public void StartType(StringBuilder sb, string name, string pluralName)
        {
            this.className = name;
            sb.AppendLine( 
$@"        public static void AssertEqual({name}{expectedSuffix} expected, {name}{actualSuffix} actual)
        {{"
            );
        }

        public void EndType(StringBuilder sb, string name, string pluralName)
        {
            sb.AppendLine("        }");
        }

        public void CreateProperty(StringBuilder sb, string name, IWriterPropertyInfo info)
        {
            if (this.compareProperties(info))
            {
                sb.AppendLine($"           Assert.Equal(expected.{name}, actual.{name});");
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
