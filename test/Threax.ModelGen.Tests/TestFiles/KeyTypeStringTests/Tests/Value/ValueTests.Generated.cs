using AutoMapper;
using Test.Database;
using Test.InputModels;
using Test.Repository;
using Test.ViewModels;
using System;
using Threax.AspNetCore.Tests;
using Xunit;
using System.Collections.Generic;

namespace Test.Tests
{
    public static partial class ValueTests
    {
        public static ValueInput CreateInput(String seed = "")
        {
            return new ValueInput()
            {
            };
        }

        public static ValueEntity CreateEntity(String seed = "", String ValueId = default(String))
        {
            return new ValueEntity()
            {
                ValueId = ValueId != null ? ValueId : seed + Guid.NewGuid().ToString(),
            };
        }

        public static Value CreateView(String seed = "", String ValueId = default(String))
        {
            return new Value()
            {
                ValueId = ValueId != null ? ValueId : seed + Guid.NewGuid().ToString(),
            };
        }

        public static void AssertEqual(ValueInput expected, ValueEntity actual)
        {
        }

        public static void AssertEqual(ValueEntity expected, Value actual)
        {
        }

    }
}