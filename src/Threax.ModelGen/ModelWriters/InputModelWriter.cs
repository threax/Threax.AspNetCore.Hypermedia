﻿using NJsonSchema;
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
                allowPropertyCallback: AllowProperty,
                additionalPropertiesCallback: () => AdditionalProperties(schema, other));
        }

        private static bool AllowProperty(JsonProperty p)
        {
            return !p.IsAbstractOnInputModel() && p.CreateInputModel();
        }

        private static IEnumerable<KeyValuePair<String, JsonProperty>> AdditionalProperties(JsonSchema4 schema, JsonSchema4 other)
        {
            if (schema.GetRelationshipSettings().IsLeftModel)
            {
                switch (schema.GetRelationshipSettings().Kind)
                {
                    case RelationKind.ManyToMany:
                    case RelationKind.OneToMany:
                        return WriteManySide(schema, other);
                    case RelationKind.OneToOne:
                    case RelationKind.ManyToOne:
                        return WriteOneSide(schema, other);
                }
            }
            else
            {
                switch (schema.GetRelationshipSettings().Kind)
                {
                    case RelationKind.ManyToMany:
                    case RelationKind.ManyToOne:
                        return WriteManySide(schema, other);
                    case RelationKind.OneToOne:
                    case RelationKind.OneToMany:
                        return WriteOneSide(schema, other);
                }
            }

            return new KeyValuePair<String, JsonProperty>[0];
        }

        private static IEnumerable<KeyValuePair<String,JsonProperty>> WriteManySide(JsonSchema4 schema, JsonSchema4 other)
        {
            var name = other.GetKeyName() + "s"; //Should be xxId so adding s should be fine

            if (!schema.Properties.ContainsKey(name)) //Don't write if schema defined property.
            {
                yield return new KeyValuePair<string, JsonProperty>
                (
                    key: name,
                    value: new JsonProperty()
                    {
                        Type = JsonObjectType.Array,
                        Item = TypeToSchemaGenerator.CreateSchema(other.GetKeyType()).GetAwaiter().GetResult(),
                        Parent = schema
                    }
                );
            }
        }

        private static IEnumerable<KeyValuePair<String, JsonProperty>> WriteOneSide(JsonSchema4 schema, JsonSchema4 other)
        {
            var name = other.Title;

            if (!schema.Properties.ContainsKey(name)) //Don't write if schema defined property.
            {
                var propSchema = TypeToSchemaGenerator.CreateSchema(other.GetKeyType()).GetAwaiter().GetResult();

                yield return new KeyValuePair<string, JsonProperty>
                (
                    key: name,
                    value: new JsonProperty()
                    {
                        Type = propSchema.Type,
                        Format = propSchema.Format,
                        Parent = schema
                    }
                );
            }
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
