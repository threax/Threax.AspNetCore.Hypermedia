using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    public class ManyToManyRelationshipGenerator
    {
        public static String Get(JsonSchema4 schema, String ns)
        {
            return Create(ns,
                schema.GetKeyType().Name,
                NameGenerator.CreatePascal(schema.GetKeyName()),
                NameGenerator.CreatePascal(schema.Title),
                NameGenerator.CreatePascal(schema.GetLeftModelName()),
                NameGenerator.CreatePascal(schema.GetRightModelName()),
                NameGenerator.CreatePascal(schema.GetOtherModelName()));
        }

        private static String Create(String ns, String ModelType, String ModelId, String Model, String LeftModel, String RightModel, String OtherModel)
        {
            return
$@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace {ns}.Database
{{
    public partial class {OtherModel}Entity
    {{
        public List<{LeftModel}To{RightModel}Entity> {LeftModel}To{RightModel}Entities {{ get; set; }}
    }}
}}

namespace {ns}.Database
{{
    public partial class {LeftModel}To{RightModel}Entity
    {{
        public {ModelType} {ModelId} {{ get; set; }}

        public {Model}Entity {Model} {{ get; set; }}
    }}
}}";
        }
    }
}
