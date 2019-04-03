using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen.TestGenerators
{
    public class ModelTestWrapperGenerated
    {
        public static String GetFileName(JsonSchema4 schema)
        {
            return $"{schema.Title}/{schema.Title}Tests.Generated.cs";
        }

        public static String Get(JsonSchema4 schema, String ns)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(schema.GetPluralName(), out Models, out models);
            string inputToEntityEqualAssertFunc, entityToViewEqualAssertFunc, createInputFunc, createEntityFunc, createViewFunc;
            GetTestHelpers(schema, ns, out inputToEntityEqualAssertFunc, out entityToViewEqualAssertFunc, out createInputFunc, out createEntityFunc, out createViewFunc);

            return Create(ns, Model, model, Models, models, inputToEntityEqualAssertFunc, entityToViewEqualAssertFunc, createInputFunc, createEntityFunc, createViewFunc, schema.GetExtraNamespaces(StrConstants.FileNewline));
        }

        public static string GetTestHelpers(JsonSchema4 schema, string ns, out string inputToEntityEqualAssertFunc, out string entityToViewEqualAssertFunc, out string createInputFunc, out string createEntityFunc, out string createViewFunc)
        {
            String createArgs = "";

            inputToEntityEqualAssertFunc = "";
            if (schema.CreateInputModel() && schema.CreateEntity())
            {
                inputToEntityEqualAssertFunc = ModelTypeGenerator.Create(schema, schema.GetPluralName(), new ModelEqualityAssert(i => i.OnInputModel && i.OnEntity, "Input", "Entity"), schema, ns, ns);
            }

            entityToViewEqualAssertFunc = "";
            if (schema.CreateEntity() && schema.CreateViewModel())
            {
                entityToViewEqualAssertFunc = ModelTypeGenerator.Create(schema, schema.GetPluralName(), new ModelEqualityAssert(i => i.OnEntity && i.OnViewModel, "Entity", ""), schema, ns, ns);
            }

            createInputFunc = "";
            if (schema.CreateInputModel())
            {
                createArgs = ModelTypeGenerator.Create(schema, schema.GetPluralName(), new ModelCreateArgs(), schema, ns, ns, p => p.CreateInputModel());
                createInputFunc = ModelTypeGenerator.Create(schema, schema.GetPluralName(), new CreateInputModel(createArgs), schema, ns, ns, p => p.CreateInputModel());
            }

            createEntityFunc = "";
            if (schema.CreateEntity())
            {
                createArgs = ModelTypeGenerator.Create(schema, schema.GetPluralName(), new ModelCreateArgs(), schema, ns, ns, p => p.CreateEntity());
                createEntityFunc = ModelTypeGenerator.Create(schema, schema.GetPluralName(), new CreateEntity(schema, createArgs), schema, ns, ns, p => p.CreateEntity());
            }

            createViewFunc = "";
            if (schema.CreateViewModel())
            {
                createArgs = ModelTypeGenerator.Create(schema, schema.GetPluralName(), new ModelCreateArgs(), schema, ns, ns, p => p.CreateViewModel());
                createViewFunc = ModelTypeGenerator.Create(schema, schema.GetPluralName(), new CreateViewModel(schema, createArgs), schema, ns, ns, p => p.CreateViewModel());
            }

            return createArgs;
        }

        private static String Create(String ns, String Model, String model, String Models, String models, String inputToEntityEqualAssertFunc, String entityToViewEqualAssertFunc, String createInputFunc, String createEntityFunc, String createViewFunc, String additionalNs)
        {
            var sb = new StringBuilder(
$@"using AutoMapper;
using {ns}.Database;
using {ns}.InputModels;
using {ns}.Repository;
using {ns}.Models;
using {ns}.ViewModels;
using System;
using Threax.AspNetCore.Tests;
using Xunit;
using System.Collections.Generic;{additionalNs}

namespace {ns}.Tests
{{
    public static partial class {Model}Tests
    {{");
            if (createInputFunc != "")
            {
                sb.AppendLine();
                sb.Append(createInputFunc);
            }

            if (createEntityFunc != "")
            {
                sb.AppendLine();
                sb.Append(createEntityFunc);
            }

            if (createViewFunc != "")
            {
                sb.AppendLine();
                sb.Append(createViewFunc);
            }

            if (inputToEntityEqualAssertFunc != "")
            {
                sb.AppendLine();
                sb.Append(inputToEntityEqualAssertFunc);
            }

            if (entityToViewEqualAssertFunc != "")
            {
                sb.AppendLine();
                sb.Append(entityToViewEqualAssertFunc);
            }
            sb.Append(
$@"
    }}
}}");
            return sb.ToString();
        }
    }
}
