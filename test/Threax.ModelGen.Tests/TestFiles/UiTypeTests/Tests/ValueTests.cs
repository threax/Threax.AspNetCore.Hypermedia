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

        public static ValueInput CreateInput(String seed = "", bool Checkbox = default(bool), DateTime DateOnly = default(DateTime), String Hidden = default(String), String Password = default(String), String Select = default(String), String TextArea = default(String), String CustomType = default(String))
        {
            return new ValueInput()
            {
                Checkbox = Checkbox,
                DateOnly = DateOnly,
                Hidden = Hidden != null ? Hidden : $"Hidden {seed}",
                Password = Password != null ? Password : $"Password {seed}",
                Select = Select != null ? Select : $"Select {seed}",
                TextArea = TextArea != null ? TextArea : $"TextArea {seed}",
                CustomType = CustomType != null ? CustomType : $"CustomType {seed}",
            };
        }


        public static ValueEntity CreateEntity(String seed = "", Guid? ValueId = default(Guid?), bool Checkbox = default(bool), DateTime DateOnly = default(DateTime), String Hidden = default(String), String Password = default(String), String Select = default(String), String TextArea = default(String), String CustomType = default(String))
        {
            return new ValueEntity()
            {
                ValueId = ValueId.HasValue ? ValueId.Value : Guid.NewGuid(),
                Checkbox = Checkbox,
                DateOnly = DateOnly,
                Hidden = Hidden != null ? Hidden : $"Hidden {seed}",
                Password = Password != null ? Password : $"Password {seed}",
                Select = Select != null ? Select : $"Select {seed}",
                TextArea = TextArea != null ? TextArea : $"TextArea {seed}",
                CustomType = CustomType != null ? CustomType : $"CustomType {seed}",
            };
        }


        public static Value CreateView(String seed = "", Guid? ValueId = default(Guid?), bool Checkbox = default(bool), DateTime DateOnly = default(DateTime), String Hidden = default(String), String Password = default(String), String Select = default(String), String TextArea = default(String), String CustomType = default(String))
        {
            return new Value()
            {
                ValueId = ValueId.HasValue ? ValueId.Value : Guid.NewGuid(),
                Checkbox = Checkbox,
                DateOnly = DateOnly,
                Hidden = Hidden != null ? Hidden : $"Hidden {seed}",
                Password = Password != null ? Password : $"Password {seed}",
                Select = Select != null ? Select : $"Select {seed}",
                TextArea = TextArea != null ? TextArea : $"TextArea {seed}",
                CustomType = CustomType != null ? CustomType : $"CustomType {seed}",
            };
        }


        public static void AssertEqual(IValue expected, IValue actual)
        {
           Assert.Equal(expected.Checkbox, actual.Checkbox);
           Assert.Equal(expected.DateOnly, actual.DateOnly);
           Assert.Equal(expected.Hidden, actual.Hidden);
           Assert.Equal(expected.Password, actual.Password);
           Assert.Equal(expected.Select, actual.Select);
           Assert.Equal(expected.TextArea, actual.TextArea);
           Assert.Equal(expected.CustomType, actual.CustomType);
        }

    }
}