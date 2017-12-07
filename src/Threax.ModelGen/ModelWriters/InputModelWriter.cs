using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.ModelGen.ModelWriters;
using Threax.AspNetCore.Models;
using System.Linq;

namespace Threax.ModelGen
{
    static class InputModelWriter
    {
        public static String Create(JsonSchema4 schema, String ns)
        {
            bool hasBase = false;

            var baseModelWriter = new BaseModelWriter("Input", CreatePropertyAttributes());
            var baseClass = ModelTypeGenerator.Create(schema, schema.GetPluralName(), baseModelWriter, ns, ns + ".InputModels", allowPropertyCallback: p =>
            {
                if (p.CreateInputModel())
                {
                    hasBase = hasBase | p.IsAbstractOnInputModel();
                    return p.IsAbstractOnInputModel();
                }
                return false;
            });

            var modelWriter = new MainModelWriter(hasBase ? baseClass : null, "Input", CreatePropertyAttributes(), CreateClassAttributes(), false, false,
                a =>
                {
                    var interfaces = new String[] { a.BaseClassName, }
                        .Concat(IdInterfaceWriter.GetInterfaces(schema, false, p => !p.OnAllModelTypes() && p.CreateInputModel()))
                        .Concat(a.Writer.GetAdditionalInterfaces());

                    a.Builder.AppendLine(
  $@"    public partial class {a.Name}{a.ModelSuffix}{InterfaceListBuilder.Build(interfaces)}
    {{"
                    );
                })
            {
                AdditionalUsings = $@"using {ns}.Models;
using Threax.AspNetCore.Halcyon.Ext.ValueProviders;"
            };

            return ModelTypeGenerator.Create(schema, schema.GetPluralName(), modelWriter, ns, ns + ".InputModels", allowPropertyCallback: p => !p.IsAbstractOnInputModel() && p.CreateInputModel());
        }

        private static IAttributeBuilder CreatePropertyAttributes()
        {
            return new NullValueLabelAttributeBuilder(new DisplayAttributeBuilder(new RequiredAttributeBuilder(new MaxLengthAttributeBuilder(new UiOrderAttributeBuilder()))));
        }

        private static IAttributeBuilder CreateClassAttributes()
        {
            return new PredefinedAttributeBuilder("[HalModel]");
        }
    }
}
