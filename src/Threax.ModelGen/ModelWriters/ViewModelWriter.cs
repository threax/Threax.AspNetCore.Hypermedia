using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Threax.AspNetCore.Models;
using Threax.ModelGen.ModelWriters;

namespace Threax.ModelGen
{
    static class ViewModelWriter
    {
        public static String Create(JsonSchema4 schema, String ns)
        {
            var sb = new StringBuilder();
            bool hasBase = false;

            var baseWriter = new BaseModelWriter("", CreateAttributeBuilder());
            var baseClass = ModelTypeGenerator.Create(schema, schema.GetPluralName(), baseWriter, ns, ns + ".ViewModels", allowPropertyCallback: p =>
            {
                if (p.CreateViewModel())
                {
                    hasBase = hasBase | p.IsAbstractOnViewModel();
                    return p.IsAbstractOnViewModel();
                }
                return false;
            });

            var mainWriter = new MainModelWriter(hasBase ? baseClass : null, "", CreateAttributeBuilder(), new NoAttributeBuilder(), schema.AllowCreated(), schema.AllowModified(),
                a =>
                {
                    var interfaces = new String[] { a.BaseClassName, }
                        .Concat(IdInterfaceWriter.GetInterfaces(schema, true, p => !p.OnAllModelTypes() && p.CreateViewModel()))
                        .Concat(a.Writer.GetAdditionalInterfaces());

                    a.Builder.AppendLine(
$@"       public partial class {a.Name}{InterfaceListBuilder.Build(interfaces)}
       {{");

                    a.Writer.CreateProperty(a.Builder, $"{a.Name}Id", new TypeWriterPropertyInfo(schema.GetKeyType()));
                }
                )
            {
                AdditionalUsings =
$@"using {ns}.Models;
using {ns}.Controllers.Api;
using Threax.AspNetCore.Halcyon.Ext.ValueProviders;"
            };
            return ModelTypeGenerator.Create(schema, schema.GetPluralName(), mainWriter, ns, ns + ".ViewModels", allowPropertyCallback: p => !p.IsAbstractOnViewModel() && p.CreateViewModel());
        }

        private static IAttributeBuilder CreateAttributeBuilder()
        {
            return new NullValueLabelAttributeBuilder(new UiOrderAttributeBuilder(new DisplayAttributeBuilder()));
        }

        public static String GetUserPartial(String ns, String modelName, String modelPluralName, String generatedSuffix = ".Generated")
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(modelName, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(modelPluralName, out Models, out models);
            return CreateUserPartial(ns, Model, model, Models, generatedSuffix);
        }

        private static String CreateUserPartial(String ns, String Model, String model, String Models, String generatedSuffix)
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
using {ns}.Models;
using {ns}.Controllers.Api;

namespace {ns}.ViewModels
{{
    [HalModel]
    [HalSelfActionLink(typeof({Models}Controller), nameof({Models}Controller.Get))]
    [HalActionLink(typeof({Models}Controller), nameof({Models}Controller.Update))]
    [HalActionLink(typeof({Models}Controller), nameof({Models}Controller.Delete))]
    public partial class {Model}
    {{
        //You can add your own customizations here. These will not be overwritten by the model generator.
        //See {Model}{generatedSuffix} for the generated code
    }}
}}";
        }
    }
}
