using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Threax.AspNetCore.Tests;
using Moq;

namespace Threax.AspNetCore.Models.Tests
{
    public class ReflectedServiceSetupTests
    {
        private static bool CalledConfigureServices = false; //Since this is static, make this the only test

        class ServiceSetup : IServiceSetup
        {
            public void ConfigureServices(IServiceCollection services)
            {
                ReflectedServiceSetupTests.CalledConfigureServices = true;
            }
        }

        private Mockup mockup = new Mockup();

        public ReflectedServiceSetupTests()
        {
            mockup.Add<IServiceCollection>(s => new Mock<IServiceCollection>().Object);
        }

        [Fact]
        public void Test()
        {
            ReflectedServiceSetup.ConfigureReflectedServices(mockup.Get<IServiceCollection>(), this.GetType().Assembly);
            Assert.True(ReflectedServiceSetupTests.CalledConfigureServices, $"Did not call configure services, could not find {nameof(ServiceSetup)}");
        }
    }
}
