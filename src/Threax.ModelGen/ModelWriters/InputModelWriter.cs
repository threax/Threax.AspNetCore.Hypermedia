using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.ModelGen.ModelWriters;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    static class InputModelWriter
    {
        public static String Create(JsonSchema4 schema, String ns)
        {
            var sb = new StringBuilder();
            bool hasBase = false;
            bool hasRemovedProperties = false;

            var baseModelWriter = new BaseModelWriter("Input", CreatePropertyAttributes());
            var baseClass = ModelTypeGenerator.Create(schema, schema.GetPluralName(), baseModelWriter, ns, ns + ".InputModels", allowPropertyCallback: p =>
            {
                hasRemovedProperties = hasRemovedProperties | !p.CreateInputModel();
                if (p.CreateInputModel())
                {
                    hasBase = hasBase | p.IsVirtual();
                    return p.IsVirtual();
                }
                return false;
            });

            var modelWriter = new MainModelWriter(hasBase ? baseClass : null, "Input", CreatePropertyAttributes(), CreateClassAttributes(), false, false,
                a =>
                {
                    var modelInterfaces = "";
                    if (!hasRemovedProperties)
                    {
                        modelInterfaces = $"I{a.Name} ";
                    }

                    a.Builder.AppendLine(
  $@"    public partial class {a.Name}{a.ModelSuffix}{InterfaceListBuilder.Build(new String[] { a.BaseClassName, modelInterfaces })}
    {{"
                    );
                })
            {
                AdditionalUsings = $"using {ns}.Models;"
            };

            return ModelTypeGenerator.Create(schema, schema.GetPluralName(), modelWriter, ns, ns + ".InputModels", allowPropertyCallback: p => !p.IsVirtual() && p.CreateInputModel());
        }

        private static IAttributeBuilder CreatePropertyAttributes()
        {
            return new DisplayAttributeBuilder(new RequiredAttributeBuilder(new MaxLengthAttributeBuilder(new UiOrderAttributeBuilder())));
        }

        private static IAttributeBuilder CreateClassAttributes()
        {
            return new PredefinedAttributeBuilder("[HalModel]");
        }
    }
}
