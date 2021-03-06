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
    public static partial class LeftTests
    {
        private static Mockup SetupModel(this Mockup mockup)
        {
            mockup.Add<ILeftRepository>(m => new LeftRepository(m.Get<AppDbContext>(), m.Get<AppMapper>()));

            return mockup;
        }

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

        public static void AssertEqual(LeftInput expected, LeftEntity actual)
        {
           Assert.Equal(expected.Info, actual.Info);
        }

        public static void AssertEqual(LeftEntity expected, Left actual)
        {
           Assert.Equal(expected.Info, actual.Info);
        }

    }
}