using AutoMapper;
using Test.Database;
using Test.InputModels;
using Test.Repository;
using Test.ViewModels;
using System;
using Threax.AspNetCore.Tests;
using Xunit;
using System.Collections.Generic;

namespace Test.Tests
{
    public static partial class ValueTests
    {
        public static ValueInput CreateInput(String seed = "", bool Checkbox = default(bool), bool CheckboxSelectAll = default(bool), DateTime DateOnly = default(DateTime), String Hidden = default(String), String Password = default(String), String Select = default(String), String TextArea = default(String), String Text = default(String), String TextAutocomplete = default(String), DateTime DateTimeWithTimezones = default(DateTime), DateTime DateTimeWithDataTimezone = default(DateTime), DateTime DateTimeWithNoTimezone = default(DateTime), String CustomType = default(String), bool CheckboxOverride = default(bool), DateTime DateOnlyOverride = default(DateTime), String HiddenOverride = default(String), String PasswordOverride = default(String), String SelectOverride = default(String), String TextAreaOverride = default(String), String TextOverride = default(String), String CustomTypeOverride = default(String), bool CheckboxOverrideSelectAll = default(bool), String TextOverrideAutocomplete = default(String))
        {
            return new ValueInput()
            {
                Checkbox = Checkbox,
                CheckboxSelectAll = CheckboxSelectAll,
                DateOnly = DateOnly,
                Hidden = Hidden != null ? Hidden : $"Hidden {seed}",
                Password = Password != null ? Password : $"Password {seed}",
                Select = Select != null ? Select : $"Select {seed}",
                TextArea = TextArea != null ? TextArea : $"TextArea {seed}",
                Text = Text != null ? Text : $"Text {seed}",
                TextAutocomplete = TextAutocomplete != null ? TextAutocomplete : $"TextAutocomplete {seed}",
                DateTimeWithTimezones = DateTimeWithTimezones,
                DateTimeWithDataTimezone = DateTimeWithDataTimezone,
                DateTimeWithNoTimezone = DateTimeWithNoTimezone,
                CustomType = CustomType != null ? CustomType : $"CustomType {seed}",
                CheckboxOverride = CheckboxOverride,
                DateOnlyOverride = DateOnlyOverride,
                HiddenOverride = HiddenOverride != null ? HiddenOverride : $"HiddenOverride {seed}",
                PasswordOverride = PasswordOverride != null ? PasswordOverride : $"PasswordOverride {seed}",
                SelectOverride = SelectOverride != null ? SelectOverride : $"SelectOverride {seed}",
                TextAreaOverride = TextAreaOverride != null ? TextAreaOverride : $"TextAreaOverride {seed}",
                TextOverride = TextOverride != null ? TextOverride : $"TextOverride {seed}",
                CustomTypeOverride = CustomTypeOverride != null ? CustomTypeOverride : $"CustomTypeOverride {seed}",
                CheckboxOverrideSelectAll = CheckboxOverrideSelectAll,
                TextOverrideAutocomplete = TextOverrideAutocomplete != null ? TextOverrideAutocomplete : $"TextOverrideAutocomplete {seed}",
            };
        }

