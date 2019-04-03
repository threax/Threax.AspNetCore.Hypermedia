using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Threax.AspNetCore.Models;
using Threax.ModelGen.ModelWriters;

namespace Threax.ModelGen
{
    public static class ViewModelWriter
    {
        public static String GetFileName(JsonSchema4 schema, bool generated)
        {
            var genStr = generated ? ".Generated" : "";
            return $"ViewModels/{schema.Title}{genStr}.cs";
        }

        public static async Task<String> Create(JsonSchema4 schema, Dictionary<String, JsonSchema4> others, String ns, bool generated)
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
                        .Concat(a.Writer.GetAdditionalInterfaces());

                    if (!generated)
                    {
                        a.Builder.AppendLine(GetLinks(schema.GetPluralName()));
                    }

                    a.Builder.AppendLine(
$@"    public partial class {a.Name}{InterfaceListBuilder.Build(interfaces)}
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
                allowPropertyCallback: AllowProperty,
                additionalProperties: await AdditionalProperties(schema, others));
        }

        private static bool AllowProperty(JsonProperty p)
        {
            return !p.IsAbstractOnViewModel() && p.CreateViewModel();
        }

        private static async Task<IEnumerable<KeyValuePair<String, JsonProperty>>> AdditionalProperties(JsonSchema4 schema, Dictionary<String, JsonSchema4> others)
        {
            IEnumerable<KeyValuePair<String, JsonProperty>> props = new KeyValuePair<String, JsonProperty>[0];
            foreach (var relationship in schema.GetRelationshipSettings())
            {
                if (relationship.IsLeftModel)
                {
                    switch (relationship.Kind)
                    {
                        case RelationKind.ManyToMany:
                        case RelationKind.ManyToOne:
                            props = props.Concat(await WriteManySide(schema, others[relationship.OtherModelName], relationship));
                            break;
                        case RelationKind.OneToOne:
                        case RelationKind.OneToMany:
                            props = props.Concat(await WriteOneSide(schema, others[relationship.OtherModelName], relationship));
                            break;
                    }
                }
                else
                {
                    switch (relationship.Kind)
                    {
                        case RelationKind.ManyToMany:
                        case RelationKind.OneToMany:
                            props = props.Concat(await WriteManySide(schema, others[relationship.OtherModelName], relationship));
                            break;
                        case RelationKind.OneToOne:
                        case RelationKind.ManyToOne:
                            props = props.Concat(await WriteOneSide(schema, others[relationship.OtherModelName], relationship));
                            break;
                    }
                }
            }
            return props;
        }

        private static async Task<Dictionary<String, JsonProperty>> WriteManySide(JsonSchema4 schema, JsonSchema4 other, RelationshipSettings relationship)
        {
            var name = other.GetKeyName() + "s"; //Should be xxId so adding s should be fine
            var props = new Dictionary<String, JsonProperty>();

            if (!schema.Properties.ContainsKey(name)) //Don't write if schema defined property.
            {
                props.Add(name, new JsonProperty()
                {
                    Type = JsonObjectType.Array,
                    Item = await TypeToSchemaGenerator.CreateSchema(other.GetKeyType()),
                    Parent = schema,
                    ExtensionData = relationship.CopyExtensions(),
                    Title = relationship.OriginalPropertyDefinition?.Title
                });
            }

            return props;
        }

        private static async Task<Dictionary<String, JsonProperty>> WriteOneSide(JsonSchema4 schema, JsonSchema4 other, RelationshipSettings relationship)
        {
            var name = other.GetKeyName();
            var props = new Dictionary<String, JsonProperty>();

            if (!schema.Properties.ContainsKey(name)) //Don't write if schema defined property.
            {
                var propSchema = await TypeToSchemaGenerator.CreateSchema(other.GetKeyType());

                props.Add(name, new JsonProperty()
                {
                    Type = propSchema.Type,
                    Format = propSchema.Format,
                    Parent = schema,
                    ExtensionData = relationship.CopyExtensions(),
                    Title = relationship.OriginalPropertyDefinition?.Title
                });
            }

            return props;
        }

        private static IAttributeBuilder CreateAttributeBuilder()
        {
            return new NullValueLabelAttributeBuilder(new UiOrderAttributeBuilder(
                new DisplayAttributeBuilder(
                    new UiTypeAttributeBuilder(
                        new ValueProviderAttributeBuilder()))));
        }

        public static String GetUserPartial(JsonSchema4 schema, String ns, String generatedSuffix = ".Generated")
        {
            String Model, model;
            NameGenerator.CreatePascalAndCamel(schema.Title, out Model, out model);
            String Models, models;
            NameGenerator.CreatePascalAndCamel(schema.GetPluralName(), out Models, out models);
            return CreateUserPartial(ns, Model, model, Models, generatedSuffix, schema.GetExtraNamespaces(StrConstants.FileNewline));
        }

        private static String GetLinks(String Models)
        {
            return $@"    [HalModel]
    [HalSelfActionLink(typeof({Models}Controller), nameof({Models}Controller.Get))]
    [HalActionLink(typeof({Models}Controller), nameof({Models}Controller.Update))]
    [HalActionLink(typeof({Models}Controller), nameof({Models}Controller.Delete))]";
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
{GetLinks(Models)}
    public partial class {Model}
    {{
        //You can add your own customizations here. These will not be overwritten by the model generator.
        //See {Model}{generatedSuffix} for the generated code
    }}
}}";
        }
    }
}
