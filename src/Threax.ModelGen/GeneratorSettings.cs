using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.IO;

namespace Threax.ModelGen
{
    class GeneratorSettings
    {
        public void Configure()
        {
            if (!File.Exists(Source))
            {
                throw new MessageException($"Cannot find schema file {Source}.");
            }

            if (Path.GetExtension(Source) == ".cs")
            {
                String code;

                using(var stream = new StreamReader(File.Open(Source, FileMode.Open, FileAccess.Read, FileShare.Read)))
                {
                    code = stream.ReadToEnd();
                }

                var scriptOptions = ScriptOptions.Default.WithReferences(
                        typeof(System.ComponentModel.DataAnnotations.RequiredAttribute).Assembly,
                        typeof(AspNetCore.Models.PluralNameAttribute).Assembly);

                //If we have an app out dir, try to build and load the assembly the project creates
                if (Directory.Exists(AppOutDir))
                {
                    scriptOptions.WithReferences(ProjectAssemblyLoader.LoadProjectAssembly(AppOutDir));
                }

                var script = CSharpScript.Create(code)
                    .WithOptions(scriptOptions);

                var runScriptTask = script.RunAsync();
                runScriptTask.Wait();
                var runScriptResult = runScriptTask.Result;

                var typeName = Path.GetFileNameWithoutExtension(Path.GetFileName(Source));
                var typeRecoverTask = runScriptResult.ContinueWithAsync<Type>($"return typeof(ModelDefs.{typeName});");
                typeRecoverTask.Wait();
                var typeRecoverResult = typeRecoverTask.Result;
                var type = typeRecoverResult.ReturnValue;

                var schemaTask = JsonSchema4.FromTypeAsync(type);
                schemaTask.Wait();
                Schema = schemaTask.Result;
            }
            else
            {
                var schemaTask = JsonSchema4.FromFileAsync(Source);
                schemaTask.Wait();
                Schema = schemaTask.Result;
            }

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

            //Make sure directories exist before trying to write files
            WriteApp = WriteApp && Directory.Exists(AppOutDir);
            WriteTests = WriteTests && Directory.Exists(TestOutDir);
        }

        public String AppNamespace { get; set; }

        public String Source { get; set; }

        public String AppOutDir { get; set; }

        public String TestOutDir { get; set; }

        public bool WriteApp { get; set; } = true;

        public bool WriteTests { get; set; } = true;

        public string UiController { get; set; } = "Home";

        public String ModelName { get; set; }

        public String PluralModelName { get; set; }

        public JsonSchema4 Schema { get; set; }
    }
}
