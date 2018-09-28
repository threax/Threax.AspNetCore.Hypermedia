using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using NJsonSchema;
using NJsonSchema.Generation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext.ValueProviders;
using Threax.AspNetCore.Tests;
using Xunit;

namespace Threax.AspNetCore.Halcyon.Ext.Tests
{
    public class SchemaJsonConverterTests
    {
        private Mockup mockup = new Mockup().SetupGlobal();

        public SchemaJsonConverterTests()
        {
            
        }

        [Fact]
        public Task TestSimple()
        {
            return TestSchema(typeof(TestType), "TestSimple.json");
        }

        [Fact]
        public Task TestACROSimple()
        {
            return TestSchema(typeof(ACROTestCapital), "TestACROSimple.json");
        }

        [Fact]
        public Task TestClassWithItems()
        {
            return TestSchema(typeof(ClassWithItems), "TestClassWithItems.json");
        }

        [Fact]
        public Task TestSimpleArray()
        {
            return TestSchema(typeof(TestSimpleArray), "TestSimpleArray.json");
        }

        [Fact]
        public Task TestComplexArray()
        {
            return TestSchema(typeof(TestComplexArrayType), "TestComplexArray.json");
        }

        [Fact]
        public Task TestEnum()
        {
            return TestSchema(typeof(EnumClass), "TestEnum.json");
        }

        [Fact]
        public Task TestOptionalEnum()
        {
            return TestSchema(typeof(OptionalEnumClass), "TestOptionalEnum.json");
        }

        [Fact]
        public Task TestEnumArray()
        {
            return TestSchema(typeof(EnumArrayClass), "TestEnumArray.json");
        }

        [Fact]
        public Task TestDisplayExpression()
        {
            return TestSchema(typeof(DisplayExpressionClass), "TestDisplayExpression.json");
        }

        [Fact]
        public Task TestSearchAttribute()
        {
            return TestSchema(typeof(SearchPropertyClass), "TestSearchAttribute.json");
        }

        [Fact]
        public Task TestFileProperty()
        {
            return TestSchema(typeof(FileInputClass), "TestFileProperty.json");
        }

        [Fact]
        public Task TestReadOnly()
        {
            return TestSchema(typeof(ReadOnlyClass), "TestReadOnly.json");
        }

        [Fact]
        public Task TestRequiredValueProvider()
        {
            mockup.MockServiceCollection.AddConventionalHalcyon(new HalcyonConventionOptions());
            mockup.MockServiceCollection.AddScoped<TestValueProvider>();
            return TestSchema(typeof(RequiredValueProviderClass), "TestRequiredValueProvider.json");
        }

        [Fact]
        public Task TestOptionalValueProvider()
        {
            mockup.MockServiceCollection.AddConventionalHalcyon(new HalcyonConventionOptions());
            mockup.MockServiceCollection.AddScoped<TestValueProvider>();
            return TestSchema(typeof(OptionalValueProviderClass), "TestOptionalValueProvider.json");
        }

        [Fact]
        public Task TestValueType()
        {
            return TestSchema(typeof(ValueTypeClass), "TestValueType.json");
        }

        [Fact]
        public Task TestValueTypeNullable()
        {
            return TestSchema(typeof(ValueTypeNullableClass), "TestValueTypeNullable.json");
        }

        [Fact]
        public Task TestValueTypeNullableRequired()
        {
            return TestSchema(typeof(ValueTypeNullableRequiredClass), "TestValueTypeNullableRequired.json");
        }

        [Fact]
        public Task TestReferenceType()
        {
            return TestSchema(typeof(ReferenceTypeClass), "TestReferenceType.json");
        }

        private async Task TestSchema(Type type, String Filename)
        {
            var schemaBuilder = mockup.Get<ISchemaBuilder>();
            var schema = await schemaBuilder.GetSchema(type);
            var converter = mockup.Get<SchemaJsonConverter>();
            var serializer = JsonSerializer.Create(HalcyonConvention.DefaultJsonSerializerSettings);
            String finalJson;
            using (var stream = new MemoryStream())
            using (var streamWriter = new StreamWriter(stream))
            using (var jsonWriter = new JsonTextWriter(streamWriter))
            {
                converter.WriteJson(jsonWriter, schema, serializer);
                jsonWriter.Flush();
                streamWriter.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                using (var streamReader = new StreamReader(stream))
                {
                    finalJson = streamReader.ReadToEnd();
                }
            }

            FileUtils.WriteTestFile(this.GetType(), Filename, finalJson);
            var expected = FileUtils.ReadTestFile(this.GetType(), Filename);
            Assert.Equal(expected, finalJson);
        }
    }

    public class ClassWithItems
    {
        public TestType TestType { get; set; }

        public bool MyProp { get; set; }

        public ACROTestCapital ACROTest { get; set; }
    }

    public class ACROTestCapital
    {
        public bool COOLValue { get; set; }
    }

    public class TestComplexArrayType
    {
        public List<TestType> Children { get; set; }
    }

    public class TestType
    {
        public String Name { get; set; }
    }

    public class TestSimpleArray
    {
        public List<Guid> Ids { get; set; }
    }

    public enum TestEnum
    {
        [Display(Name = "One (uno)")]
        One,
        [Display(Name = "Two (dos)")]
        Two,
        [Display(Name = "Three (tres)")]
        Three
    }

    public class EnumClass
    {
        public TestEnum Value { get; set; }
    }

    public class OptionalEnumClass
    {
        public TestEnum? Value { get; set; }
    }

    public class EnumArrayClass
    {
        public List<TestEnum> Value { get; set; }
    }

    public class DisplayExpressionClass
    {
        private static readonly Expression<Func<DisplayExpressionClass, bool>> IsConditionExpression = i => i.ConditionProperty == 5;
        [DisplayExpression(nameof(IsConditionExpression))]
        public Guid? OptionalProperty { get; set; }

        public int ConditionProperty { get; set; }
    }

    public class SearchPropertyClass
    {
        [SearchValue("TheProvider", CurrentValuePropertyName = nameof(OtherProperty))]
        public Guid TheProperty { get; set; }

        public String OtherProperty { get; set; }
    }

    public class FileInputClass
    {
        public IFormFile FormFile { get; set; }
    }

    public class ReadOnlyClass
    {
        [ReadOnly(true)]
        public String TheProp { get; set; }
    }

    public class TestValueProvider : LabelValuePairProviderSync
    {
        protected override IEnumerable<ILabelValuePair> GetSourcesSync()
        {
            return new List<ILabelValuePair<bool>>() { new LabelValuePair<bool>("No", false), new LabelValuePair<bool>("Yes", true) };
        }
    }

    public class RequiredValueProviderClass
    {
        [ValueProvider(typeof(TestValueProvider))]
        public bool TheProp { get; set; }
    }

    public class OptionalValueProviderClass
    {
        [ValueProvider(typeof(TestValueProvider))]
        public bool? TheProp { get; set; }
    }

    public class ValueTypeClass
    {
        public int ValueType { get; set; }
    }

    public class ValueTypeNullableClass
    {
        public int? ValueType { get; set; }
    }

    public class ValueTypeNullableRequiredClass
    {
        [Required]
        public int? ValueType { get; set; }
    }

    public class ReferenceTypeClass
    {
        public String ReferenceType { get; set; }
    }
}
