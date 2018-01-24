using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    public class PartialModelInterfaceGenerator
    {
        public static String GetUserPartial(JsonSchema4 schema, String modelNamespace)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
            return Create(Model, model, modelNamespace, NameGenerator.CreatePascal(schema.GetKeyName()), schema.GetExtraNamespaces(StrConstants.FileNewline));
        }

        private static String Create(String Model, String model, String modelNamespace, String ModelId, String additionalNs)
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
    public partial interface I{Model}
    {{
        //Customize main interface here, see {Model}.Generated for generated code
    }}  

    public partial interface I{ModelId}
    {{
        //Customize id interface here, see {Model}.Generated for generated code
    }}    

    public partial interface I{Model}Query
    {{
        //Customize query interface here, see {Model}.Generated for generated code
    }}
}}";
        }
    }
}
