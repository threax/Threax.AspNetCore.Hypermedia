using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Threax.AspNetCore.Models;
using Threax.ModelGen.ModelWriters;

namespace Threax.ModelGen
{
    public static class ViewModelWriter
    {
        public static String GetFileName(JsonSchema4 schema)
        {
            return $"ViewModels/{schema.Title}.Generated.cs";
        }

        public static String Create(JsonSchema4 schema, JsonSchema4 other, String ns)
        {
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

                    a.Writer.CreateProperty(a.Builder, NameGenerator.CreatePascal(schema.GetKeyName()), new TypeWriterPropertyInfo(schema.GetKeyType()));
                }
                )
            {
                AdditionalUsings =
$@"using {ns}.Models;
using {ns}.Controllers.Api;
using Threax.AspNetCore.Halcyon.Ext.ValueProviders;"
+ schema.GetExtraNamespaces(StrConstants.FileNewline)
            };
            return ModelTypeGenerator.Create(schema, schema.GetPluralName(), mainWriter, ns, ns + ".ViewModels",
                allowPropertyCallback: p => !p.IsAbstractOnViewModel() && p.CreateViewModel(),
                additionalPropertiesCallback: () => AdditionalProperties(schema, other));
        }

        private static IEnumerable<KeyValuePair<String, JsonProperty>> AdditionalProperties(JsonSchema4 schema, JsonSchema4 other)
        {
            if (schema.IsLeftModel())
            {
                switch (schema.GetRelationshipKind())
                {
                    case RelationKind.ManyToMany:
                    case RelationKind.OneToMany:
                        yield return WriteManySide(schema, other);
                        break;
                    case RelationKind.OneToOne:
                    case RelationKind.ManyToOne:
                        yield return WriteOneSide(schema, other);
                        break;
                }
            }
            else
            {
                switch (schema.GetRelationshipKind())
                {
                    case RelationKind.ManyToMany:
                    case RelationKind.ManyToOne:
                        yield return WriteManySide(schema, other);
                        break;
                    case RelationKind.OneToOne:
                    case RelationKind.OneToMany:
                        yield return WriteOneSide(schema, other);
                        break;
                }
            }
        }

        private static KeyValuePair<String, JsonProperty> WriteManySide(JsonSchema4 schema, JsonSchema4 other)
        {
            return new KeyValuePair<string, JsonProperty>
            (
                key: other.GetPluralName(),
                value: new JsonProperty()
                {
                    Type = JsonObjectType.Array,
                    Item = new JsonSchema4()
                    {
                        Type = JsonObjectType.Object,
                        Format = "Guid",
                    },
                    Parent = schema
                }
            );
        }

        private static KeyValuePair<String, JsonProperty> WriteOneSide(JsonSchema4 schema, JsonSchema4 other)
        {
            return new KeyValuePair<string, JsonProperty>
            (
                key: other.Title,
                value: new JsonProperty()
                {
                    Type = JsonObjectType.Object,
                    Format = "Guid",
                    Parent = schema
                }
            );
        }

        private static IAttributeBuilder CreateAttributeBuilder()
        {
            return new NullValueLabelAttributeBuilder(new UiOrderAttributeBuilder(
                new DisplayAttributeBuilder(
                    new UiTypeAttributeBuilder(
                        new ValueProviderAttributeBuilder()))));
        }

        public static String GetUserPartialFileName(JsonSchema4 schema)
        {
            return $"ViewModels/{schema.Title}.cs";
        }

        public static String GetUserPartial(JsonSchema4 schema, String ns, String generatedSuffix = ".Generated")
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(schema.GetPluralName(), out Models, out models);
            return CreateUserPartial(ns, Model, model, Models, generatedSuffix, schema.GetExtraNamespaces(StrConstants.FileNewline));
        }

        private static String CreateUserPartial(String ns, String Model, String model, String Models, String generatedSuffix, String additionalNs)
        {
            return
$@"using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;
using {ns}.Models;
using {ns}.Controllers.Api;{additionalNs}

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
