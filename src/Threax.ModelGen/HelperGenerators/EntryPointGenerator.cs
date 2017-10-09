using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    public class EntryPointGenerator
    {
        public static String Get(String ns, String modelName)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(modelName, out Model, out model);
            return Create(ns, Model, model);
        }

        private static String Create(String ns, String Model, String model)
        {
            return
$@"using Halcyon.HAL.Attributes;
using Spc.AspNetCore.Authorization.Entities.Mvc;
using Threax.AspNetCore.Halcyon.Ext;
using {ns}.Controllers.Api;

namespace {ns}.ViewModels
{{
    [HalActionLink(typeof({Model}sController), nameof({Model}sController.List), ""List{Model}s"")]
    [HalActionLink(typeof({Model}sController), nameof({Model}sController.Add), ""Add{Model}"")]
    public partial class EntryPoint
    {{
        
    }}
}}";
        }
    }
}
