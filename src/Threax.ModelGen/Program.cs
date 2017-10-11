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
            try
            {
                if (args.Length != 3 && args.Length != 2)
                {
                    throw new MessageException(@"You must provide the following arguments [] is required {} is optional. 
To create a model pass:
[Schema File Path] [App Namespace] {Output Directory}.
To remove a model pass:
remove [Schema File Path] {Output Directory}");
                }

                if (args[0] == "remove")
                {
                    var settings = new GeneratorSettings()
                    {
                        Source = args[1],
                        AppNamespace = "Doesn't Matter",
                        AppOutDir = args.Length > 2 ? args[2] : Directory.GetCurrentDirectory(),
                    };
                    settings.Configure();
                    DeleteClasses(settings);
                }
                else
                {
                    var settings = new GeneratorSettings()
                    {
                        Source = args[0],
                        AppNamespace = args[1],
                        AppOutDir = args.Length > 2 ? args[2] : Directory.GetCurrentDirectory(),
                    };
                    settings.Configure();
                    GenerateClasses(settings);
                }
            }
            catch(MessageException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"A {ex.GetType().Name} occured. Message: {ex.Message}");
            }
        }

        class GeneratorSettings
        {
            public void Configure()
            {
                if (!File.Exists(Source))
                {
                    throw new MessageException($"Cannot find schema file {Source}.");
                }

                var schemaTask = JsonSchema4.FromFileAsync(Source);
                schemaTask.Wait();
                Schema = schemaTask.Result;
                if (Schema.ExtensionData == null) //Make sure this exists
                {
                    Schema.ExtensionData = new Dictionary<String, Object>();
                }

                ModelName = Schema.Title;
                Object pluralTitleObj;
                if (Schema.ExtensionData.TryGetValue("x-plural-title", out pluralTitleObj))
                {
                    PluralModelName = pluralTitleObj.ToString();
                }
                else
                {
                    PluralModelName = ModelName + "s";
                }
            }

            public String AppNamespace { get; set; }

            public String Source { get; set; }

            public String AppOutDir { get; set; }

            public string UiController { get; set; } = "Home";

            public String ModelName { get; set; }

            public String PluralModelName { get; set; }

            public JsonSchema4 Schema { get; set; }
        }

        private static void GenerateClasses(GeneratorSettings settings)
        {
            String modelInterface, entity, inputModel, viewModel;

            List<String> propertyNames = null;

            if (settings.AppOutDir != null && settings.AppNamespace != null)
            {
                if (settings.Schema != null)
                {
                    modelInterface = ModelTypeGenerator.Create(settings.Schema, settings.PluralModelName, new IdInterfaceWriter(), settings.Schema, settings.AppNamespace, settings.AppNamespace + ".Models");
                    entity = ModelTypeGenerator.Create(settings.Schema, settings.PluralModelName, new EntityWriter(), settings.Schema, settings.AppNamespace, settings.AppNamespace + ".Database");
                    inputModel = ModelTypeGenerator.Create(settings.Schema, settings.PluralModelName, new InputModelWriter(), settings.Schema, settings.AppNamespace, settings.AppNamespace + ".InputModels");
                    viewModel = ModelTypeGenerator.Create(settings.Schema, settings.PluralModelName, new ViewModelWriter(), settings.Schema, settings.AppNamespace, settings.AppNamespace + ".ViewModels");
                }
                else
                {
                    modelInterface = ModelTypeGenerator.Create(settings.Source, settings.PluralModelName, new IdInterfaceWriter(), settings.AppNamespace, settings.AppNamespace + ".Models");
                    entity = ModelTypeGenerator.Create(settings.Source, settings.PluralModelName, new EntityWriter(), settings.AppNamespace, settings.AppNamespace + ".Database");
                    inputModel = ModelTypeGenerator.Create(settings.Source, settings.PluralModelName, new InputModelWriter(), settings.AppNamespace, settings.AppNamespace + ".InputModels");
                    viewModel = ModelTypeGenerator.Create(settings.Source, settings.PluralModelName, new ViewModelWriter(), settings.AppNamespace, settings.AppNamespace + ".ViewModels");
                }

                propertyNames = ModelTypeGenerator.LastPropertyNames.ToList();

                WriteFile(Path.Combine(settings.AppOutDir, $"Models/I{settings.ModelName}.cs"), modelInterface);
                WriteFile(Path.Combine(settings.AppOutDir, $"Database/{settings.ModelName}Entity.cs"), entity);
                WriteFile(Path.Combine(settings.AppOutDir, $"InputModels/{settings.ModelName}Input.cs"), inputModel);
                WriteFile(Path.Combine(settings.AppOutDir, $"InputModels/{settings.ModelName}Query.cs"), QueryModelWriter.Get(settings.AppNamespace, settings.ModelName, settings.PluralModelName));
                WriteFile(Path.Combine(settings.AppOutDir, $"ViewModels/{settings.ModelName}.cs"), viewModel);

                WriteFile(Path.Combine(settings.AppOutDir, $"Repository/{settings.ModelName}Repository.cs"), RepoGenerator.Get(settings.AppNamespace, settings.ModelName, settings.PluralModelName));
                WriteFile(Path.Combine(settings.AppOutDir, $"Repository/I{settings.ModelName}Repository.cs"), RepoInterfaceGenerator.Get(settings.AppNamespace, settings.ModelName, settings.PluralModelName));
                WriteFile(Path.Combine(settings.AppOutDir, $"Repository/{settings.ModelName}RepoConfig.cs"), RepoConfigGenerator.Get(settings.AppNamespace, settings.ModelName));
                WriteFile(Path.Combine(settings.AppOutDir, $"Controllers/Api/{settings.PluralModelName}Controller.cs"), ControllerGenerator.Get(settings.AppNamespace, settings.ModelName, settings.PluralModelName));
                WriteFile(Path.Combine(settings.AppOutDir, $"Mappers/{settings.ModelName}Profile.cs"), MappingProfileGenerator.Get(settings.AppNamespace, settings.ModelName));
                WriteFile(Path.Combine(settings.AppOutDir, $"Database/AppDbContext.{settings.ModelName}.cs"), AppDbContextGenerator.Get(settings.AppNamespace, settings.ModelName, settings.PluralModelName));
                WriteFile(Path.Combine(settings.AppOutDir, $"ViewModels/{settings.ModelName}Collection.cs"), ModelCollectionGenerator.Get(settings.AppNamespace, settings.ModelName, settings.PluralModelName));
                WriteFile(Path.Combine(settings.AppOutDir, $"ViewModels/EntryPoint.{settings.ModelName}.cs"), EntryPointGenerator.Get(settings.AppNamespace, settings.ModelName, settings.PluralModelName));

                WriteFile(Path.Combine(settings.AppOutDir, $"Views/{settings.UiController}/{settings.PluralModelName}.cshtml"), CrudCshtmlInjectorGenerator.Get(settings.ModelName, settings.PluralModelName, propertyNames: propertyNames));
                WriteFile(Path.Combine(settings.AppOutDir, $"Client/Libs/{settings.ModelName}CrudInjector.ts"), CrudInjectorGenerator.Get(settings.ModelName, settings.PluralModelName));
                WriteFile(Path.Combine(settings.AppOutDir, $"Views/{settings.UiController}/{settings.PluralModelName}.ts"), CrudUiTypescriptGenerator.Get(settings.ModelName));
                WriteFile(Path.Combine(settings.AppOutDir, $"Controllers/{settings.UiController}Controller.{settings.PluralModelName}.cs"), UiControllerGenerator.Get(settings.AppNamespace, settings.UiController, settings.ModelName, settings.PluralModelName));
            }
        }

        private static void DeleteClasses(GeneratorSettings settings)
        {
            DeleteFile(Path.Combine(settings.AppOutDir, $"Models/I{settings.ModelName}.cs"));
            DeleteFile(Path.Combine(settings.AppOutDir, $"Database/{settings.ModelName}Entity.cs"));
            DeleteFile(Path.Combine(settings.AppOutDir, $"InputModels/{settings.ModelName}Input.cs"));
            DeleteFile(Path.Combine(settings.AppOutDir, $"InputModels/{settings.ModelName}Query.cs"));
            DeleteFile(Path.Combine(settings.AppOutDir, $"ViewModels/{settings.ModelName}.cs"));

            DeleteFile(Path.Combine(settings.AppOutDir, $"Repository/{settings.ModelName}Repository.cs"));
            DeleteFile(Path.Combine(settings.AppOutDir, $"Repository/I{settings.ModelName}Repository.cs"));
            DeleteFile(Path.Combine(settings.AppOutDir, $"Repository/{settings.ModelName}RepoConfig.cs"));
            DeleteFile(Path.Combine(settings.AppOutDir, $"Controllers/Api/{settings.PluralModelName}Controller.cs"));
            DeleteFile(Path.Combine(settings.AppOutDir, $"Mappers/{settings.ModelName}Profile.cs"));
            DeleteFile(Path.Combine(settings.AppOutDir, $"Database/AppDbContext.{settings.ModelName}.cs"));
            DeleteFile(Path.Combine(settings.AppOutDir, $"ViewModels/{settings.ModelName}Collection.cs"));
            DeleteFile(Path.Combine(settings.AppOutDir, $"ViewModels/EntryPoint.{settings.ModelName}.cs"));

            DeleteFile(Path.Combine(settings.AppOutDir, $"Views/{settings.UiController}/{settings.PluralModelName}.cshtml"));
            DeleteFile(Path.Combine(settings.AppOutDir, $"Client/Libs/{settings.ModelName}CrudInjector.ts"));
            DeleteFile(Path.Combine(settings.AppOutDir, $"Views/{settings.UiController}/{settings.PluralModelName}.ts"));
            DeleteFile(Path.Combine(settings.AppOutDir, $"Controllers/{settings.UiController}Controller.{settings.PluralModelName}.cs"));
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

        private static void DeleteFile(String file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
    }
}