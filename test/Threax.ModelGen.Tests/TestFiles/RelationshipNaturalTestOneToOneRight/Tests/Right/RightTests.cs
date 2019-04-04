using AutoMapper;
using Test.Database;
using Test.InputModels;
using Test.Repository;
using Test.ViewModels;
using Test.Mappers;
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
            mockup.Add<IRightRepository>(m => new RightRepository(m.Get<AppDbContext>(), m.Get<AppMapper>()));

            return mockup;
        }
    }
}