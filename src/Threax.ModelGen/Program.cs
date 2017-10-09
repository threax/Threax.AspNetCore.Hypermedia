using NJsonSchema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Threax.ModelGen
{
    class Program
    {
        static void Main(string[] args)
        {
            GenerateClasses(new GeneratorSettings()
            {
                Source = args[0],
                AppOutDir = args[1],
                AppNamespace = args[2],
            });
        }

        class GeneratorSettings
        {
            public String AppNamespace { get; set; }

            public String Source { get; set; }

            public String AppOutDir { get; set; }

            public string UiController { get; set; } = "Home";
        }

        private static void GenerateClasses(GeneratorSettings settings)
        {
            String modelName, model, entity, inputModel, viewModel;

            JsonSchema4 schema = null;
            try
            {
                var schemaTask = JsonSchema4.FromJsonAsync(settings.Source);
                schemaTask.Wait();
                schema = schemaTask.Result;
            }
            catch (Exception)
            {
                if (File.Exists(settings.Source))
                {
                    try
                    {
                        var schemaTask = JsonSchema4.FromFileAsync(settings.Source);
                        schemaTask.Wait();
                        schema = schemaTask.Result;
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            if (schema != null)
            {
                modelName = schema.Title;
            }
            else
            {
                if (HasWhitespace(settings.Source))
                {
                    throw new InvalidOperationException($"Invalid model name {settings.Source}");
                }

                modelName = settings.Source;
            }

            List<String> propertyNames = null;

            if (settings.AppOutDir != null && settings.AppNamespace != null)
            {
                if (schema != null)
                {
                    model = ModelTypeGenerator.Create(schema, new IdInterfaceWriter(), schema, settings.AppNamespace, settings.AppNamespace + ".Models");
                    entity = ModelTypeGenerator.Create(schema, new EntityWriter(), schema, settings.AppNamespace, settings.AppNamespace + ".Database");
                    inputModel = ModelTypeGenerator.Create(schema, new InputModelWriter(), schema, settings.AppNamespace, settings.AppNamespace + ".InputModels");
                    viewModel = ModelTypeGenerator.Create(schema, new ViewModelWriter(), schema, settings.AppNamespace, settings.AppNamespace + ".ViewModels");
                }
                else
                {
                    model = ModelTypeGenerator.Create(settings.Source, new IdInterfaceWriter(), settings.AppNamespace, settings.AppNamespace + ".Models");
                    entity = ModelTypeGenerator.Create(settings.Source, new EntityWriter(), settings.AppNamespace, settings.AppNamespace + ".Database");
                    inputModel = ModelTypeGenerator.Create(settings.Source, new InputModelWriter(), settings.AppNamespace, settings.AppNamespace + ".InputModels");
                    viewModel = ModelTypeGenerator.Create(settings.Source, new ViewModelWriter(), settings.AppNamespace, settings.AppNamespace + ".ViewModels");
                }

                propertyNames = ModelTypeGenerator.LastPropertyNames.ToList();

                WriteFile(Path.Combine(settings.AppOutDir, $"Models/I{modelName}.cs"), model);
                WriteFile(Path.Combine(settings.AppOutDir, $"Database/{modelName}Entity.cs"), entity);
                WriteFile(Path.Combine(settings.AppOutDir, $"InputModels/{modelName}Input.cs"), inputModel);
                WriteFile(Path.Combine(settings.AppOutDir, $"InputModels/{modelName}Query.cs"), QueryModelWriter.Get(settings.AppNamespace, modelName));
                WriteFile(Path.Combine(settings.AppOutDir, $"ViewModels/{modelName}.cs"), viewModel);

                WriteFile(Path.Combine(settings.AppOutDir, $"Repository/{modelName}Repository.cs"), RepoGenerator.Get(settings.AppNamespace, modelName));
                WriteFile(Path.Combine(settings.AppOutDir, $"Repository/I{modelName}Repository.cs"), RepoInterfaceGenerator.Get(settings.AppNamespace, modelName));
                WriteFile(Path.Combine(settings.AppOutDir, $"Repository/{modelName}RepoConfig.cs"), RepoConfigGenerator.Get(settings.AppNamespace, modelName));
                WriteFile(Path.Combine(settings.AppOutDir, $"Controllers/Api/{modelName}sController.cs"), ControllerGenerator.Get(settings.AppNamespace, modelName));
                WriteFile(Path.Combine(settings.AppOutDir, $"Mappers/{modelName}Mapper.cs"), MappingProfileGenerator.Get(settings.AppNamespace, modelName));
                WriteFile(Path.Combine(settings.AppOutDir, $"Database/AppDbContext.{modelName}.cs"), AppDbContextGenerator.Get(settings.AppNamespace, modelName));
                WriteFile(Path.Combine(settings.AppOutDir, $"ViewModels/{modelName}Collection.cs"), ModelCollectionGenerator.Get(settings.AppNamespace, modelName));
                WriteFile(Path.Combine(settings.AppOutDir, $"ViewModels/EntryPoint.{modelName}.cs"), EntryPointGenerator.Get(settings.AppNamespace, modelName));

                WriteFile(Path.Combine(settings.AppOutDir, $"Views/{settings.UiController}/{modelName}s.cshtml"), CrudCshtmlInjectorGenerator.Get(modelName, propertyNames: propertyNames));
                WriteFile(Path.Combine(settings.AppOutDir, $"Client/Libs/{modelName}CrudInjector.ts"), CrudInjectorGenerator.Get(modelName));
                WriteFile(Path.Combine(settings.AppOutDir, $"Views/{settings.UiController}/{modelName}s.ts"), CrudUiTypescriptGenerator.Get(modelName));
                WriteFile(Path.Combine(settings.AppOutDir, $"Controllers/{settings.UiController}Controller.{modelName}s.cs"), UiControllerGenerator.Get(settings.AppNamespace, settings.UiController, modelName));
            }
        }

        private static bool HasWhitespace(String test)
        {
            foreach (var c in test)
            {
                if (Char.IsWhiteSpace(c))
                {
                    return true;
                }
            }
            return false;
        }

        private static void WriteFile(String file, String content)
        {
            var folder = Path.GetDirectoryName(file);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            using(var writer = new StreamWriter(File.Open(file, FileMode.Create, FileAccess.Write, FileShare.None)))
            {
                writer.Write(content);
            }
        }
    }
}