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

        public static String Create(JsonSchema4 schema, JsonSchema4 otherSchema, String ns)
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
                allowPropertyCallback: p => !p.IsAbstractOnEntity() && p.CreateEntity(),
                additionalPropertiesCallback: () => AdditionalProperties(schema, otherSchema));
        }

        private static IAttributeBuilder CreateAttributeBuilder()
        {
            return new RequiredAttributeBuilder(new MaxLengthAttributeBuilder());
        }

        private static IEnumerable<KeyValuePair<String, JsonProperty>> AdditionalProperties(JsonSchema4 schema, JsonSchema4 other)
        {
            if (schema.IsLeftModel())
            {
                switch (schema.GetRelationshipKind())
                {
                    case RelationKind.ManyToMany:
                        yield return WriteManyManySide(schema, other);
                        break;
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
                        yield return WriteManyManySide(schema, other);
                        break;
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

        private static KeyValuePair<String, JsonProperty> WriteManyManySide(JsonSchema4 schema, JsonSchema4 other)
        {
            return new KeyValuePair<string, JsonProperty>
            (
                key: $"Join{schema.GetLeftModelName()}To{schema.GetRightModelName()}",
                value: new JsonProperty()
                {
                    Type = JsonObjectType.Array,
                    Item = new JsonSchema4()
                    {
                        Type = JsonObjectType.Object,
                        Format = $"Join{schema.GetLeftModelName()}To{schema.GetRightModelName()}Entity",
                    },
                    Parent = schema
                }
            );
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
                        Format = other.Title,
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
                    Format = other.Title,
                    Parent = schema
                }
            );
        }
    }
}
