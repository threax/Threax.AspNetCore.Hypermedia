using Microsoft.Extensions.Configuration;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Threax.AspNetCore.Models;
using Threax.ModelGen.TestGenerators;

namespace Threax.ModelGen
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 2)
                {
                    throw new MessageException(@"You must provide the following arguments [] is required {} is optional. 
To create a model pass:
[Schema File Path] [App Namespace] {--AppOutDir OutputDirectory} {--TestOutDir TestDirectory}.
To remove a model pass:
remove [Schema File Path] {--AppOutDir OutputDirectory} {--TestOutDir TestDirectory}");
                }

                var config = new ConfigurationBuilder().AddCommandLine(args.Skip(2).ToArray()).Build();
                var appDir = Directory.GetCurrentDirectory();
                var directoryName = Path.GetFileName(appDir);
                var testDir = Path.GetFullPath(Path.Combine(appDir, $"../{directoryName}.Tests"));

                if (args[0] == "remove")
                {
                    var settings = new GeneratorSettings()
                    {
                        Source = args[1],
                        AppNamespace = "Doesn't Matter",
                        AppOutDir = appDir,
                        TestOutDir = testDir
                    };
                    config.Bind(settings);
                    settings.Configure();
                    DeleteClasses(settings);
                }
                else
                {
                    var settings = new GeneratorSettings()
                    {
                        Source = args[0],
                        AppNamespace = args[1],
                        AppOutDir = appDir,
                        TestOutDir = testDir
                    };
                    config.Bind(settings);
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

        private static void GenerateClasses(GeneratorSettings settings)
        {
            if (settings.AppOutDir != null && settings.AppNamespace != null)
            {
                if (settings.WriteApp)
                {
                    WriteFile(Path.Combine(settings.AppOutDir, $"Models/I{settings.ModelName}.cs"), PartialModelInterfaceGenerator.GetUserPartial(settings.ModelName, settings.AppNamespace + ".Models"), false);
                    WriteFile(Path.Combine(settings.AppOutDir, $"Models/I{settings.ModelName}.Generated.cs"), IdInterfaceWriter.Create(settings.Schema, settings.AppNamespace), true);
                    WriteFile(Path.Combine(settings.AppOutDir, $"Database/{settings.ModelName}Entity.cs"), PartialTypeGenerator.GetUserPartial(settings.ModelName, settings.AppNamespace + ".Database", "Entity"), false);
                    WriteFile(Path.Combine(settings.AppOutDir, $"Database/{settings.ModelName}Entity.Generated.cs"), EntityWriter.Create(settings.Schema, settings.AppNamespace), true);
                    WriteFile(Path.Combine(settings.AppOutDir, $"InputModels/{settings.ModelName}Input.cs"), PartialTypeGenerator.GetUserPartial(settings.ModelName, settings.AppNamespace + ".InputModels", "Input"), false);
                    WriteFile(Path.Combine(settings.AppOutDir, $"InputModels/{settings.ModelName}Input.Generated.cs"), InputModelWriter.Create(settings.Schema, settings.AppNamespace), true);
                    WriteFile(Path.Combine(settings.AppOutDir, $"InputModels/{settings.ModelName}Query.cs"), PartialTypeGenerator.GetUserPartial(settings.ModelName, settings.AppNamespace + ".InputModels", "Query"), false);
                    WriteFile(Path.Combine(settings.AppOutDir, $"InputModels/{settings.ModelName}Query.Generated.cs"), QueryModelWriter.Get(settings.AppNamespace, settings.ModelName, settings.PluralModelName, settings.Schema), true);
                    WriteFile(Path.Combine(settings.AppOutDir, $"ViewModels/{settings.ModelName}.cs"), ViewModelWriter.GetUserPartial(settings.AppNamespace, settings.ModelName, settings.PluralModelName), false);
                    WriteFile(Path.Combine(settings.AppOutDir, $"ViewModels/{settings.ModelName}.Generated.cs"), ViewModelWriter.Create(settings.Schema, settings.AppNamespace), true);

                    WriteFile(Path.Combine(settings.AppOutDir, $"Repository/{settings.ModelName}Repository.cs"), RepoGenerator.Get(settings.Schema, settings.AppNamespace), settings.ForceWriteApi);
                    WriteFile(Path.Combine(settings.AppOutDir, $"Repository/I{settings.ModelName}Repository.cs"), RepoInterfaceGenerator.Get(settings.Schema, settings.AppNamespace), settings.ForceWriteApi);
                    WriteFile(Path.Combine(settings.AppOutDir, $"Repository/{settings.ModelName}Repository.Config.cs"), RepoConfigGenerator.Get(settings.AppNamespace, settings.ModelName), settings.ForceWriteApi);
                    WriteFile(Path.Combine(settings.AppOutDir, $"Controllers/Api/{settings.PluralModelName}Controller.cs"), ControllerGenerator.Get(settings.AppNamespace, settings.ModelName, settings.PluralModelName, settings.Schema), settings.ForceWriteApi);
                    WriteFile(Path.Combine(settings.AppOutDir, $"Mappers/{settings.ModelName}Profile.cs"), MappingProfileGenerator.Get(settings.AppNamespace, settings.ModelName, settings.Schema.AllowCreated(), settings.Schema.AllowModified()), settings.ForceWriteApi);
                    WriteFile(Path.Combine(settings.AppOutDir, $"Mappers/{settings.ModelName}Profile.Generated.cs"), MappingProfileGenerator.GetGenerated(settings.AppNamespace, settings.ModelName, settings.Schema.AllowCreated(), settings.Schema.AllowModified()), true);
                    WriteFile(Path.Combine(settings.AppOutDir, $"Database/AppDbContext.{settings.ModelName}.cs"), AppDbContextGenerator.Get(settings.AppNamespace, settings.ModelName, settings.PluralModelName), settings.ForceWriteApi);
                    WriteFile(Path.Combine(settings.AppOutDir, $"ViewModels/{settings.ModelName}Collection.cs"), ModelCollectionGenerator.GetUserPartial(settings.AppNamespace, settings.ModelName, settings.PluralModelName), false);
                    WriteFile(Path.Combine(settings.AppOutDir, $"ViewModels/{settings.ModelName}Collection.Generated.cs"), ModelCollectionGenerator.Get(settings.AppNamespace, settings.ModelName, settings.PluralModelName, settings.Schema), true);
                    WriteFile(Path.Combine(settings.AppOutDir, $"ViewModels/EntryPoint.{settings.ModelName}.cs"), EntryPointGenerator.Get(settings.AppNamespace, settings.ModelName, settings.PluralModelName), settings.ForceWriteApi);

                    var propertyNames = settings.Schema.Properties.Values.Select(i => NameGenerator.CreatePascal(i.Name));
                    WriteFile(Path.Combine(settings.AppOutDir, $"Views/{settings.UiController}/{settings.PluralModelName}.cshtml"), CrudCshtmlInjectorGenerator.Get(settings.ModelName, settings.PluralModelName, propertyNames: propertyNames), settings.ForceWriteUi);
                    WriteFile(Path.Combine(settings.AppOutDir, $"Client/Libs/{settings.ModelName}CrudInjector.ts"), CrudInjectorGenerator.Get(settings.ModelName, settings.PluralModelName), settings.ForceWriteUi);
                    WriteFile(Path.Combine(settings.AppOutDir, $"Views/{settings.UiController}/{settings.PluralModelName}.ts"), CrudUiTypescriptGenerator.Get(settings.ModelName), settings.ForceWriteUi);
                    WriteFile(Path.Combine(settings.AppOutDir, $"Controllers/{settings.UiController}Controller.{settings.PluralModelName}.cs"), UiControllerGenerator.Get(settings.AppNamespace, settings.UiController, settings.ModelName, settings.PluralModelName, settings.Schema), settings.ForceWriteUi);
                }

                if (settings.WriteTests)
                {
                    WriteFile(Path.Combine(settings.TestOutDir, $"{settings.ModelName}/{settings.ModelName}Tests.cs"), ModelTestWrapper.Get(settings.AppNamespace, settings.ModelName, settings.PluralModelName, settings.Schema), settings.ForceWriteTests);
                    WriteFile(Path.Combine(settings.TestOutDir, $"{settings.ModelName}/Controller.cs"), ControllerTests.Get(settings.AppNamespace, settings.ModelName, settings.PluralModelName), settings.ForceWriteTests);
                    WriteFile(Path.Combine(settings.TestOutDir, $"{settings.ModelName}/Profile.cs"), ProfileTests.Get(settings.Schema, settings.AppNamespace), settings.ForceWriteTests);
                    WriteFile(Path.Combine(settings.TestOutDir, $"{settings.ModelName}/Repository.cs"), RepositoryTests.Get(settings.AppNamespace, settings.ModelName, settings.PluralModelName), settings.ForceWriteTests);
                }
            }
        }

        private static void DeleteClasses(GeneratorSettings settings)
        {
            if (settings.WriteApp)
            {
                DeleteFile(Path.Combine(settings.AppOutDir, $"Models/I{settings.ModelName}.cs"));
                DeleteFile(Path.Combine(settings.AppOutDir, $"Models/I{settings.ModelName}.Generated.cs"));
                DeleteFile(Path.Combine(settings.AppOutDir, $"Database/{settings.ModelName}Entity.cs"));
                DeleteFile(Path.Combine(settings.AppOutDir, $"Database/{settings.ModelName}Entity.Generated.cs"));
                DeleteFile(Path.Combine(settings.AppOutDir, $"InputModels/{settings.ModelName}Input.cs"));
                DeleteFile(Path.Combine(settings.AppOutDir, $"InputModels/{settings.ModelName}Input.Generated.cs"));
                DeleteFile(Path.Combine(settings.AppOutDir, $"InputModels/{settings.ModelName}Query.cs"));
                DeleteFile(Path.Combine(settings.AppOutDir, $"InputModels/{settings.ModelName}Query.Generated.cs"));
                DeleteFile(Path.Combine(settings.AppOutDir, $"ViewModels/{settings.ModelName}.cs"));
                DeleteFile(Path.Combine(settings.AppOutDir, $"ViewModels/{settings.ModelName}.Generated.cs"));

                DeleteFile(Path.Combine(settings.AppOutDir, $"Repository/{settings.ModelName}Repository.cs"));
                DeleteFile(Path.Combine(settings.AppOutDir, $"Repository/I{settings.ModelName}Repository.cs"));
                DeleteFile(Path.Combine(settings.AppOutDir, $"Repository/{settings.ModelName}Repository.Config.cs"));
                DeleteFile(Path.Combine(settings.AppOutDir, $"Controllers/Api/{settings.PluralModelName}Controller.cs"));
                DeleteFile(Path.Combine(settings.AppOutDir, $"Mappers/{settings.ModelName}Profile.cs"));
                DeleteFile(Path.Combine(settings.AppOutDir, $"Database/AppDbContext.{settings.ModelName}.cs"));
                DeleteFile(Path.Combine(settings.AppOutDir, $"ViewModels/{settings.ModelName}Collection.cs"));
                DeleteFile(Path.Combine(settings.AppOutDir, $"ViewModels/{settings.ModelName}Collection.Generated.cs"));
                DeleteFile(Path.Combine(settings.AppOutDir, $"ViewModels/EntryPoint.{settings.ModelName}.cs"));

                DeleteFile(Path.Combine(settings.AppOutDir, $"Views/{settings.UiController}/{settings.PluralModelName}.cshtml"));
                DeleteFile(Path.Combine(settings.AppOutDir, $"Client/Libs/{settings.ModelName}CrudInjector.ts"));
                DeleteFile(Path.Combine(settings.AppOutDir, $"Views/{settings.UiController}/{settings.PluralModelName}.ts"));
                DeleteFile(Path.Combine(settings.AppOutDir, $"Controllers/{settings.UiController}Controller.{settings.PluralModelName}.cs"));
            }

            if (settings.WriteTests)
            {
                DeleteFile(Path.Combine(settings.TestOutDir, $"{settings.ModelName}/{settings.ModelName}Tests.cs"));
                DeleteFile(Path.Combine(settings.TestOutDir, $"{settings.ModelName}/Controller.cs"));
                DeleteFile(Path.Combine(settings.TestOutDir, $"{settings.ModelName}/Profile.cs"));
                DeleteFile(Path.Combine(settings.TestOutDir, $"{settings.ModelName}/Repository.cs"));
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

        private static void WriteFile(String file, String content, bool force)
        {
            var folder = Path.GetDirectoryName(file);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            if(force || !File.Exists(file))
            {
                using (var writer = new StreamWriter(File.Open(file, FileMode.Create, FileAccess.Write, FileShare.None)))
                {
                    writer.Write(content);
                }
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