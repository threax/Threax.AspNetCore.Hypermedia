using System.Text;

namespace Threax.ModelGen.ModelWriters
{
    public interface IAttributeBuilder
    {
        void BuildAttributes(StringBuilder sb, string name, IWriterPropertyInfo prop, string spaces = "        ");
    }
}