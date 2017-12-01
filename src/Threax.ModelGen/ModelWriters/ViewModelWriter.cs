using System;
using System.Collections.Generic;
using System.Text;
using Threax.ModelGen.ModelWriters;

namespace Threax.ModelGen
{
    class ViewModelWriter : ClassWriter
    {
        public ViewModelWriter(bool hasCreated, bool hasModified) : base(hasCreated, hasModified, new AttributeBuilder() { BuildRequired = false, BuildMaxLength = false })
        {
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
            sb.AppendLine( 
$@"    public partial class {name} : I{name}, I{name}Id {GetAdditionalInterfaces()}
    {{"
            );

            CreateProperty(sb, $"{name}Id", new TypeWriterPropertyInfo<Guid>());
        }

        public override void CreateProperty(StringBuilder sb, string name, IWriterPropertyInfo info)
        {
            sb.AppendLine("        [UiOrder]");
            base.CreateProperty(sb, name, info);
        }

        public static String GetUserPartial(String ns, String modelName, String modelPluralName, String generatedSuffix = ".Generated")
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(modelName, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(modelPluralName, out Models, out models);
            return Create(ns, Model, model, Models, generatedSuffix);
        }

        private static String Create(String ns, String Model, String model, String Models, String generatedSuffix)
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
