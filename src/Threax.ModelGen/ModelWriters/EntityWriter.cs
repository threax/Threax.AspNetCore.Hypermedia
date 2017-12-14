using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Threax.AspNetCore.Models;
using Threax.ModelGen.ModelWriters;

namespace Threax.ModelGen
{
    public static class EntityWriter
    {
        public static String Create(JsonSchema4 schema, String ns)
        {
            bool hasBase = false;

            var baseWriter = new BaseModelWriter("Entity", CreateAttributeBuilder());
            var baseClass = ModelTypeGenerator.Create(schema, schema.GetPluralName(), baseWriter, ns, ns + ".Database", allowPropertyCallback: p =>
            {
                if (p.CreateEntity())
                {
                    hasBase = hasBase | p.IsAbstractOnEntity();
                    return p.IsAbstractOnEntity();
                }
                return false;
            });

            var mainWriter = new MainModelWriter(hasBase ? baseClass : null, "Entity", CreateAttributeBuilder(), new NoAttributeBuilder(), schema.AllowCreated(), schema.AllowModified(),
                a =>
                {
                    var interfaces = new String[] { a.BaseClassName, }
                        .Concat(IdInterfaceWriter.GetInterfaces(schema, true, p => !p.OnAllModelTypes() && p.CreateEntity()))
                        .Concat(a.Writer.GetAdditionalInterfaces());

                    a.Builder.AppendLine(
$@"    public partial class {a.Name}Entity{InterfaceListBuilder.Build(interfaces)}
    {{
        [Key]"
            );

                    a.Writer.CreateProperty(a.Builder, NameGenerator.CreatePascal(schema.GetKeyName()), new TypeWriterPropertyInfo(schema.GetKeyType()));
                }
                )
            {
                AdditionalUsings =
$@"using Threax.AspNetCore.Models;
using {ns}.Models;"
            };
            return ModelTypeGenerator.Create(schema, schema.GetPluralName(), mainWriter, ns, ns + ".Database", allowPropertyCallback: p => !p.IsAbstractOnEntity() && p.CreateEntity());
        }

        private static IAttributeBuilder CreateAttributeBuilder()
        {
            return new RequiredAttributeBuilder(new MaxLengthAttributeBuilder());
        }
    }
}
