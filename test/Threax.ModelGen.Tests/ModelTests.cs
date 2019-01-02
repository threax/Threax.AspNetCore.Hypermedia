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
        protected bool WriteTestFiles = false;

        public ModelTests()
        {

        }

        private static JsonSchema4 ___LoadedSchemaDoNotUse;
        private async Task<JsonSchema4> GetSchema()
        {
            if (___LoadedSchemaDoNotUse == null)
            {
                ___LoadedSchemaDoNotUse = await TypeToSchemaGenerator.CreateSchema(typeof(T));
            }
            return ___LoadedSchemaDoNotUse;
        }

        protected virtual Task<Dictionary<String, JsonSchema4>> GetOtherSchema()
        {
            return Task.FromResult(new Dictionary<String, JsonSchema4>());
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
        public async Task Interface()
        {
            TestCode
            (
                IdInterfaceWriter.GetFileName(await GetSchema(), false),
                PartialModelInterfaceGenerator.GetUserPartial(await GetSchema(), AppNamespace + ".Models")
            );
        }

        [Fact]
        public async Task InterfaceGenerated()
        {
            TestCode
            (
                IdInterfaceWriter.GetFileName(await GetSchema(), true),
                IdInterfaceWriter.Create(await GetSchema(), AppNamespace)
            );
        }

        [Fact]
        public async Task Entity()
        {
            TestCode
            (
                EntityWriter.GetFileName(await GetSchema(), false),
                PartialTypeGenerator.GetEntity(await GetSchema(), AppNamespace)
            );
        }

        [Fact]
        public async Task EntityGenerated()
        {
            TestCode
            (
                EntityWriter.GetFileName(await GetSchema(), true),
                EntityWriter.Create(await GetSchema(), await GetOtherSchema(), AppNamespace)
            );
        }

        [Fact]
        public async Task InputModel()
        {
            TestCode
            (
                InputModelWriter.GetFileName(await GetSchema(), false),
                PartialTypeGenerator.GetInput(await GetSchema(), AppNamespace)
            );
        }

        [Fact]
        public async Task InputModelGenerated()
        {
            TestCode
            (
                InputModelWriter.GetFileName(await GetSchema(), true),
                await InputModelWriter.Create(await GetSchema(), await GetOtherSchema(), AppNamespace)
            );
        }

        [Fact]
        public async Task Query()
        {
            TestCode
            (
                QueryModelWriter.GetFileName(await GetSchema(), false),
                QueryUserPartialGenerator.GetQuery(await GetSchema(), AppNamespace)
            );
        }

        [Fact]
        public async Task QueryGenerated()
        {
            TestCode
            (
                QueryModelWriter.GetFileName(await GetSchema(), true),
                QueryModelWriter.Get(await GetSchema(), AppNamespace, true)
            );
        }

        [Fact]
        public async Task QueryComplete()
        {
            TestCode
            (
                QueryModelWriter.GetFileName(await GetSchema(), false) + ".Complete.cs",
                QueryModelWriter.Get(await GetSchema(), AppNamespace, false)
            );
        }

        [Fact]
        public async Task ViewModel()
        {
            TestCode
            (
                ViewModelWriter.GetFileName(await GetSchema(), false),
                ViewModelWriter.GetUserPartial(await GetSchema(), AppNamespace)
            );
        }

        [Fact]
        public async Task ViewModelGenerated()
        {
            TestCode
            (
                ViewModelWriter.GetFileName(await GetSchema(), true),
                await ViewModelWriter.Create(await GetSchema(), await GetOtherSchema(), AppNamespace, true)
            );
        }

        [Fact]
        public async Task ViewModelComplete()
        {
            TestCode
            (
                ViewModelWriter.GetFileName(await GetSchema(), false) + ".Complete.cs",
                await ViewModelWriter.Create(await GetSchema(), await GetOtherSchema(), AppNamespace, false)
            );
        }

        [Fact]
        public async Task Repository()
        {
            TestCode
            (
                RepoGenerator.GetFileName(await GetSchema()),
                RepoGenerator.Get(await GetSchema(), AppNamespace)
            );
        }

        [Fact]
        public async Task RepositoryInterface()
        {
            TestCode
            (
                RepoInterfaceGenerator.GetFileName(await GetSchema()),
                RepoInterfaceGenerator.Get(await GetSchema(), AppNamespace)
            );
        }

        [Fact]
        public async Task RepositoryConfig()
        {
            TestCode
            (
                RepoConfigGenerator.GetFileName(await GetSchema()),
                RepoConfigGenerator.Get(await GetSchema(), AppNamespace)
            );
        }

        [Fact]
        public async Task ApiController()
        {
            TestCode
            (
                ControllerGenerator.GetFileName(await GetSchema()),
                ControllerGenerator.Get(await GetSchema(), AppNamespace)
            );
        }

        [Fact]
        public async Task Profile()
        {
            TestCode
            (
                MappingProfileGenerator.GetFileName(await GetSchema(), false),
                MappingProfileGenerator.Get(await GetSchema(), AppNamespace, true)
            );
        }

        [Fact]
        public async Task ProfileGenerated()
        {
            TestCode
            (
                MappingProfileGenerator.GetFileName(await GetSchema(), true),
                MappingProfileGenerator.GetGenerated(await GetSchema(), AppNamespace)
            );
        }

        [Fact]
        public async Task ProfileComplete()
        {
            TestCode
            (
                MappingProfileGenerator.GetFileName(await GetSchema(), false) + ".Complete.cs",
                MappingProfileGenerator.Get(await GetSchema(), AppNamespace, false)
            );
        }

        [Fact]
        public async Task AppDbContext()
        {
            TestCode
            (
                AppDbContextGenerator.GetFileName(await GetSchema()),
                AppDbContextGenerator.Get(await GetSchema(), AppNamespace)
            );
        }

        [Fact]
        public async Task ModelCollection()
        {
            TestCode
            (
                ModelCollectionGenerator.GetFileName(await GetSchema(), false),
                ModelCollectionGenerator.GetUserPartial(await GetSchema(), AppNamespace)
            );
        }

        [Fact]
        public async Task ModelCollectionGenerated()
        {
            TestCode
            (
                ModelCollectionGenerator.GetFileName(await GetSchema(), true),
                ModelCollectionGenerator.Get(await GetSchema(), AppNamespace, true)
            );
        }

        [Fact]
        public virtual async Task ModelCollectionComplete()
        {
            TestCode
            (
                ModelCollectionGenerator.GetFileName(await GetSchema(), false) + ".Complete.cs",
                ModelCollectionGenerator.Get(await GetSchema(), AppNamespace, false)
            );
        }

        [Fact]
        public async Task EntryPoint()
        {
            TestCode
            (
                EntryPointGenerator.GetFileName(await GetSchema()),
                EntryPointGenerator.Get(await GetSchema(), AppNamespace)
            );
        }

        [Fact]
        public async Task Cshtml()
        {
            var propertyNames = (await GetSchema()).Properties.Values.Where(i => i.CreateViewModel()).Select(i => NameGenerator.CreatePascal(i.Name));
            TestCode
            (
                CrudCshtmlInjectorGenerator.GetFileName(await GetSchema()),
                CrudCshtmlInjectorGenerator.Get(await GetSchema(), propertyNames: propertyNames)
            );
        }

        [Fact]
        public async Task CrudInjector()
        {
            TestCode
            (
                CrudInjectorGenerator.GetFileName(await GetSchema()),
                CrudInjectorGenerator.Get(await GetSchema())
            );
        }

        [Fact]
        public async Task UiTypescript()
        {
            TestCode
            (
                CrudUiTypescriptGenerator.GetFileName(await GetSchema()),
                CrudUiTypescriptGenerator.Get((await GetSchema()).Title)
            );
        }

        [Fact]
        public async Task ViewController()
        {
            TestCode
            (
                UiControllerGenerator.GetFileName(await GetSchema()),
                UiControllerGenerator.Get(await GetSchema(), AppNamespace)
            );
        }

        [Fact]
        public async Task TestWrapper()
        {
            TestCode
            (
                Path.Combine("Tests", ModelTestWrapper.GetFileName(await GetSchema())),
                ModelTestWrapper.Get(await GetSchema(), AppNamespace)
            );
        }

        [Fact]
        public async Task TestWrapperGenerated()
        {
            TestCode
            (
                Path.Combine("Tests", ModelTestWrapperGenerated.GetFileName(await GetSchema())),
                ModelTestWrapperGenerated.Get(await GetSchema(), AppNamespace)
            );
        }

        [Fact]
        public async Task TestController()
        {
            TestCode
            (
                Path.Combine("Tests", ControllerTests.GetFileName(await GetSchema())),
                ControllerTests.Get(await GetSchema(), AppNamespace)
            );
        }

        [Fact]
        public async Task TestProfile()
        {
            TestCode
            (
                Path.Combine("Tests", ProfileTests.GetFileName(await GetSchema())),
                ProfileTests.Get(await GetSchema(), AppNamespace)
            );
        }

        [Fact]
        public async Task TestRepository()
        {
            TestCode
            (
                Path.Combine("Tests", RepositoryTests.GetFileName(await GetSchema())),
                RepositoryTests.Get(await GetSchema(), AppNamespace)
            );
        }

        [Fact]
        public async Task JoinEntityGenerated()
        {
            foreach (var relationship in (await GetSchema()).GetRelationshipSettings())
            {
                TestCode
                (
                    JoinEntityWriter.GetFileName(relationship, true),
                    JoinEntityWriter.Get(await GetSchema(), await GetOtherSchema(), relationship, AppNamespace)
                );
            }
        }

        [Fact]
        public async Task JoinEntity()
        {
            foreach (var relationship in (await GetSchema()).GetRelationshipSettings())
            {
                TestCode
                (
                    JoinEntityWriter.GetFileName(relationship, false),
                    PartialTypeGenerator.GetJoinEntity(await GetSchema(), relationship, AppNamespace)
                );
            }
        }

        [Fact]
        public async Task JoinEntityDbContext()
        {
            foreach (var relationship in (await GetSchema()).GetRelationshipSettings())
            {
                TestCode
                (
                    AppDbContextGenerator.GetManyToManyEntityDbContextFileName(relationship),
                    AppDbContextGenerator.GetManyToManyEntityDbContext(relationship, AppNamespace)
                );
            }
        }
    }

    public abstract class ModelTests<T, TO> : ModelTests<T>
    {
        public ModelTests()
        {
                        
        }

        private static Dictionary<String, JsonSchema4> ___LoadedOtherSchemaDoNotUse;
        protected override async Task<Dictionary<String, JsonSchema4>> GetOtherSchema()
        {
            if (___LoadedOtherSchemaDoNotUse == null)
            {
                ___LoadedOtherSchemaDoNotUse = new Dictionary<string, JsonSchema4>();;
                var schema = await TypeToSchemaGenerator.CreateSchema(typeof(TO));
                ___LoadedOtherSchemaDoNotUse[schema.Title] = schema;
            }
            return ___LoadedOtherSchemaDoNotUse;
        }
    }

    public abstract class ModelTests<T, TO, TO2> : ModelTests<T, TO>
    {
        public ModelTests()
        {
            
        }

        private static Dictionary<String, JsonSchema4> ___LoadedOtherSchemaDoNotUse;
        protected override async Task<Dictionary<String, JsonSchema4>> GetOtherSchema()
        {
            if (___LoadedOtherSchemaDoNotUse == null)
            {
                ___LoadedOtherSchemaDoNotUse = new Dictionary<string, JsonSchema4>();
                var schema = await TypeToSchemaGenerator.CreateSchema(typeof(TO));
                ___LoadedOtherSchemaDoNotUse[schema.Title] = schema;
                schema = await TypeToSchemaGenerator.CreateSchema(typeof(TO2));
                ___LoadedOtherSchemaDoNotUse[schema.Title] = schema;
            }
            return ___LoadedOtherSchemaDoNotUse;
        }
    }
}