        public static ValueEntity CreateEntity(String seed = "", Guid? ValueId = default(Guid?), bool Checkbox = default(bool), bool CheckboxSelectAll = default(bool), DateTime DateOnly = default(DateTime), String Hidden = default(String), String Password = default(String), String Select = default(String), String TextArea = default(String), String Text = default(String), String TextAutocomplete = default(String), DateTime DateTimeWithTimezones = default(DateTime), DateTime DateTimeWithDataTimezone = default(DateTime), DateTime DateTimeWithNoTimezone = default(DateTime), String CustomType = default(String), bool CheckboxOverride = default(bool), DateTime DateOnlyOverride = default(DateTime), String HiddenOverride = default(String), String PasswordOverride = default(String), String SelectOverride = default(String), String TextAreaOverride = default(String), String TextOverride = default(String), String CustomTypeOverride = default(String), bool CheckboxOverrideSelectAll = default(bool), String TextOverrideAutocomplete = default(String))
        {
            return new ValueEntity()
            {
                ValueId = ValueId.HasValue ? ValueId.Value : Guid.NewGuid(),
                Checkbox = Checkbox,
                CheckboxSelectAll = CheckboxSelectAll,
                DateOnly = DateOnly,
                Hidden = Hidden != null ? Hidden : $"Hidden {seed}",
                Password = Password != null ? Password : $"Password {seed}",
                Select = Select != null ? Select : $"Select {seed}",
                TextArea = TextArea != null ? TextArea : $"TextArea {seed}",
                Text = Text != null ? Text : $"Text {seed}",
                TextAutocomplete = TextAutocomplete != null ? TextAutocomplete : $"TextAutocomplete {seed}",
                DateTimeWithTimezones = DateTimeWithTimezones,
                DateTimeWithDataTimezone = DateTimeWithDataTimezone,
                DateTimeWithNoTimezone = DateTimeWithNoTimezone,
                CustomType = CustomType != null ? CustomType : $"CustomType {seed}",
                CheckboxOverride = CheckboxOverride,
                DateOnlyOverride = DateOnlyOverride,
                HiddenOverride = HiddenOverride != null ? HiddenOverride : $"HiddenOverride {seed}",
                PasswordOverride = PasswordOverride != null ? PasswordOverride : $"PasswordOverride {seed}",
                SelectOverride = SelectOverride != null ? SelectOverride : $"SelectOverride {seed}",
                TextAreaOverride = TextAreaOverride != null ? TextAreaOverride : $"TextAreaOverride {seed}",
                TextOverride = TextOverride != null ? TextOverride : $"TextOverride {seed}",
                CustomTypeOverride = CustomTypeOverride != null ? CustomTypeOverride : $"CustomTypeOverride {seed}",
                CheckboxOverrideSelectAll = CheckboxOverrideSelectAll,
                TextOverrideAutocomplete = TextOverrideAutocomplete != null ? TextOverrideAutocomplete : $"TextOverrideAutocomplete {seed}",
            };
        }

        public static Value CreateView(String seed = "", Guid? ValueId = default(Guid?), bool Checkbox = default(bool), bool CheckboxSelectAll = default(bool), DateTime DateOnly = default(DateTime), String Hidden = default(String), String Password = default(String), String Select = default(String), String TextArea = default(String), String Text = default(String), String TextAutocomplete = default(String), DateTime DateTimeWithTimezones = default(DateTime), DateTime DateTimeWithDataTimezone = default(DateTime), DateTime DateTimeWithNoTimezone = default(DateTime), String CustomType = default(String), bool CheckboxOverride = default(bool), DateTime DateOnlyOverride = default(DateTime), String HiddenOverride = default(String), String PasswordOverride = default(String), String SelectOverride = default(String), String TextAreaOverride = default(String), String TextOverride = default(String), String CustomTypeOverride = default(String), bool CheckboxOverrideSelectAll = default(bool), String TextOverrideAutocomplete = default(String))
        {
            return new Value()
            {
                ValueId = ValueId.HasValue ? ValueId.Value : Guid.NewGuid(),
                Checkbox = Checkbox,
                CheckboxSelectAll = CheckboxSelectAll,
                DateOnly = DateOnly,
                Hidden = Hidden != null ? Hidden : $"Hidden {seed}",
                Password = Password != null ? Password : $"Password {seed}",
                Select = Select != null ? Select : $"Select {seed}",
                TextArea = TextArea != null ? TextArea : $"TextArea {seed}",
                Text = Text != null ? Text : $"Text {seed}",
                TextAutocomplete = TextAutocomplete != null ? TextAutocomplete : $"TextAutocomplete {seed}",
                DateTimeWithTimezones = DateTimeWithTimezones,
                DateTimeWithDataTimezone = DateTimeWithDataTimezone,
                DateTimeWithNoTimezone = DateTimeWithNoTimezone,
                CustomType = CustomType != null ? CustomType : $"CustomType {seed}",
                CheckboxOverride = CheckboxOverride,
                DateOnlyOverride = DateOnlyOverride,
                HiddenOverride = HiddenOverride != null ? HiddenOverride : $"HiddenOverride {seed}",
                PasswordOverride = PasswordOverride != null ? PasswordOverride : $"PasswordOverride {seed}",
                SelectOverride = SelectOverride != null ? SelectOverride : $"SelectOverride {seed}",
                TextAreaOverride = TextAreaOverride != null ? TextAreaOverride : $"TextAreaOverride {seed}",
                TextOverride = TextOverride != null ? TextOverride : $"TextOverride {seed}",
                CustomTypeOverride = CustomTypeOverride != null ? CustomTypeOverride : $"CustomTypeOverride {seed}",
                CheckboxOverrideSelectAll = CheckboxOverrideSelectAll,
                TextOverrideAutocomplete = TextOverrideAutocomplete != null ? TextOverrideAutocomplete : $"TextOverrideAutocomplete {seed}",
            };
        }

