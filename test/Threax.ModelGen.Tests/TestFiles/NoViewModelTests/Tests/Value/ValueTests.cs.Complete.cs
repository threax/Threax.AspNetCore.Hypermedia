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
    public static partial class ValueTests
    {
        private static Mockup SetupModel(this Mockup mockup)
        {
            mockup.Add<IValueRepository>(m => new ValueRepository(m.Get<AppDbContext>(), m.Get<AppMapper>()));

            return mockup;
        }

        public static ValueInput CreateInput(String seed = "", String Info = default(String))
        {
            return new ValueInput()
            {
                Info = Info != null ? Info : $"Info {seed}",
            };
        }

        public static ValueEntity CreateEntity(String seed = "", Guid? ValueId = default(Guid?), String Info = default(String))
        {
            return new ValueEntity()
            {
                ValueId = ValueId.HasValue ? ValueId.Value : Guid.NewGuid(),
                Info = Info != null ? Info : $"Info {seed}",
            };
        }

        public static Value CreateView(String seed = "", Guid? ValueId = default(Guid?))
        {
            return new Value()
            {
                ValueId = ValueId.HasValue ? ValueId.Value : Guid.NewGuid(),
            };
        }

        public static void AssertEqual(ValueInput expected, ValueEntity actual)
        {
           Assert.Equal(expected.Info, actual.Info);
        }

        public static void AssertEqual(ValueEntity expected, Value actual)
        {
        }

    }
}