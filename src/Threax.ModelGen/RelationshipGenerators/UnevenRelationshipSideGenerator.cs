using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    public static class UnevenRelationshipSideGenerator
    {
        public static String CreateOne(String ns, String LeftModel, String RightModel, String RightModels, String RightKeyType, String RightModelId)
        {
            return
$@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace {ns}.Database
{{
    public partial class {LeftModel}Entity
    {{
        public List<{RightModel}Entity> {RightModels} {{ get; set; }}
    }}
}}";
        }

        public static String CreateMany(String ns, String RightModel, String LeftModel, String LeftKeyType, String LeftModelId, String Suffix, bool writeObjectLink)
        {
            var objectLink = writeObjectLink ? $@"

        public {LeftModel}{Suffix} {LeftModel} {{ get; set; }}" : "";

            return
$@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace {ns}
{{
    public partial class {RightModel}{Suffix}
    {{
        public {LeftKeyType} {LeftModelId} {{ get; set; }}{objectLink}
    }}
}}";
        }
    }

    public class OneToManyGenerator
    {
        public static String Get(JsonSchema4 schema, String ns)
        {
            if (schema.IsLeftModel())
            {
                return UnevenRelationshipSideGenerator.CreateMany(ns + ".Database",
                    NameGenerator.CreatePascal(schema.GetOtherModelName()),
                    NameGenerator.CreatePascal(schema.Title),
                    NameGenerator.CreatePascal(schema.GetKeyType().Name),
                    NameGenerator.CreatePascal(schema.GetKeyName()),
                    "Entity", true);
            }
            else
            {
                return UnevenRelationshipSideGenerator.CreateOne(ns,
                    NameGenerator.CreatePascal(schema.GetOtherModelName()),
                    NameGenerator.CreatePascal(schema.Title),
                    NameGenerator.CreatePascal(schema.GetPluralName()),
                    NameGenerator.CreatePascal(schema.GetKeyType().Name),
                    NameGenerator.CreatePascal(schema.GetKeyName()));
            }
        }
    }

    public class ManyToOneGenerator
    {
        public static String Get(JsonSchema4 schema, String ns)
        {
            if (schema.IsLeftModel())
            {
                return UnevenRelationshipSideGenerator.CreateOne(ns,
                    NameGenerator.CreatePascal(schema.GetOtherModelName()),
                    NameGenerator.CreatePascal(schema.Title),
                    NameGenerator.CreatePascal(schema.GetPluralName()),
                    NameGenerator.CreatePascal(schema.GetKeyType().Name),
                    NameGenerator.CreatePascal(schema.GetKeyName()));
            }
            else
            {
                return UnevenRelationshipSideGenerator.CreateMany(ns + ".Database",
                    NameGenerator.CreatePascal(schema.GetOtherModelName()),
                    NameGenerator.CreatePascal(schema.Title),
                    NameGenerator.CreatePascal(schema.GetKeyType().Name),
                    NameGenerator.CreatePascal(schema.GetKeyName()),
                    "Entity", true);
            }
        }
    }
}
