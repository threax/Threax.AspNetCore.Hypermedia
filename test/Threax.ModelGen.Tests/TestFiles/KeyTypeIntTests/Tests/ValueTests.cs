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
        private static Mockup SetupModel(this Mockup mockup)
        {
            mockup.Add<IValueRepository>(m => new ValueRepository(m.Get<AppDbContext>(), m.Get<IMapper>()));

            return mockup;
        }

        public static ValueInput CreateInput(String seed = "")
        {
            return new ValueInput()
            {
            };
        }


        private static Int32 currentEntityId = 0;
        private static Object entityIdLock = new Object();
        private static Int32 GetNextEntityId()
        {
            lock(entityIdLock)
            {
                if(currentEntityId == Int32.MaxValue)
                {
                    throw new InvalidOperationException("Ran out of key values for Value entities suggest modifying your tests to create keys manually.");
                }
                return currentEntityId++;
            }
        }

        public static ValueEntity CreateEntity(String seed = "", Int32? ValueId = default(Int32?))
        {
            return new ValueEntity()
            {
                ValueId = ValueId.HasValue ? ValueId.Value : GetNextId(),
            };
        }


        private static Int32 currentViewModelId = 0;
        private static Object viewModelIdLock = new Object();
        private static Int32 GetNextId()
        {
            lock(viewModelIdLock)
            {
                if(currentViewModelId == Int32.MaxValue)
                {
                    throw new InvalidOperationException("Ran out of key values for Value entities suggest modifying your tests to create keys manually.");
                }
                return currentViewModelId++;
            }
        }

        public static Value CreateView(String seed = "", Int32? ValueId = default(Int32?))
        {
            return new Value()
            {
                ValueId = ValueId.HasValue ? ValueId.Value : GetNextId(),
            };
        }


        public static void AssertEqual(IValue expected, IValue actual)
        {
        }

    }
}