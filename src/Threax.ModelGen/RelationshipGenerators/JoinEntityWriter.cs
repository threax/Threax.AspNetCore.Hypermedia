using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    public static class JoinEntityWriter
    {
        public static String GetFileName(RelationshipSettings relationship, bool generated)
        {
            var genStr = generated ? ".Generated" : "";
            if (relationship.Kind == RelationKind.ManyToMany)
            {
                return $"Database/Join{relationship.LeftModelName}To{relationship.RightModelName}Entity{genStr}.cs";
            }
            return "Does__Not_____Exist.dne";
        }

        public static String Get(JsonSchema4 schema, Dictionary<String, JsonSchema4> otherSchemas, RelationshipSettings relationship, String ns)
        {
            if (relationship.Kind != RelationKind.ManyToMany)
            {
                return null;
            }

            var otherSchema = otherSchemas[relationship.OtherModelName];

            if (relationship.IsLeftModel)
            {
                return Create(ns,
                    NameGenerator.CreatePascal(schema.GetKeyType().Name),
                    NameGenerator.CreatePascal(schema.GetKeyName()),
                    NameGenerator.CreatePascal(schema.Title),
                    NameGenerator.CreatePascal(otherSchema.GetKeyType().Name),
                    NameGenerator.CreatePascal(otherSchema.GetKeyName()),
                    NameGenerator.CreatePascal(otherSchema.Title));
            }
            else
            {
                return Create(ns,
                    NameGenerator.CreatePascal(otherSchema.GetKeyType().Name),
                    NameGenerator.CreatePascal(otherSchema.GetKeyName()),
                    NameGenerator.CreatePascal(otherSchema.Title),
                    NameGenerator.CreatePascal(schema.GetKeyType().Name),
                    NameGenerator.CreatePascal(schema.GetKeyName()),
                    NameGenerator.CreatePascal(schema.Title));
            }
        }

        private static String Create(String ns, String LeftModelType, String LeftModelId, String LeftModel, String RightModelType, String RightModelId, String RightModel)
        {
            return
$@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace {ns}.Database
{{
    public partial class Join{LeftModel}To{RightModel}Entity
    {{
        public {LeftModelType} {LeftModelId} {{ get; set; }}

        public {LeftModel}Entity {LeftModel} {{ get; set; }}

        public {RightModelType} {RightModelId} {{ get; set; }}

        public {RightModel}Entity {RightModel} {{ get; set; }}
    }}
}}";
        }
    }
}
