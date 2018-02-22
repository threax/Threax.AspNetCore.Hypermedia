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
                    NameGenerator.CreatePascal(schema.GetOtherModelName()),
                    NameGenerator.CreatePascal(schema.GetLeftModelName()),
                    NameGenerator.CreatePascal(schema.GetRightModelName()));
        }

        public static String Create(String ns, String OtherModel, String LeftModel, String RightModel)
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
}}";
        }
    }
}
