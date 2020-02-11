using Microsoft.Extensions.DependencyInjection;

namespace Threax.AspNetCore.Halcyon.Client
{
    public interface IHalcyonClientSetup<T>
    {
        IServiceCollection Services { get; }
    }
}