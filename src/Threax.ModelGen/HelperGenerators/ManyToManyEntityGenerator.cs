using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    public class ManyToManyEntityGenerator
    {
        public static String Get(JsonSchema4 schema, String ns)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(schema.GetPluralName(), out Models, out models);
            return Create(ns, schema.GetKeyType().Name, NameGenerator.CreatePascal(schema.GetKeyName()), NameGenerator.CreatePascal(schema.GetLeftModelName()), NameGenerator.CreatePascal(schema.GetRightModelName()));
        }

        private static String Create(String ns, String ModelType, String ModelId, String Model, String OtherModel)
        {
            return
$@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace {ns}.Database
{{
    public partial class {Model}To{OtherModel}Entity
    {{
        public {ModelType} {ModelId} {{ get; set; }}

        public {Model}Entity {Model} {{ get; set; }}

        public Guid {OtherModel}Id {{ get; set; }}

        public {OtherModel}Entity {OtherModel} {{ get; set; }}
    }}
}}";
        }
    }
}
