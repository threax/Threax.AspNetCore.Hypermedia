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

        public static ValueInput CreateInput(String seed = "", String Things = default(String))
        {
            return new ValueInput()
            {
                Things = Things != null ? Things : $"Things {seed}",
            };
        }


        public static ValueEntity CreateEntity(String seed = "", Guid? ValueId = default(Guid?), String Things = default(String))
        {
            return new ValueEntity()
            {
                ValueId = ValueId.HasValue ? ValueId.Value : Guid.NewGuid(),
                Things = Things != null ? Things : $"Things {seed}",
            };
        }


        public static Value CreateView(String seed = "", Guid? ValueId = default(Guid?), String Things = default(String))
        {
            return new Value()
            {
                ValueId = ValueId.HasValue ? ValueId.Value : Guid.NewGuid(),
                Things = Things != null ? Things : $"Things {seed}",
            };
        }


        public static void AssertEqual(IValue expected, IValue actual)
        {
           Assert.Equal(expected.Things, actual.Things);
        }

    }
}