using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    public static class JoinEntityWriter
    {
        public static String GetFileName(JsonSchema4 schema)
        {
            if (schema.GetRelationshipKind() == RelationKind.ManyToMany)
            {
                return $"Database/Join{schema.GetLeftModelName()}To{schema.GetRightModelName()}Entity.cs";
            }
            return null;
        }

        public static String Get(JsonSchema4 schema, JsonSchema4 otherSchema, String ns)
        {
            if(otherSchema == null)
            {
                return null;
            }

            return Create(ns,
                schema.GetKeyType().Name,
                NameGenerator.CreatePascal(schema.GetKeyName()),
                NameGenerator.CreatePascal(schema.Title),
                NameGenerator.CreatePascal(schema.GetLeftModelName()),
                NameGenerator.CreatePascal(schema.GetRightModelName()),
                NameGenerator.CreatePascal(otherSchema.GetKeyType().Name),
                NameGenerator.CreatePascal(otherSchema.GetKeyName()),
                NameGenerator.CreatePascal(otherSchema.Title));
        }

        private static String Create(String ns, String ModelType, String ModelId, String Model, String LeftModel, String RightModel, String OtherModelType, String OtherModelId, String OtherModel)
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
        public {ModelType} {ModelId} {{ get; set; }}

        public {Model}Entity {Model} {{ get; set; }}

        public {OtherModelType} {OtherModelId} {{ get; set; }}

        public {OtherModel}Entity {OtherModel} {{ get; set; }}
    }}
}}";
        }
    }
}
