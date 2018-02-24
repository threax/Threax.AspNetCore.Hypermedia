using AutoMapper;
using Test.Database;
using Test.ViewModels;
using Test.Models;
using System;
using Threax.AspNetCore.Tests;
using Xunit;

namespace Test.Tests
{
    public static partial class LeftTests
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
                var mapper = mockup.Get<IMapper>();
                var input = LeftTests.CreateInput();
                var entity = mapper.Map<LeftEntity>(input);

                //Make sure the id does not copy over
                Assert.Equal(default(Guid), entity.LeftId);
                AssertEqual(input, entity);
            }

            [Fact]
            void EntityToView()
            {
                var mapper = mockup.Get<IMapper>();
                var entity = LeftTests.CreateEntity();
                var view = mapper.Map<Left>(entity);

                Assert.Equal(entity.LeftId, view.LeftId);
                AssertEqual(entity, view);
            }
        }
    }
}