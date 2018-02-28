using AutoMapper;
using Test.Database;
using Test.InputModels;
using Test.Repository;
using Test.Models;
using Test.ViewModels;
using System;
using Threax.AspNetCore.Tests;
using Xunit;
using System.Collections.Generic;

namespace Test.Tests
{
    public static partial class RightTests
    {
        private static Mockup SetupModel(this Mockup mockup)
        {
            mockup.Add<IRightRepository>(m => new RightRepository(m.Get<AppDbContext>(), m.Get<IMapper>()));

            return mockup;
        }
    }
}