using NJsonSchema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.Tests;
using Threax.ModelGen.TestGenerators;
using Xunit;

namespace Threax.ModelGen.Tests
{
    public abstract class ModelTests<T>
    {
        private const String AppNamespace = "Test";
        private JsonSchema4 schema;
        protected bool WriteTestFiles = false;

        public ModelTests()
        {
            schema = Task.Run(async () => await TypeToSchemaGenerator.CreateSchema(typeof(T))).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Test some code, sending null for code means that no file should exist for that code
        /// and this will be checked.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="code">The code to test.</param>
        private void TestCode(String fileName, String code)
        {
            if (WriteTestFiles)
            {
                if (code != null)
                {
                    FileUtils.WriteTestFile(this.GetType(), fileName, code);
                }
                else
                {
                    FileUtils.DeleteTestFile(this.GetType(), fileName);
                }
            }

            if (code != null)
            {
                Assert.Equal(FileUtils.ReadTestFile(this.GetType(), fileName), code);
            }
            else
            {
                Assert.False(File.Exists(fileName));
            }
        }

        [Fact]
        public void Interface()
        {
            TestCode
            (
                $"Models/I{schema.Title}.cs",
                PartialModelInterfaceGenerator.GetUserPartial(schema, AppNamespace + ".Models")
            );
        }

        [Fact]
        public void InterfaceGenerated()
        {
            TestCode
            (
                $"Models/I{schema.Title}.Generated.cs",
                IdInterfaceWriter.Create(schema, AppNamespace)
            );
        }

        [Fact]
        public void Entity()
        {
            TestCode
            (
                $"Database/{schema.Title}Entity.cs",
                PartialTypeGenerator.GetUserPartial(schema.Title, AppNamespace + ".Database", "Entity", schema.GetExtraNamespaces(StrConstants.FileNewline))
            );
        }

        [Fact]
        public void EntityGenerated()
        {
            TestCode
            (
                $"Database/{schema.Title}Entity.Generated.cs",
                EntityWriter.Create(schema, AppNamespace)
            );
        }

        [Fact]
        public void InputModel()
        {
            TestCode
            (
                $"InputModels/{schema.Title}Input.cs",
                PartialTypeGenerator.GetUserPartial(schema.Title, AppNamespace + ".InputModels", "Input", schema.GetExtraNamespaces(StrConstants.FileNewline))
            );
        }

        [Fact]
        public void InputModelGenerated()
        {
            TestCode
            (
                $"InputModels/{schema.Title}Input.Generated.cs",
                InputModelWriter.Create(schema, AppNamespace)
            );
        }

        [Fact]
        public void Query()
        {
            TestCode
            (
                $"InputModels/{schema.Title}Query.cs",
                PartialTypeGenerator.GetUserPartial(schema.Title, AppNamespace + ".InputModels", "Query", schema.GetExtraNamespaces(StrConstants.FileNewline))
            );
        }

        [Fact]
        public void QueryGenerated()
        {
            TestCode
            (
                $"InputModels/{schema.Title}Query.Generated.cs",
                QueryModelWriter.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void ViewModel()
        {
            TestCode
            (
                $"ViewModels/{schema.Title}.cs",
                ViewModelWriter.GetUserPartial(schema, AppNamespace)
            );
        }

        [Fact]
        public void ViewModelGenerated()
        {
            TestCode
            (
                $"ViewModels/{schema.Title}.Generated.cs",
                ViewModelWriter.Create(schema, AppNamespace)
            );
        }

        [Fact]
        public void Repository()
        {
            TestCode
            (
                $"Repository/{schema.Title}Repository.cs",
                RepoGenerator.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void RepositoryInterface()
        {
            TestCode
            (
                $"Repository/I{schema.Title}Repository.cs",
                RepoInterfaceGenerator.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void RepositoryConfig()
        {
            TestCode
            (
                $"Repository/{schema.Title}Repository.Config.cs",
                RepoConfigGenerator.Get(AppNamespace, schema.Title)
            );
        }

        [Fact]
        public void ApiController()
        {
            TestCode
            (
                $"Controllers/Api/{schema.GetPluralName()}Controller.cs",
                ControllerGenerator.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void Profile()
        {
            TestCode
            (
                $"Mappers/{schema.Title}Profile.cs",
                MappingProfileGenerator.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void ProfileGenerated()
        {
            TestCode
            (
                $"Mappers/{schema.Title}Profile.Generated.cs",
                MappingProfileGenerator.GetGenerated(schema, AppNamespace)
            );
        }

        [Fact]
        public void AppDbContext()
        {
            TestCode
            (
                $"Database/AppDbContext.{schema.Title}.cs",
                AppDbContextGenerator.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void ModelCollection()
        {
            TestCode
            (
                $"ViewModels/{schema.Title}Collection.cs",
                ModelCollectionGenerator.GetUserPartial(schema, AppNamespace)
            );
        }

        [Fact]
        public void ModelCollectionGenerated()
        {
            TestCode
            (
                $"ViewModels/{schema.Title}Collection.Generated.cs",
                ModelCollectionGenerator.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void EntryPoint()
        {
            TestCode
            (
                $"ViewModels/EntryPoint.{schema.Title}.cs",
                EntryPointGenerator.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void Cshtml()
        {
            var propertyNames = schema.Properties.Values.Where(i => i.CreateViewModel()).Select(i => NameGenerator.CreatePascal(i.Name));
            TestCode
            (
                $"Views/{schema.GetUiControllerName()}/{schema.GetPluralName()}.cshtml",
                CrudCshtmlInjectorGenerator.Get(schema, propertyNames: propertyNames)
            );
        }

        [Fact]
        public void CrudInjector()
        {
            TestCode
            (
                $"Client/Libs/{schema.Title}CrudInjector.ts",
                CrudInjectorGenerator.Get(schema)
            );
        }

        [Fact]
        public void UiTypescript()
        {
            TestCode
            (
                $"Views/{schema.GetUiControllerName()}/{schema.GetPluralName()}.ts",
                CrudUiTypescriptGenerator.Get(schema.Title)
            );
        }

        [Fact]
        public void ViewController()
        {
            TestCode
            (
                $"Controllers/{schema.GetUiControllerName()}Controller.{schema.GetPluralName()}.cs",
                UiControllerGenerator.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void TestWrapper()
        {
            TestCode
            (
                $"Tests/{schema.Title}Tests.cs",
                ModelTestWrapper.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void TestWrapperGenerated()
        {
            TestCode
            (
                $"Tests/{schema.Title}Tests.Generated.cs",
                ModelTestWrapperGenerated.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void TestController()
        {
            TestCode
            (
                $"Tests/Controller.cs",
                ControllerTests.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void TestProfile()
        {
            TestCode
            (
                $"Tests/Profile.cs",
                ProfileTests.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void TestRepository()
        {
            TestCode
            (
                $"Tests/Repository.cs",
                RepositoryTests.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void Relationship()
        {
            TestCode
            (
                $"Database/{schema.Title}Entity.Generated.{schema.GetOtherModelName()}Relationship.cs",
                RelationshipWriter.Get(schema, AppNamespace)
            );
        }
    }
}
