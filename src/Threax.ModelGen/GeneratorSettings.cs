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
            if (Path.GetExtension(Source) != ".json")
            {
                if (!Directory.Exists(AppOutDir))
                {
                    throw new DirectoryNotFoundException($"Cannot find app out directory {AppOutDir}");
                }

                var assembly = ProjectAssemblyLoader.LoadProjectAssembly(AppOutDir);
                var type = assembly.GetType(Source);
                var schemaTask = JsonSchema4.FromTypeAsync(type);
                schemaTask.Wait();
                Schema = schemaTask.Result;
            }
            else
            {
                if (!File.Exists(Source))
                {
                    throw new MessageException($"Cannot find schema file {Source}.");
                }

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
