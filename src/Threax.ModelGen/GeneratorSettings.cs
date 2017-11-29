using Microsoft.CodeAnalysis.CSharp.Scripting;
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

                var script = CSharpScript.Create(code);
                var runScriptTask = script.RunAsync();
                runScriptTask.Wait();
                var runScriptResult = runScriptTask.Result;

                var typeRecoverTask = runScriptResult.ContinueWithAsync<Type>("return Recovery.GetType();");
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
