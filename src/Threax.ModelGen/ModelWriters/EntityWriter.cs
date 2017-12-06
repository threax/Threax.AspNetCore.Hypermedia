using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Threax.AspNetCore.Models;
using Threax.ModelGen.ModelWriters;

namespace Threax.ModelGen
{
    static class EntityWriter
    {
        public static String Create(JsonSchema4 schema, String ns)
        {
            var sb = new StringBuilder();
            bool hasBase = false;
            bool hasRemovedProperties = false;

            var baseWriter = new BaseModelWriter("Entity", CreateAttributeBuilder());
            var baseClass = ModelTypeGenerator.Create(schema, schema.GetPluralName(), baseWriter, ns, ns + ".Database", allowPropertyCallback: p =>
            {
                hasRemovedProperties = hasRemovedProperties | !p.CreateEntity();
                if (p.CreateEntity())
                {
                    hasBase = hasBase | p.IsVirtual();
                    return p.IsVirtual();
                }
                return false;
            });

            var mainWriter = new MainModelWriter(hasBase ? baseClass : null, "Entity", CreateAttributeBuilder(), new NoAttributeBuilder(), schema.AllowCreated(), schema.AllowModified(),
                a =>
                {
                    var modelInterfaces = "";
                    if (!hasRemovedProperties)
                    {
                        modelInterfaces = $"I{a.Name}, I{a.Name}Id ";
                    }

                    var interfaces = new String[] { a.BaseClassName, modelInterfaces }.Concat(a.Writer.GetAdditionalInterfaces());

                    a.Builder.AppendLine(
$@"    public partial class {a.Name}Entity{InterfaceListBuilder.Build(interfaces)}
    {{
        [Key]"
            );

                    a.Writer.CreateProperty(a.Builder, $"{a.Name}Id", new TypeWriterPropertyInfo(schema.GetKeyType()));
                }
                )
            {
                AdditionalUsings =
$@"using Threax.AspNetCore.Models;
using {ns}.Models;"
            };
            return ModelTypeGenerator.Create(schema, schema.GetPluralName(), mainWriter, ns, ns + ".Database", allowPropertyCallback: p => !p.IsVirtual());
        }

        private static IAttributeBuilder CreateAttributeBuilder()
        {
            return new RequiredAttributeBuilder(new MaxLengthAttributeBuilder());
        }
    }
}
