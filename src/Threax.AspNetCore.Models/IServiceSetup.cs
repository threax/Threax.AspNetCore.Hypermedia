using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Models
{
    public interface IServiceSetup
    {
        void ConfigureServices(IServiceCollection services);
    }
}
