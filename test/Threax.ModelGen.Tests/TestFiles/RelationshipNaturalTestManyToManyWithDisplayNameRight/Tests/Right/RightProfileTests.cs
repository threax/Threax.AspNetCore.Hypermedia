using AutoMapper;
using Test.Database;
using Test.ViewModels;
using Test.Mappers;
using Test.Models;
using System;
using Threax.AspNetCore.Tests;
using Xunit;

namespace Test.Tests
{
    public static partial class RightTests
    {
        public class Profile : IDisposable
        {
            private Mockup mockup = new Mockup().SetupGlobal().SetupModel();

            public Profile()
            {

            }

            public void Dispose()
            {
                mockup.Dispose();
            }

            [Fact]
            void InputToEntity()
            {
                var mapper = mockup.Get<AppMapper>();
                var input = RightTests.CreateInput();
                var entity = mapper.MapRight(input, new RightEntity());

                //Make sure the id does not copy over
                Assert.Equal(default(Guid), entity.RightId);
                AssertEqual(input, entity);
            }

            [Fact]
            void EntityToView()
            {
                var mapper = mockup.Get<AppMapper>();
                var entity = RightTests.CreateEntity();
                var view = mapper.MapRight(entity, new Right());

                Assert.Equal(entity.RightId, view.RightId);
                AssertEqual(entity, view);
            }
        }
    }
}