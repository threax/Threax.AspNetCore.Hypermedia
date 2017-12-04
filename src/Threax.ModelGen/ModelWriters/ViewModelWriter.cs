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
            //Names and namespaces don't matter, just generating properties.
            var baseClass = ModelTypeGenerator.Create(schema, schema.GetPluralName(), new BaseModelWriter(), ns, ns + ".ViewModels", allowPropertyCallback: p =>
            {
                hasBase = hasBase | p.IsVirtual();
                return p.IsVirtual();
            });
            return ModelTypeGenerator.Create(schema, schema.GetPluralName(), new MainModelWriter(schema, hasBase ? baseClass : null), ns, ns + ".ViewModels", allowPropertyCallback: p => !p.IsVirtual());
        }

        class BaseModelWriter : ClassWriter
        {
            public BaseModelWriter() : base(false, false, CreateAttributeBuilder())
            {
                this.WriteUsings = false;
                this.WriteNamespace = false;
                this.WritePropertiesVirtual = true;
            }

            public override void StartType(StringBuilder sb, string name, string pluralName)
            {
                sb.AppendLine(
$@"    public class {name}Base
    {{"
);
            }
        }

        class MainModelWriter : ClassWriter
        {
            private String baseClass;

            public MainModelWriter(JsonSchema4 schema, String baseClass) : base(schema.AllowCreated(), schema.AllowModified(), CreateAttributeBuilder())
            {
                this.baseClass = baseClass;
            }

            public override void AddUsings(StringBuilder sb, string ns)
            {
                base.AddUsings(sb, ns);
                sb.AppendLine(
$@"using {ns}.Models;
using {ns}.Controllers.Api;"
                );
            }

            public override void StartType(StringBuilder sb, String name, String pluralName)
            {
                var baseClassName = "";
                if (baseClass != null)
                {
                    baseClassName = $"{name}Base, ";
                    sb.AppendLine(baseClass);
                }

                sb.AppendLine(
$@"    public partial class {name} : {baseClassName}I{name}, I{name}Id {GetAdditionalInterfaces()}
    {{"
                );

                CreateProperty(sb, $"{name}Id", new TypeWriterPropertyInfo<Guid>());
            }
        }

        private static IAttributeBuilder CreateAttributeBuilder()
        {
            return new UiOrderAttributeBuilder(new DisplayAttributeBuilder());
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
