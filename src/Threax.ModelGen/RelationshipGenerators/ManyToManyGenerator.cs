using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    public class ManyToManyGenerator
    {
        public static String Get(JsonSchema4 schema, String ns)
        {
            return Create(ns, 
                schema.GetKeyType().Name, 
                NameGenerator.CreatePascal(schema.GetKeyName()), 
                NameGenerator.CreatePascal(schema.Title), 
                NameGenerator.CreatePascal(schema.GetOtherModelName()));
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
    }}
}}";
        }
    }
}
