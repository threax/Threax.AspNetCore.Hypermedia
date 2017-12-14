using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    public class PartialTypeGenerator
    {
        public static String GetUserPartial(String modelName, String modelNamespace, String modelType, String generatedSuffix = ".Generated")
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(modelName, out Model, out model);
            return Create(Model, model, modelNamespace, modelType, generatedSuffix);
        }

        private static String Create(String Model, String model, String modelNamespace, String modelType, String generatedSuffix)
        {
            return
$@"using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;

namespace {modelNamespace}
{{
    public partial class {Model}{modelType}
    {{
        //You can add your own customizations here. These will not be overwritten by the model generator.
        //See {Model}{modelType}{generatedSuffix} for the generated code
    }}
}}";
        }
    }
}
