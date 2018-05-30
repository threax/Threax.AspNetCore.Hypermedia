using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.ModelGen.ModelWriters;
using Threax.AspNetCore.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.ModelGen
{
    public static class InputModelWriter
    {
        public static String GetFileName(JsonSchema4 schema)
        {
            return $"InputModels/{schema.Title}Input.Generated.cs";
        }

        public static async Task<String> Create(JsonSchema4 schema, Dictionary<String, JsonSchema4> others, String ns)
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
+ schema.GetExtraNamespaces(StrConstants.FileNewline)
            };

            return ModelTypeGenerator.Create(schema, schema.GetPluralName(), modelWriter, ns, ns + ".InputModels",
                allowPropertyCallback: AllowProperty,
                additionalProperties: await AdditionalProperties(schema, others));
        }

        private static bool AllowProperty(JsonProperty p)
        {
            return !p.IsAbstractOnInputModel() && p.CreateInputModel();
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
                        case RelationKind.OneToMany:
                            props = props.Concat(await WriteManySide(schema, others[relationship.OtherModelName], relationship));
                            break;
                        case RelationKind.OneToOne:
                        case RelationKind.ManyToOne:
                            props = props.Concat(await WriteOneSide(schema, others[relationship.OtherModelName], relationship));
                            break;
                    }
                }
                else
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
                    ExtensionData = relationship.CopyExtensionData(),
                });
            }

            return props;
        }

        private static async Task<Dictionary<String, JsonProperty>> WriteOneSide(JsonSchema4 schema, JsonSchema4 other, RelationshipSettings relationship)
        {
            var name = other.Title;
            var props = new Dictionary<String, JsonProperty>();

            if (!schema.Properties.ContainsKey(name)) //Don't write if schema defined property.
            {
                var propSchema = await TypeToSchemaGenerator.CreateSchema(other.GetKeyType());

                props.Add(name, new JsonProperty()
                {
                    Type = propSchema.Type,
                    Format = propSchema.Format,
                    Parent = schema,
                    ExtensionData = relationship.CopyExtensionData(),
                });
            }

            return props;
        }

        private static IAttributeBuilder CreatePropertyAttributes()
        {
            return new NullValueLabelAttributeBuilder(
                new DisplayAttributeBuilder(
                    new RequiredAttributeBuilder(
                        new MaxLengthAttributeBuilder(
                            new UiOrderAttributeBuilder(
                                new UiTypeAttributeBuilder(
                                    new ValueProviderAttributeBuilder()))))));
        }

        private static IAttributeBuilder CreateClassAttributes()
        {
            return new PredefinedAttributeBuilder("[HalModel]");
        }
    }
}
