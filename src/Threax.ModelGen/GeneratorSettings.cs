using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    public class GeneratorSettings
    {
        public async Task Configure()
        {
            if (Path.GetExtension(Source) != ".json")
            {
                if (!Directory.Exists(AppOutDir))
                {
                    throw new DirectoryNotFoundException($"Cannot find app out directory {AppOutDir}");
                }

                String ns;
                try
                {
                    var assembly = ProjectAssemblyLoader.LoadProjectAssembly(AppOutDir, out ns);
                    AppNamespace = ns;
                    var type = assembly.GetType(Source);

                    if (type == null)
                    {
                        throw new InvalidOperationException($"Cannot find type {Source} in assembly {assembly.FullName}.");
                    }

                    Schema = await TypeToSchemaGenerator.CreateSchema(type);

                    foreach (var relationship in Schema.GetRelationshipSettings())
                    {
                        if (relationship.Kind != RelationKind.None)
                        {
                            var otherClrName = relationship.OtherModelClrName;
                            var otherType = assembly.GetType(otherClrName);

                            if (otherType == null)
                            {
                                throw new InvalidOperationException($"Cannot find related type {otherClrName}");
                            }

                            var otherSchema = await TypeToSchemaGenerator.CreateSchema(otherType);
                            OtherSchemas[otherSchema.Title] = otherSchema;
                        }
                    }
                }
                catch(FileLoadException ex)
                {
                    throw new RunOnFullFrameworkException(ex);
                }
            }
            else
            {
                if (!File.Exists(Source))
                {
                    throw new MessageException($"Cannot find schema file {Source}.");
                }

                Schema = await JsonSchema4.FromFileAsync(Source);
            }

            if (Schema.ExtensionData == null) //Make sure this exists
            {
                Schema.ExtensionData = new Dictionary<String, Object>();
            }

            ModelName = Schema.Title;
            PluralModelName = Schema.GetPluralName();
            UiController = Schema.GetUiControllerName();

            //Make sure directories exist before trying to write files
            WriteApp = WriteApp && Directory.Exists(AppOutDir);
            WriteTests = WriteTests && Directory.Exists(TestOutDir);

            //Validate
            if (String.IsNullOrWhiteSpace(AppNamespace))
            {
                throw new MessageException($"You must provide an app namespace, one could not be found. Please pass {{--{nameof(AppNamespace)} Your.Namespace}}");
            }
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

        public Dictionary<String, JsonSchema4> OtherSchemas { get; private set; } = new Dictionary<string, JsonSchema4>();

        /// <summary>
        /// Set this to true to force the ui classes to write.
        /// </summary>
        public bool ForceWriteUi { get; set; } = false;

        /// <summary>
        /// Set this to true to force write the app, which will override all files.
        /// </summary>
        public bool ForceWriteApi { get; set; } = false;

        /// <summary>
        /// Set this to true to force write the tests, which will override all files.
        /// </summary>
        public bool ForceWriteTests { get; set; } = false;

        /// <summary>
        /// This will be true to create .Generated files with the generated code. Otherwise put that code into the main file.
        /// </summary>
        public bool CreateGeneratedFiles
        {
            get
            {
                return Schema.CreateGeneratedFiles();
            }
        }
    }
}
