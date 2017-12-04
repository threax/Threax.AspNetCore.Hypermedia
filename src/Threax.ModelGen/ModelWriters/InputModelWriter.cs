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
            var baseClass = ModelTypeGenerator.Create(schema, schema.GetPluralName(), new BaseModelWriter("Input", CreatePropertyAttributes()), ns, ns + ".InputModels", allowPropertyCallback: p =>
            {
                hasBase = hasBase | p.IsVirtual();
                return p.IsVirtual();
            });
            return ModelTypeGenerator.Create(schema, schema.GetPluralName(), new MainModelWriter(hasBase ? baseClass : null, "Input", CreatePropertyAttributes(), CreateClassAttributes(), false, false,
                (b, a) => b.AppendLine(
$@"    public partial class {a.Name}{a.ModelSuffix} : {a.BaseClassName}I{a.Name}
    {{"))
            {
                AdditionalUsings = $"using {ns}.Models;"
            }, ns, ns + ".InputModels", allowPropertyCallback: p => !p.IsVirtual());
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
