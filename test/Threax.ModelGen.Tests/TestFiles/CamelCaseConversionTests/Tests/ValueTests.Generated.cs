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
        public static ValueInput CreateInput(String seed = "", int NormalName = default(int), int CAPSName = default(int), int ALLCAPS = default(int))
        {
            return new ValueInput()
            {
                NormalName = NormalName,
                CAPSName = CAPSName,
                ALLCAPS = ALLCAPS,
            };
        }


        public static ValueEntity CreateEntity(String seed = "", Guid? ValueId = default(Guid?), int NormalName = default(int), int CAPSName = default(int), int ALLCAPS = default(int))
        {
            return new ValueEntity()
            {
                ValueId = ValueId.HasValue ? ValueId.Value : Guid.NewGuid(),
                NormalName = NormalName,
                CAPSName = CAPSName,
                ALLCAPS = ALLCAPS,
            };
        }


        public static Value CreateView(String seed = "", Guid? ValueId = default(Guid?), int NormalName = default(int), int CAPSName = default(int), int ALLCAPS = default(int))
        {
            return new Value()
            {
                ValueId = ValueId.HasValue ? ValueId.Value : Guid.NewGuid(),
                NormalName = NormalName,
                CAPSName = CAPSName,
                ALLCAPS = ALLCAPS,
            };
        }


        public static void AssertEqual(IValue expected, IValue actual)
        {
           Assert.Equal(expected.NormalName, actual.NormalName);
           Assert.Equal(expected.CAPSName, actual.CAPSName);
           Assert.Equal(expected.ALLCAPS, actual.ALLCAPS);
        }

    }
}