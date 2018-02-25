﻿using NJsonSchema;
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
    public abstract class ModelTests<T, TO> : ModelTests<T>
    {
        public ModelTests()
        {
            var otherSchema = Task.Run(async () => await TypeToSchemaGenerator.CreateSchema(typeof(TO))).GetAwaiter().GetResult();
            this.otherSchema[otherSchema.Title] = otherSchema;
        }
    }

    public abstract class ModelTests<T>
    {
        private const String AppNamespace = "Test";
        private JsonSchema4 schema;
        protected Dictionary<String, JsonSchema4> otherSchema = new Dictionary<string, JsonSchema4>();
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
                PartialModelInterfaceGenerator.GetFileName(schema),
                PartialModelInterfaceGenerator.GetUserPartial(schema, AppNamespace + ".Models")
            );
        }

        [Fact]
        public void InterfaceGenerated()
        {
            TestCode
            (
                IdInterfaceWriter.GetFileName(schema),
                IdInterfaceWriter.Create(schema, AppNamespace)
            );
        }

        [Fact]
        public void Entity()
        {
            TestCode
            (
                PartialTypeGenerator.GetEntityFileName(schema),
                PartialTypeGenerator.GetEntity(schema, AppNamespace)
            );
        }

        [Fact]
        public void EntityGenerated()
        {
            TestCode
            (
                EntityWriter.GetFileName(schema),
                EntityWriter.Create(schema, otherSchema, AppNamespace)
            );
        }

        [Fact]
        public void InputModel()
        {
            TestCode
            (
                PartialTypeGenerator.GetInputFileName(schema),
                PartialTypeGenerator.GetInput(schema, AppNamespace)
            );
        }

        [Fact]
        public void InputModelGenerated()
        {
            TestCode
            (
                InputModelWriter.GetFileName(schema),
                InputModelWriter.Create(schema, otherSchema, AppNamespace)
            );
        }

        [Fact]
        public void Query()
        {
            TestCode
            (
                PartialTypeGenerator.GetQueryFileName(schema),
                PartialTypeGenerator.GetQuery(schema, AppNamespace)
            );
        }

        [Fact]
        public void QueryGenerated()
        {
            TestCode
            (
                QueryModelWriter.GetFileName(schema),
                QueryModelWriter.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void ViewModel()
        {
            TestCode
            (
                ViewModelWriter.GetUserPartialFileName(schema),
                ViewModelWriter.GetUserPartial(schema, AppNamespace)
            );
        }

        [Fact]
        public void ViewModelGenerated()
        {
            TestCode
            (
                ViewModelWriter.GetFileName(schema),
                ViewModelWriter.Create(schema, otherSchema, AppNamespace)
            );
        }

        [Fact]
        public void Repository()
        {
            TestCode
            (
                RepoGenerator.GetFileName(schema),
                RepoGenerator.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void RepositoryInterface()
        {
            TestCode
            (
                RepoInterfaceGenerator.GetFileName(schema),
                RepoInterfaceGenerator.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void RepositoryConfig()
        {
            TestCode
            (
                RepoConfigGenerator.GetFileName(schema),
                RepoConfigGenerator.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void ApiController()
        {
            TestCode
            (
                ControllerGenerator.GetFileName(schema),
                ControllerGenerator.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void Profile()
        {
            TestCode
            (
                MappingProfileGenerator.GetFileName(schema),
                MappingProfileGenerator.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void ProfileGenerated()
        {
            TestCode
            (
                MappingProfileGenerator.GetGeneratedFileName(schema),
                MappingProfileGenerator.GetGenerated(schema, AppNamespace)
            );
        }

        [Fact]
        public void AppDbContext()
        {
            TestCode
            (
                AppDbContextGenerator.GetFileName(schema),
                AppDbContextGenerator.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void ModelCollection()
        {
            TestCode
            (
                ModelCollectionGenerator.GetUserPartialFileName(schema),
                ModelCollectionGenerator.GetUserPartial(schema, AppNamespace)
            );
        }

        [Fact]
        public void ModelCollectionGenerated()
        {
            TestCode
            (
                ModelCollectionGenerator.GetFileName(schema),
                ModelCollectionGenerator.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void EntryPoint()
        {
            TestCode
            (
                EntryPointGenerator.GetFileName(schema),
                EntryPointGenerator.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void Cshtml()
        {
            var propertyNames = schema.Properties.Values.Where(i => i.CreateViewModel()).Select(i => NameGenerator.CreatePascal(i.Name));
            TestCode
            (
                CrudCshtmlInjectorGenerator.GetFileName(schema),
                CrudCshtmlInjectorGenerator.Get(schema, propertyNames: propertyNames)
            );
        }

        [Fact]
        public void CrudInjector()
        {
            TestCode
            (
                CrudInjectorGenerator.GetFileName(schema),
                CrudInjectorGenerator.Get(schema)
            );
        }

        [Fact]
        public void UiTypescript()
        {
            TestCode
            (
                CrudUiTypescriptGenerator.GetFileName(schema),
                CrudUiTypescriptGenerator.Get(schema.Title)
            );
        }

        [Fact]
        public void ViewController()
        {
            TestCode
            (
                UiControllerGenerator.GetFileName(schema),
                UiControllerGenerator.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void TestWrapper()
        {
            TestCode
            (
                Path.Combine("Tests", ModelTestWrapper.GetFileName(schema)),
                ModelTestWrapper.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void TestWrapperGenerated()
        {
            TestCode
            (
                Path.Combine("Tests", ModelTestWrapperGenerated.GetFileName(schema)),
                ModelTestWrapperGenerated.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void TestController()
        {
            TestCode
            (
                Path.Combine("Tests", ControllerTests.GetFileName(schema)),
                ControllerTests.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void TestProfile()
        {
            TestCode
            (
                Path.Combine("Tests", ProfileTests.GetFileName(schema)),
                ProfileTests.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void TestRepository()
        {
            TestCode
            (
                Path.Combine("Tests", RepositoryTests.GetFileName(schema)),
                RepositoryTests.Get(schema, AppNamespace)
            );
        }

        [Fact]
        public void JoinEntityGenerated()
        {
            foreach (var relationship in schema.GetRelationshipSettings())
            {
                TestCode
                (
                    JoinEntityWriter.GetFileName(relationship),
                    JoinEntityWriter.Get(schema, otherSchema, relationship, AppNamespace)
                );
            }
        }

        [Fact]
        public void JoinEntity()
        {
            foreach (var relationship in schema.GetRelationshipSettings())
            {
                TestCode
                (
                    PartialTypeGenerator.GetJoinEntityFileName(relationship),
                    PartialTypeGenerator.GetJoinEntity(schema, relationship, AppNamespace)
                );
            }
        }

        [Fact]
        public void JoinEntityDbContext()
        {
            foreach (var relationship in schema.GetRelationshipSettings())
            {
                TestCode
                (
                    AppDbContextGenerator.GetManyToManyEntityDbContextFileName(relationship),
                    AppDbContextGenerator.GetManyToManyEntityDbContext(relationship, AppNamespace)
                );
            }
        }
    }
}
