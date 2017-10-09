using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.ModelGen
{
    public class EntryPointGenerator
    {
        public static String Get(String ns, String modelName, String modelPluralName)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(modelName, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(modelPluralName, out Models, out models);
            return Create(ns, Model, model, Models, models);
        }

        private static String Create(String ns, String Model, String model, String Models, String models)
        {
            return
$@"using Halcyon.HAL.Attributes;
using Spc.AspNetCore.Authorization.Entities.Mvc;
using Threax.AspNetCore.Halcyon.Ext;
using {ns}.Controllers.Api;

namespace {ns}.ViewModels
{{
    [HalActionLink(typeof({Models}Controller), nameof({Models}Controller.List), ""List{Models}"")]
    [HalActionLink(typeof({Models}Controller), nameof({Models}Controller.Add), ""Add{Model}"")]
    public partial class EntryPoint
    {{
        
    }}
}}";
        }
    }
}
