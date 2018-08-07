using AutoMapper;
using Test.Database;
using Test.InputModels;
using Test.Repository;
using Test.Models;
using Test.ViewModels;
using Test.Mappers;
using System;
using Threax.AspNetCore.Tests;
using Xunit;
using System.Collections.Generic;

namespace Test.Tests
{
    public static partial class LeftTests
    {
        private static Mockup SetupModel(this Mockup mockup)
        {
            mockup.Add<ILeftRepository>(m => new LeftRepository(m.Get<AppDbContext>(), m.Get<AppMapper>()));

            return mockup;
        }
    }
}