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
        public static ValueInput CreateInput(String seed = "", Guid? OptionalId = default(Guid?))
        {
            return new ValueInput()
            {
                OptionalId = OptionalId,
            };
        }


        public static ValueEntity CreateEntity(String seed = "", Guid? ValueId = default(Guid?), Guid? OptionalId = default(Guid?))
        {
            return new ValueEntity()
            {
                ValueId = ValueId.HasValue ? ValueId.Value : Guid.NewGuid(),
                OptionalId = OptionalId,
            };
        }


        public static Value CreateView(String seed = "", Guid? ValueId = default(Guid?), Guid? OptionalId = default(Guid?))
        {
            return new Value()
            {
                ValueId = ValueId.HasValue ? ValueId.Value : Guid.NewGuid(),
                OptionalId = OptionalId,
            };
        }


        public static void AssertEqual(IValue expected, IValue actual)
        {
           Assert.Equal(expected.OptionalId, actual.OptionalId);
        }

    }
}