using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    public class OneToOneGenerator
    {
        public static String Get(JsonSchema4 schema, String ns)
        {
            return CreateOne(ns,
                    NameGenerator.CreatePascal(schema.GetOtherModelName()),
                    NameGenerator.CreatePascal(schema.Title),
                    NameGenerator.CreatePascal(schema.GetKeyType().Name),
                    NameGenerator.CreatePascal(schema.GetKeyName()));
        }

        public static String CreateOne(String ns, String OtherModel, String ThisModel, String ThisKeyType, String ThisModelId)
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
        public {ThisKeyType} {ThisModelId} {{ get; set; }}

        public {ThisModel}Entity {ThisModel} {{ get; set; }}
    }}
}}";
        }
    }
}
