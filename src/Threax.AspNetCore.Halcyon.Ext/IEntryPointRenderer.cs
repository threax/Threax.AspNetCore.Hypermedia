using Newtonsoft.Json.Linq;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface IEntryPointRenderer
    {
        JObject Render();
    }

    public interface IEntryPointRenderer<T> : IEntryPointRenderer
    {

    }
}