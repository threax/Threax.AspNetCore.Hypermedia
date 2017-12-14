using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    public class EntryPointGenerator
    {
        public static String Get(JsonSchema4 schema, String ns)
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(schema.GetPluralName(), out Models, out models);
            return Create(ns, Model, model, Models, models);
        }

        private static String Create(String ns, String Model, String model, String Models, String models)
        {
            return
$@"using Halcyon.HAL.Attributes;
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
