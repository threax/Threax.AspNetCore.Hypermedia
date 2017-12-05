using NJsonSchema;
using System;
using System.Collections.Generic;
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
            var baseClass = ModelTypeGenerator.Create(schema, schema.GetPluralName(), new BaseModelWriter("Entity", CreateAttributeBuilder()), ns, ns + ".Database", allowPropertyCallback: p =>
            {
                hasBase = hasBase | p.IsVirtual();
                return p.IsVirtual();
            });
            return ModelTypeGenerator.Create(schema, schema.GetPluralName(), new MainModelWriter(hasBase ? baseClass : null, "Entity", CreateAttributeBuilder(), new NoAttributeBuilder(), schema.AllowCreated(), schema.AllowModified(),
                a =>
                {
                    a.Builder.AppendLine(
$@"    public partial class {a.Name}Entity : {a.BaseClassName}I{a.Name}, I{a.Name}Id {a.Writer.GetAdditionalInterfaces()}
    {{
        [Key]"
            );

                    a.Writer.CreateProperty(a.Builder, $"{a.Name}Id", new TypeWriterPropertyInfo<Guid>());
                }
                )
            {
                AdditionalUsings = 
$@"using Threax.AspNetCore.Models;
using {ns}.Models;"
            }, ns, ns + ".Database", allowPropertyCallback: p => !p.IsVirtual());
        }

        private static IAttributeBuilder CreateAttributeBuilder()
        {
            return new IndexPropAttributeBuilder(new RequiredAttributeBuilder(new MaxLengthAttributeBuilder()));
        }
    }
}
