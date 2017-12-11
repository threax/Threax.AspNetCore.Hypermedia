using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    class PartialModelInterfaceGenerator
    {
        public static String GetUserPartial(JsonSchema4 schema, String modelNamespace, String generatedSuffix = ".Generated")
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
            return Create(Model, model, modelNamespace, generatedSuffix, NameGenerator.CreatePascal(schema.GetKeyName()));
        }

        private static String Create(String Model, String model, String modelNamespace, String generatedSuffix, String ModelId)
        {
            return
$@"using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Halcyon.Ext.UIAttrs;

namespace {modelNamespace}
{{
    public partial interface I{Model}
    {{
        //Customize main interface here, see {Model}{generatedSuffix} for generated code
    }}  

    public partial interface I{ModelId}
    {{
        //Customize id interface here, see {Model}{generatedSuffix} for generated code
    }}    

    public partial interface I{Model}Query
    {{
        //Customize query interface here, see {Model}{generatedSuffix} for generated code
    }}
}}";
        }
    }
}
