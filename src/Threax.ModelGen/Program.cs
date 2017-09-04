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
                ServiceNamespace = args[0],
                Source = args[1],
                ServiceOutDir = args[2],
                UiOutDir = args[3],
                UiNamespace = args[4]
            });
        }

        class GeneratorSettings
        {
            public String ServiceNamespace { get; set; }

            public String Source { get; set; }

            public String ServiceOutDir { get; set; }

            public String UiOutDir { get; set; }

            public String UiNamespace { get; set; }

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

            if (settings.ServiceOutDir != null && settings.ServiceNamespace != null)
            {

                if (schema != null)
                {
                    model = ModelTypeGenerator.Create(schema, new IdInterfaceWriter(), schema, settings.ServiceNamespace, settings.ServiceNamespace + ".Models");
                    entity = ModelTypeGenerator.Create(schema, new EntityWriter(), schema, settings.ServiceNamespace, settings.ServiceNamespace + ".Database");
                    inputModel = ModelTypeGenerator.Create(schema, new InputModelWriter(), schema, settings.ServiceNamespace, settings.ServiceNamespace + ".InputModels");
                    viewModel = ModelTypeGenerator.Create(schema, new ViewModelWriter(), schema, settings.ServiceNamespace, settings.ServiceNamespace + ".ViewModels");
                }
                else
                {
                    model = ModelTypeGenerator.Create(settings.Source, new IdInterfaceWriter(), settings.ServiceNamespace, settings.ServiceNamespace + ".Models");
                    entity = ModelTypeGenerator.Create(settings.Source, new EntityWriter(), settings.ServiceNamespace, settings.ServiceNamespace + ".Database");
                    inputModel = ModelTypeGenerator.Create(settings.Source, new InputModelWriter(), settings.ServiceNamespace, settings.ServiceNamespace + ".InputModels");
                    viewModel = ModelTypeGenerator.Create(settings.Source, new ViewModelWriter(), settings.ServiceNamespace, settings.ServiceNamespace + ".ViewModels");
                }

                propertyNames = ModelTypeGenerator.LastPropertyNames.ToList();

                WriteFile(Path.Combine(settings.ServiceOutDir, $"Models/I{modelName}.cs"), model);
                WriteFile(Path.Combine(settings.ServiceOutDir, $"Database/{modelName}Entity.cs"), entity);
                WriteFile(Path.Combine(settings.ServiceOutDir, $"InputModels/{modelName}Input.cs"), inputModel);
                WriteFile(Path.Combine(settings.ServiceOutDir, $"ViewModels/{modelName}.cs"), viewModel);

                WriteFile(Path.Combine(settings.ServiceOutDir, $"Repository/{modelName}Repository.cs"), RepoGenerator.Get(settings.ServiceNamespace, modelName));
                WriteFile(Path.Combine(settings.ServiceOutDir, $"Repository/I{modelName}Repository.cs"), RepoInterfaceGenerator.Get(settings.ServiceNamespace, modelName));
                WriteFile(Path.Combine(settings.ServiceOutDir, $"Repository/{modelName}RepoConfig.cs"), RepoConfigGenerator.Get(settings.ServiceNamespace, modelName));
                WriteFile(Path.Combine(settings.ServiceOutDir, $"Controllers/{modelName}sController.cs"), ControllerGenerator.Get(settings.ServiceNamespace, modelName));
                WriteFile(Path.Combine(settings.ServiceOutDir, $"Mappers/{modelName}Mapper.cs"), MappingProfileGenerator.Get(settings.ServiceNamespace, modelName));
                WriteFile(Path.Combine(settings.ServiceOutDir, $"Database/AppDbContext{modelName}.cs"), AppDbContextGenerator.Get(settings.ServiceNamespace, modelName));
                WriteFile(Path.Combine(settings.ServiceOutDir, $"ViewModels/{modelName}Collection.cs"), ModelCollectionGenerator.Get(settings.ServiceNamespace, modelName));
            }

            if(settings.UiNamespace != null && settings.UiOutDir != null)
            {
                WriteFile(Path.Combine(settings.UiOutDir, $"Views/{settings.UiController}/{modelName}s.cshtml"), CrudCshtmlInjectorGenerator.Get(modelName, propertyNames: propertyNames));
                WriteFile(Path.Combine(settings.UiOutDir, $"Client/Libs/{modelName}CrudInjector.ts"), CrudInjectorGenerator.Get(modelName));
                WriteFile(Path.Combine(settings.UiOutDir, $"Views/{settings.UiController}/{modelName}s.ts"), CrudUiTypescriptGenerator.Get(modelName));
                WriteFile(Path.Combine(settings.UiOutDir, $"Controllers/{settings.UiController}{modelName}s.cs"), UiControllerGenerator.Get(settings.UiNamespace, settings.UiController, modelName));
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