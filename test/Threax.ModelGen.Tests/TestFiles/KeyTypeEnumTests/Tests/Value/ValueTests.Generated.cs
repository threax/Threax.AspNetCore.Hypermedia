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
        public static ValueInput CreateInput(String seed = "")
        {
            return new ValueInput()
            {
            };
        }

        public static ValueEntity CreateEntity(KeyEnum? ValueId = default(KeyEnum?), String seed = "")
        {
            return new ValueEntity()
            {
                ValueId = ValueId != null ? (KeyEnum)ValueId : default(KeyEnum),
            };
        }

        public static Value CreateView(KeyEnum? ValueId = default(KeyEnum?), String seed = "")
        {
            return new Value()
            {
                ValueId = ValueId != null ? (KeyEnum)ValueId : default(KeyEnum),
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