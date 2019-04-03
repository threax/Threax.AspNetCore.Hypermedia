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
    public static partial class RightTests
    {
        public static RightInput CreateInput(String seed = "", String Info = default(String))
        {
            return new RightInput()
            {
                Info = Info != null ? Info : $"Info {seed}",
            };
        }


        public static RightEntity CreateEntity(String seed = "", Guid? RightId = default(Guid?), String Info = default(String))
        {
            return new RightEntity()
            {
                RightId = RightId.HasValue ? RightId.Value : Guid.NewGuid(),
                Info = Info != null ? Info : $"Info {seed}",
            };
        }


        public static Right CreateView(String seed = "", Guid? RightId = default(Guid?), String Info = default(String))
        {
            return new Right()
            {
                RightId = RightId.HasValue ? RightId.Value : Guid.NewGuid(),
                Info = Info != null ? Info : $"Info {seed}",
            };
        }


        public static void AssertEqual(RightInput expected, RightEntity actual)
        {
           Assert.Equal(expected.Info, actual.Info);
        }


        public static void AssertEqual(RightEntity expected, Right actual)
        {
           Assert.Equal(expected.Info, actual.Info);
        }

    }
}