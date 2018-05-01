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
    public static partial class LeftTests
    {
        public static LeftInput CreateInput(String seed = "", String Info = default(String), List<Guid> RightIds = default(List<Guid>))
        {
            return new LeftInput()
            {
                Info = Info != null ? Info : $"Info {seed}",
                RightIds = RightIds,
            };
        }


        public static LeftEntity CreateEntity(String seed = "", Guid? LeftId = default(Guid?), String Info = default(String))
        {
            return new LeftEntity()
            {
                LeftId = LeftId.HasValue ? LeftId.Value : Guid.NewGuid(),
                Info = Info != null ? Info : $"Info {seed}",
            };
        }


        public static Left CreateView(String seed = "", Guid? LeftId = default(Guid?), String Info = default(String), List<Guid> RightIds = default(List<Guid>))
        {
            return new Left()
            {
                LeftId = LeftId.HasValue ? LeftId.Value : Guid.NewGuid(),
                Info = Info != null ? Info : $"Info {seed}",
                RightIds = RightIds,
            };
        }


        public static void AssertEqual(ILeft expected, ILeft actual)
        {
           Assert.Equal(expected.Info, actual.Info);
           if(expected is ILeft_RightIds && actual is ILeft_RightIds)
           {
               Assert.Equal((expected as ILeft_RightIds).RightIds, (actual as ILeft_RightIds).RightIds);
           }
        }

    }
}