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
        public static String GetFileName(JsonSchema4 schema)
        {
            return $"Database/{schema.Title}Entity.Generated.cs";
        }

        public static String Create(JsonSchema4 schema, Dictionary<String, JsonSchema4> others, String ns)
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
$@"using {ns}.Models;"
+ schema.GetExtraNamespaces(StrConstants.FileNewline)
            };
            return ModelTypeGenerator.Create(schema, schema.GetPluralName(), mainWriter, ns, ns + ".Database",
                allowPropertyCallback: AllowProperty,
                additionalProperties: AdditionalProperties(schema, others));
        }

        private static bool AllowProperty(JsonProperty p)
        {
            return !p.IsAbstractOnEntity() && p.CreateEntity();
        }

        private static IAttributeBuilder CreateAttributeBuilder()
        {
            return new RequiredAttributeBuilder(new MaxLengthAttributeBuilder());
        }

        private static IEnumerable<KeyValuePair<String, JsonProperty>> AdditionalProperties(JsonSchema4 schema, Dictionary<String, JsonSchema4> others)
        {
            IEnumerable<KeyValuePair<String, JsonProperty>> props = new KeyValuePair<String, JsonProperty>[0];
            foreach (var relationship in schema.GetRelationshipSettings())
            {
                if (relationship.IsLeftModel)
                {
                    switch (relationship.Kind)
                    {
                        case RelationKind.ManyToMany:
                            props = props.Concat(WriteManyManySide(schema, others[relationship.OtherModelName], relationship));
                            break;
                        case RelationKind.OneToMany:
                            props = props.Concat(WriteManySide(schema, others[relationship.OtherModelName], relationship));
                            break;
                        case RelationKind.OneToOne:
                        case RelationKind.ManyToOne:
                            props = props.Concat(WriteOneSide(schema, others[relationship.OtherModelName], relationship));
                            break;

                    }
                }
                else
                {
                    switch (relationship.Kind)
                    {
                        case RelationKind.ManyToMany:
                            props = props.Concat(WriteManyManySide(schema, others[relationship.OtherModelName], relationship));
                            break;
                        case RelationKind.ManyToOne:
                            props = props.Concat(WriteManySide(schema, others[relationship.OtherModelName], relationship));
                            break;
                        case RelationKind.OneToOne:
                        case RelationKind.OneToMany:
                            props = props.Concat(WriteOneSide(schema, others[relationship.OtherModelName], relationship));
                            break;
                    }
                }
            }
            return props;
        }

        private static IEnumerable<KeyValuePair<String, JsonProperty>> WriteManyManySide(JsonSchema4 schema, JsonSchema4 other, RelationshipSettings relationship)
        {
            var name = $"Join{relationship.LeftModelName}To{relationship.RightModelName}";

            if (!schema.Properties.ContainsKey(name)) //Don't write if schema defined property.
            {
                yield return new KeyValuePair<string, JsonProperty>
                (
                    key: name,
                    value: new JsonProperty()
                    {
                        Type = JsonObjectType.Array,
                        Item = new JsonSchema4()
                        {
                            Type = JsonObjectType.Object,
                            Format = $"Join{relationship.LeftModelName}To{relationship.RightModelName}Entity",
                        },
                        Parent = schema
                    }
                );
            }
        }

        private static IEnumerable<KeyValuePair<String, JsonProperty>> WriteManySide(JsonSchema4 schema, JsonSchema4 other, RelationshipSettings relationship)
        {
            var name = other.GetPluralName();

            if (!schema.Properties.ContainsKey(name)) //Don't write if schema defined property.
            {
                yield return new KeyValuePair<string, JsonProperty>
                (
                    key: name,
                    value: new JsonProperty()
                    {
                        Type = JsonObjectType.Array,
                        Item = new JsonSchema4()
                        {
                            Type = JsonObjectType.Object,
                            Format = other.Title,
                        },
                        Parent = schema
                    }
                );
            }
        }

        private static IEnumerable<KeyValuePair<String, JsonProperty>> WriteOneSide(JsonSchema4 schema, JsonSchema4 other, RelationshipSettings relationship)
        {
            var name = other.GetKeyName();

            if (!schema.Properties.ContainsKey(name)) //Don't write if schema defined property.
            {
                yield return new KeyValuePair<string, JsonProperty>
                (
                    key: name,
                    value: new JsonProperty()
                    {
                        Type = JsonObjectType.Object,
                        Format = other.GetKeyType().Name,
                        Parent = schema
                    }
                );
            }

            name = other.Title;

            if (!schema.Properties.ContainsKey(name)) //Don't write if schema defined property.
            {
                yield return new KeyValuePair<string, JsonProperty>
                (
                    key: name,
                    value: new JsonProperty()
                    {
                        Type = JsonObjectType.Object,
                        Format = other.Title,
                        Parent = schema
                    }
                );
            }
        }
    }
}
