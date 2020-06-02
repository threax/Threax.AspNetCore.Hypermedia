using Microsoft.Extensions.Configuration;
using Threax.NJsonSchema;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Threax.AspNetCore.Models;
using Threax.ModelGen.TestGenerators;

namespace Threax.ModelGen
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () => await AsyncMain(args)).GetAwaiter().GetResult();
        }

        static async Task AsyncMain(string[] args)
        {
            try
            {
                if (args.Length < 1)
                {
                    throw new MessageException($@"threax-modelgen version: {typeof(Program).Assembly.GetName().Version.ToString()}
You must provide the following arguments [] is required {{}} is optional. 
To create a model pass:
[Schema File Path] {{--AppOutDir OutputDirectory}} {{--TestOutDir TestDirectory}} {{--AppNamespace Your.Namespace}}.
To remove a model pass:
remove [Schema File Path] {{--AppOutDir OutputDirectory}} {{--TestOutDir TestDirectory}}");
                }

                var appDir = Directory.GetCurrentDirectory();
                var directoryName = Path.GetFileName(appDir);
                var testDir = Path.GetFullPath(Path.Combine(appDir, $"../{directoryName}.Tests"));

                try
                {
                    if (args[0] == "remove")
                    {
                        if (args.Length < 2)
                        {
                            throw new MessageException("You must provide the model schema after the remove argument to remove a model");
                        }

                        var settings = new GeneratorSettings()
                        {
                            Source = args[1],
                            AppOutDir = appDir,
                            TestOutDir = testDir
                        };
                        var config = new ConfigurationBuilder().AddCommandLine(args.Skip(2).ToArray()).Build();
                        config.Bind(settings);
                        await settings.Configure();
                        DeleteClasses(settings);
                    }
                    else
                    {
                        var settings = new GeneratorSettings()
                        {
                            Source = args[0],
                            AppOutDir = appDir,
                            TestOutDir = testDir
                        };
                        var config = new ConfigurationBuilder().AddCommandLine(args.Skip(1).ToArray()).Build();
                        config.Bind(settings);
                        await settings.Configure();
                        await GenerateClasses(settings);
                    }
                }
                catch (RunOnFullFrameworkException ex)
                {
#if NETCOREAPP2_0
                    RunFullFramework(args, ex);
#else
                    throw;
#endif
                }
            }
            catch (MessageException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"A {ex.GetType().Name} occured. Message: {ex.Message}");
            }
        }

        private static void RunFullFramework(string[] args, RunOnFullFrameworkException ex)
        {
            var fullExePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "../net471/dotnet-threax-modelgen.exe"));
            if (!File.Exists(fullExePath))
            {
                throw new InvalidOperationException($"Cannot find full .net framework executable at {fullExePath}", ex);
            }
            Console.WriteLine($"Running full .net framework version from {fullExePath}");
            var argsBuilder = new StringBuilder();
            foreach (var arg in args)
            {
                argsBuilder.Append(arg);
                argsBuilder.Append(" ");
            }
            var process = new Process();
            process.StartInfo.Arguments = argsBuilder.ToString();
            process.StartInfo.FileName = fullExePath;
            process.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.OutputDataReceived += (s, o) => Console.WriteLine(o.Data);
            process.ErrorDataReceived += (s, o) => Console.WriteLine(o.Data);
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
        }

        private static async Task GenerateClasses(GeneratorSettings settings)
        {
            if (settings.AppOutDir != null && settings.AppNamespace != null)
            {
                if (settings.WriteApp)
                {
                    //Entity
                    if (settings.Schema.CreateEntity())
                    {
                        if (settings.CreateGeneratedFiles)
                        {
                            WriteFile(settings.AppOutDir, EntityWriter.GetFileName(settings.Schema, false), PartialTypeGenerator.GetEntity(settings.Schema, settings.AppNamespace), false);
                        }
                        else
                        {
                            DeleteFile(settings.AppOutDir, EntityWriter.GetFileName(settings.Schema, true));
                        }
                        WriteFile(settings.AppOutDir, AppDbContextGenerator.GetFileName(settings.Schema), AppDbContextGenerator.Get(settings.Schema, settings.AppNamespace), settings.ForceWriteApi);
                        WriteFile(settings.AppOutDir, EntityWriter.GetFileName(settings.Schema, settings.CreateGeneratedFiles), EntityWriter.Create(settings.Schema, settings.OtherSchemas, settings.AppNamespace), true);

                        foreach (var relationship in settings.Schema.GetRelationshipSettings())
                        {
                            if (relationship.Kind == RelationKind.ManyToMany)
                            {
                                if (settings.CreateGeneratedFiles)
                                {
                                    WriteFile(settings.AppOutDir, JoinEntityWriter.GetFileName(relationship, false), PartialTypeGenerator.GetJoinEntity(settings.Schema, relationship, settings.AppNamespace), false);
                                }
                                else
                                {
                                    DeleteFile(settings.AppOutDir, JoinEntityWriter.GetFileName(relationship, true));
                                }
                                WriteFile(settings.AppOutDir, AppDbContextGenerator.GetManyToManyEntityDbContextFileName(relationship), AppDbContextGenerator.GetManyToManyEntityDbContext(relationship, settings.AppNamespace), settings.ForceWriteApi);
                                WriteFile(settings.AppOutDir, JoinEntityWriter.GetFileName(relationship, settings.CreateGeneratedFiles), JoinEntityWriter.Get(settings.Schema, settings.OtherSchemas, relationship, settings.AppNamespace), true);
                            }
                        }
                    }

                    //Input Model
                    if (settings.Schema.CreateInputModel())
                    {
                        if (settings.CreateGeneratedFiles)
                        {
                            WriteFile(settings.AppOutDir, InputModelWriter.GetFileName(settings.Schema, false), PartialTypeGenerator.GetInput(settings.Schema, settings.AppNamespace), false);
                        }
                        else
                        {
                            DeleteFile(settings.AppOutDir, InputModelWriter.GetFileName(settings.Schema, true));
                        }
                        WriteFile(settings.AppOutDir, InputModelWriter.GetFileName(settings.Schema, settings.CreateGeneratedFiles), await InputModelWriter.Create(settings.Schema, settings.OtherSchemas, settings.AppNamespace), true);
                    }

                    //Query Model
                    if (settings.Schema.CreateQuery())
                    {
                        if (settings.CreateGeneratedFiles)
                        {
                            WriteFile(settings.AppOutDir, QueryModelWriter.GetFileName(settings.Schema, false), QueryUserPartialGenerator.GetQuery(settings.Schema, settings.AppNamespace), false);
                        }
                        else
                        {
                            DeleteFile(settings.AppOutDir, QueryModelWriter.GetFileName(settings.Schema, true));
                        }
                        WriteFile(settings.AppOutDir, QueryModelWriter.GetFileName(settings.Schema, settings.CreateGeneratedFiles), QueryModelWriter.Get(settings.Schema, settings.AppNamespace, settings.CreateGeneratedFiles), true);
                    }

                    //View Model
                    if (settings.Schema.CreateViewModel())
                    {
                        if (settings.CreateGeneratedFiles)
                        {
                            WriteFile(settings.AppOutDir, ViewModelWriter.GetFileName(settings.Schema, false), ViewModelWriter.GetUserPartial(settings.Schema, settings.AppNamespace), false);
                        }
                        else
                        {
                            DeleteFile(settings.AppOutDir, ViewModelWriter.GetFileName(settings.Schema, true));
                        }
                        WriteFile(settings.AppOutDir, ViewModelWriter.GetFileName(settings.Schema, settings.CreateGeneratedFiles), await ViewModelWriter.Create(settings.Schema, settings.OtherSchemas, settings.AppNamespace, settings.CreateGeneratedFiles), true);
                    }

                    //Repository
                    if (settings.Schema.CreateRepository())
                    {
                        WriteFile(settings.AppOutDir, RepoGenerator.GetFileName(settings.Schema), RepoGenerator.Get(settings.Schema, settings.AppNamespace), settings.ForceWriteApi);
                        WriteFile(settings.AppOutDir, RepoInterfaceGenerator.GetFileName(settings.Schema), RepoInterfaceGenerator.Get(settings.Schema, settings.AppNamespace), settings.ForceWriteApi);
                        WriteFile(settings.AppOutDir, RepoConfigGenerator.GetFileName(settings.Schema), RepoConfigGenerator.Get(settings.Schema, settings.AppNamespace), settings.ForceWriteApi);
                    }

                    //Controller
                    if (settings.Schema.CreateController())
                    {
                        WriteFile(settings.AppOutDir, ControllerGenerator.GetFileName(settings.Schema), ControllerGenerator.Get(settings.Schema, settings.AppNamespace), settings.ForceWriteApi);
                        WriteFile(settings.AppOutDir, EntryPointGenerator.GetFileName(settings.Schema), EntryPointGenerator.Get(settings.Schema, settings.AppNamespace), settings.ForceWriteApi);
                    }

                    //Mapping Profile
                    if (settings.Schema.CreateMappingProfile())
                    {
                        if (settings.CreateGeneratedFiles) //This if is more complicated because the generator is backward compared to the others
                        {
                            WriteFile(settings.AppOutDir, MappingProfileGenerator.GetFileName(settings.Schema, false), MappingProfileGenerator.Get(settings.Schema, settings.AppNamespace, true), settings.ForceWriteApi);
                            WriteFile(settings.AppOutDir, MappingProfileGenerator.GetFileName(settings.Schema, true), MappingProfileGenerator.GetGenerated(settings.Schema, settings.AppNamespace), true);
                        }
                        else
                        {
                            WriteFile(settings.AppOutDir, MappingProfileGenerator.GetFileName(settings.Schema, false), MappingProfileGenerator.Get(settings.Schema, settings.AppNamespace, false), true);
                            DeleteFile(settings.AppOutDir, MappingProfileGenerator.GetFileName(settings.Schema, true));
                        }
                    }

                    //Model Collection
                    if (settings.Schema.CreateModelCollection())
                    {
                        if (settings.CreateGeneratedFiles)
                        {
                            WriteFile(settings.AppOutDir, ModelCollectionGenerator.GetFileName(settings.Schema, false), ModelCollectionGenerator.GetUserPartial(settings.Schema, settings.AppNamespace), false);
                        }
                        else
                        {
                            DeleteFile(settings.AppOutDir, ModelCollectionGenerator.GetFileName(settings.Schema, true));
                        }
                        WriteFile(settings.AppOutDir, ModelCollectionGenerator.GetFileName(settings.Schema, settings.CreateGeneratedFiles), ModelCollectionGenerator.Get(settings.Schema, settings.AppNamespace, settings.CreateGeneratedFiles), true);
                    }

                    //Ui
                    if (settings.Schema.CreateUi())
                    {
                        var propertyNames = settings.Schema.Properties.Values.Where(i => i.CreateViewModel()).Select(i => NameGenerator.CreatePascal(i.Name));
                        WriteFile(settings.AppOutDir, CrudCshtmlInjectorGenerator.GetFileName(settings.Schema), CrudCshtmlInjectorGenerator.Get(settings.Schema, propertyNames: propertyNames), settings.ForceWriteUi);
                        WriteFile(settings.AppOutDir, CrudInjectorGenerator.GetFileName(settings.Schema), CrudInjectorGenerator.Get(settings.Schema), settings.ForceWriteUi);
                        WriteFile(settings.AppOutDir, CrudUiTypescriptGenerator.GetFileName(settings.Schema), CrudUiTypescriptGenerator.Get(settings.ModelName), settings.ForceWriteUi);
                        WriteFile(settings.AppOutDir, UiControllerGenerator.GetFileName(settings.Schema), UiControllerGenerator.Get(settings.Schema, settings.AppNamespace), true);
                    }
                }

                if (settings.WriteTests)
                {
                    WriteFile(settings.TestOutDir, ModelTestWrapper.GetFileName(settings.Schema), ModelTestWrapper.Get(settings.Schema, settings.AppNamespace, settings.CreateGeneratedFiles), settings.ForceWriteTests);
                    if (settings.CreateGeneratedFiles)
                    {
                        WriteFile(settings.TestOutDir, ModelTestWrapperGenerated.GetFileName(settings.Schema), ModelTestWrapperGenerated.Get(settings.Schema, settings.AppNamespace), true);
                    }

                    if (settings.Schema.CreateController())
                    {
                        if (!File.Exists(Path.Combine(settings.TestOutDir, $"{settings.ModelName}/Controller.cs"))) //Legacy File check
                        {
                            WriteFile(settings.TestOutDir, ControllerTests.GetFileName(settings.Schema), ControllerTests.Get(settings.Schema, settings.AppNamespace), settings.ForceWriteTests);
                        }
                    }

                    if (settings.Schema.CreateMappingProfile())
                    {
                        if (!File.Exists(Path.Combine(settings.TestOutDir, $"{settings.ModelName}/Profile.cs"))) //Legacy File check
                        {
                            WriteFile(settings.TestOutDir, ProfileTests.GetFileName(settings.Schema), ProfileTests.Get(settings.Schema, settings.AppNamespace), settings.ForceWriteTests);
                        }
                    }

                    if (settings.Schema.CreateRepository())
                    {
                        if (!File.Exists(Path.Combine(settings.TestOutDir, $"{settings.ModelName}/Repository.cs"))) //Legacy File check
                        {
                            WriteFile(settings.TestOutDir, RepositoryTests.GetFileName(settings.Schema), RepositoryTests.Get(settings.Schema, settings.AppNamespace), settings.ForceWriteTests);
                        }
                    }
                }
            }
        }

        private static void DeleteClasses(GeneratorSettings settings)
        {
            if (settings.WriteApp)
            {
                DeleteFile(settings.AppOutDir, EntityWriter.GetFileName(settings.Schema, false));
                DeleteFile(settings.AppOutDir, EntityWriter.GetFileName(settings.Schema, true));
                foreach (var relationship in settings.Schema.GetRelationshipSettings())
                {
                    DeleteFile(settings.AppOutDir, JoinEntityWriter.GetFileName(relationship, false));
                    DeleteFile(settings.AppOutDir, JoinEntityWriter.GetFileName(relationship, true));
                    DeleteFile(settings.AppOutDir, AppDbContextGenerator.GetManyToManyEntityDbContextFileName(relationship));
                }
                DeleteFile(settings.AppOutDir, InputModelWriter.GetFileName(settings.Schema, false));
                DeleteFile(settings.AppOutDir, InputModelWriter.GetFileName(settings.Schema, true));
                DeleteFile(settings.AppOutDir, QueryModelWriter.GetFileName(settings.Schema, false));
                DeleteFile(settings.AppOutDir, QueryModelWriter.GetFileName(settings.Schema, true));
                DeleteFile(settings.AppOutDir, ViewModelWriter.GetFileName(settings.Schema, false));
                DeleteFile(settings.AppOutDir, ViewModelWriter.GetFileName(settings.Schema, true));

                DeleteFile(settings.AppOutDir, RepoGenerator.GetFileName(settings.Schema));
                DeleteFile(settings.AppOutDir, RepoInterfaceGenerator.GetFileName(settings.Schema));
                DeleteFile(settings.AppOutDir, RepoConfigGenerator.GetFileName(settings.Schema));
                DeleteFile(settings.AppOutDir, ControllerGenerator.GetFileName(settings.Schema));
                DeleteFile(settings.AppOutDir, MappingProfileGenerator.GetFileName(settings.Schema, false));
                DeleteFile(settings.AppOutDir, MappingProfileGenerator.GetFileName(settings.Schema, true));
                DeleteFile(settings.AppOutDir, AppDbContextGenerator.GetFileName(settings.Schema));
                DeleteFile(settings.AppOutDir, ModelCollectionGenerator.GetFileName(settings.Schema, false));
                DeleteFile(settings.AppOutDir, ModelCollectionGenerator.GetFileName(settings.Schema, true));
                DeleteFile(settings.AppOutDir, EntryPointGenerator.GetFileName(settings.Schema));

                DeleteFile(settings.AppOutDir, CrudCshtmlInjectorGenerator.GetFileName(settings.Schema));
                DeleteFile(settings.AppOutDir, CrudInjectorGenerator.GetFileName(settings.Schema));
                DeleteFile(settings.AppOutDir, CrudUiTypescriptGenerator.GetFileName(settings.Schema));
                DeleteFile(settings.AppOutDir, UiControllerGenerator.GetFileName(settings.Schema));
            }

            if (settings.WriteTests)
            {
                DeleteFile(settings.TestOutDir, ModelTestWrapper.GetFileName(settings.Schema));
                DeleteFile(settings.TestOutDir, ModelTestWrapperGenerated.GetFileName(settings.Schema));
                DeleteFile(settings.TestOutDir, ControllerTests.GetFileName(settings.Schema));
                DeleteFile(settings.TestOutDir, ProfileTests.GetFileName(settings.Schema));
                DeleteFile(settings.TestOutDir, RepositoryTests.GetFileName(settings.Schema));
                DeleteFile(settings.TestOutDir, $"{settings.ModelName}/Controller.cs"); //Legacy File erase
                DeleteFile(settings.TestOutDir, $"{settings.ModelName}/Profile.cs"); //Legacy File erase
                DeleteFile(settings.TestOutDir, $"{settings.ModelName}/Repository.cs"); //Legacy File erase
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

        private static void WriteFile(String outDir, String file, String content, bool force)
        {
            var fullPath = Path.Combine(outDir, file);

            if (content == null)
            {
                DeleteFile(outDir, file);
            }

            var folder = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            if (force || !File.Exists(fullPath))
            {
                using (var writer = new StreamWriter(File.Open(fullPath, FileMode.Create, FileAccess.Write, FileShare.None)))
                {
                    writer.Write(content);
                }
            }
        }

        private static void DeleteFile(String outDir, String file)
        {
            var fullPath = Path.Combine(outDir, file);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}