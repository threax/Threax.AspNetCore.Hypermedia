using Microsoft.AspNetCore.Mvc;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface IEntryPointRenderer
    {
        void AddEntryPoint(Controller controller);
    }

    public interface IEntryPointRenderer<T> : IEntryPointRenderer
         where T : Controller
    {
    }
}