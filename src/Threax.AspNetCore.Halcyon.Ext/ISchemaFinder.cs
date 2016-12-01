using NJsonSchema;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface ISchemaFinder
    {
        JsonSchema4 Find(string schema);
    }
}