        public static void AssertEqual(ValueInput expected, ValueEntity actual)
        {
           Assert.Equal(expected.Checkbox, actual.Checkbox);
           Assert.Equal(expected.CheckboxSelectAll, actual.CheckboxSelectAll);
           Assert.Equal(expected.DateOnly, actual.DateOnly);
           Assert.Equal(expected.Hidden, actual.Hidden);
           Assert.Equal(expected.Password, actual.Password);
           Assert.Equal(expected.Select, actual.Select);
           Assert.Equal(expected.TextArea, actual.TextArea);
           Assert.Equal(expected.Text, actual.Text);
           Assert.Equal(expected.TextAutocomplete, actual.TextAutocomplete);
           Assert.Equal(expected.DateTimeWithTimezones, actual.DateTimeWithTimezones);
           Assert.Equal(expected.DateTimeWithDataTimezone, actual.DateTimeWithDataTimezone);
           Assert.Equal(expected.DateTimeWithNoTimezone, actual.DateTimeWithNoTimezone);
           Assert.Equal(expected.CustomType, actual.CustomType);
           Assert.Equal(expected.CheckboxOverride, actual.CheckboxOverride);
           Assert.Equal(expected.DateOnlyOverride, actual.DateOnlyOverride);
           Assert.Equal(expected.HiddenOverride, actual.HiddenOverride);
           Assert.Equal(expected.PasswordOverride, actual.PasswordOverride);
           Assert.Equal(expected.SelectOverride, actual.SelectOverride);
           Assert.Equal(expected.TextAreaOverride, actual.TextAreaOverride);
           Assert.Equal(expected.TextOverride, actual.TextOverride);
           Assert.Equal(expected.CustomTypeOverride, actual.CustomTypeOverride);
           Assert.Equal(expected.CheckboxOverrideSelectAll, actual.CheckboxOverrideSelectAll);
           Assert.Equal(expected.TextOverrideAutocomplete, actual.TextOverrideAutocomplete);
        }

        public static void AssertEqual(ValueEntity expected, Value actual)
        {
           Assert.Equal(expected.Checkbox, actual.Checkbox);
           Assert.Equal(expected.CheckboxSelectAll, actual.CheckboxSelectAll);
           Assert.Equal(expected.DateOnly, actual.DateOnly);
           Assert.Equal(expected.Hidden, actual.Hidden);
           Assert.Equal(expected.Password, actual.Password);
           Assert.Equal(expected.Select, actual.Select);
           Assert.Equal(expected.TextArea, actual.TextArea);
           Assert.Equal(expected.Text, actual.Text);
           Assert.Equal(expected.TextAutocomplete, actual.TextAutocomplete);
           Assert.Equal(expected.DateTimeWithTimezones, actual.DateTimeWithTimezones);
           Assert.Equal(expected.DateTimeWithDataTimezone, actual.DateTimeWithDataTimezone);
           Assert.Equal(expected.DateTimeWithNoTimezone, actual.DateTimeWithNoTimezone);
           Assert.Equal(expected.CustomType, actual.CustomType);
           Assert.Equal(expected.CheckboxOverride, actual.CheckboxOverride);
           Assert.Equal(expected.DateOnlyOverride, actual.DateOnlyOverride);
           Assert.Equal(expected.HiddenOverride, actual.HiddenOverride);
           Assert.Equal(expected.PasswordOverride, actual.PasswordOverride);
           Assert.Equal(expected.SelectOverride, actual.SelectOverride);
           Assert.Equal(expected.TextAreaOverride, actual.TextAreaOverride);
           Assert.Equal(expected.TextOverride, actual.TextOverride);
           Assert.Equal(expected.CustomTypeOverride, actual.CustomTypeOverride);
           Assert.Equal(expected.CheckboxOverrideSelectAll, actual.CheckboxOverrideSelectAll);
           Assert.Equal(expected.TextOverrideAutocomplete, actual.TextOverrideAutocomplete);
        }

    }
}