using Threax.NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    public class PartialTypeGenerator
    {
        public static String GetJoinEntity(JsonSchema4 schema, RelationshipSettings relationship, String ns)
        {
            if (relationship.Kind == RelationKind.ManyToMany)
            {
                return Create($"Join{relationship.LeftModelName}To{relationship.RightModelName}Entity", "", ns + ".Database", "", schema.GetExtraNamespaces(StrConstants.FileNewline));
            }
            return null;
        }

        public static String GetEntity(JsonSchema4 schema, String ns)
        {
            return Get(schema, ns + ".Database", "Entity", schema.GetExtraNamespaces(StrConstants.FileNewline));
        }

        public static String GetInput(JsonSchema4 schema, String ns)
        {
            return Get(schema, ns + ".InputModels", "Input", schema.GetExtraNamespaces(StrConstants.FileNewline));
        }

        private static String Get(JsonSchema4 schema, String modelNamespace, String modelType, String additionalNs)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
            return Create(Model, model, modelNamespace, modelType, additionalNs);
        }

        private static String Create(String Model, String model, String modelNamespace, String ModelType, String additionalNs)
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
    public partial class {Model}{ModelType}
    {{
        //You can add your own customizations here. These will not be overwritten by the model generator.
        //See {Model}{ModelType}.Generated for the generated code
    }}
}}";
        }
    }
}
