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
using An.Extra.Namespace;
using Can.Be.Multiline;

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

        public static ValueEntity CreateEntity(String seed = "", Guid? ValueId = default(Guid?))
        {
            return new ValueEntity()
            {
                ValueId = ValueId.HasValue ? ValueId.Value : Guid.NewGuid(),
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
        }

        public static void AssertEqual(ValueEntity expected, Value actual)
        {
        }

    }
}