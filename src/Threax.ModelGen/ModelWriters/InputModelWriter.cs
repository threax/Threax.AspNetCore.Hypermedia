using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.ModelGen.ModelWriters;
using Threax.AspNetCore.Models;
using System.Linq;

namespace Threax.ModelGen
{
    public static class InputModelWriter
    {
        public static String GetFileName(JsonSchema4 schema)
        {
            return $"InputModels/{schema.Title}Input.Generated.cs";
        }

        public static String Create(JsonSchema4 schema, JsonSchema4 other, String ns)
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
                allowPropertyCallback: p => !p.IsAbstractOnInputModel() && p.CreateInputModel(),
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

        private static KeyValuePair<String,JsonProperty> WriteManySide(JsonSchema4 schema, JsonSchema4 other)
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
