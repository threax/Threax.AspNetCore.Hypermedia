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
    public static partial class ValueTests
    {
        private static Mockup SetupModel(this Mockup mockup)
        {
            mockup.Add<IValueRepository>(m => new ValueRepository(m.Get<AppDbContext>(), m.Get<IMapper>()));

            return mockup;
        }

        public static ValueInput CreateInput(String seed = "")
        {
            return new ValueInput()
            {
            };
        }


        public static ValueEntity CreateEntity(String seed = "", Guid? CrazyKey = default(Guid?))
        {
            return new ValueEntity()
            {
                CrazyKey = CrazyKey.HasValue ? CrazyKey.Value : Guid.NewGuid(),
            };
        }


        public static Value CreateView(String seed = "", Guid? CrazyKey = default(Guid?))
        {
            return new Value()
            {
                CrazyKey = CrazyKey.HasValue ? CrazyKey.Value : Guid.NewGuid(),
            };
        }


        public static void AssertEqual(IValue expected, IValue actual)
        {
        }

    }
}