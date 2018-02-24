using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    public class PartialTypeGenerator
    {
        public static String GetEntityFileName(JsonSchema4 schema)
        {
            return $"Database/{schema.Title}Entity.cs";
        }

        public static String GetInputFileName(JsonSchema4 schema)
        {
            return $"InputModels/{schema.Title}Input.cs";
        }

        public static String GetQueryFileName(JsonSchema4 schema)
        {
            return $"InputModels/{schema.Title}Query.cs";
        }

        public static String Get(JsonSchema4 schema, String modelNamespace, String modelType, String additionalNs)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
            return Create(Model, model, modelNamespace, modelType, additionalNs);
        }

        private static String Create(String Model, String model, String modelNamespace, String modelType, String additionalNs)
        {
            return
$@"using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;{additionalNs}

namespace {modelNamespace}
{{
    public partial class {Model}{modelType}
    {{
        //You can add your own customizations here. These will not be overwritten by the model generator.
        //See {Model}{modelType}.Generated for the generated code
    }}
}}";
        }
    }
}
