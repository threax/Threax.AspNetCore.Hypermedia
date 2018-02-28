using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    public static class TypeToSchemaGenerator
    {
        public static async Task<JsonSchema4> CreateSchema(Type type)
        {
            var schema = await JsonSchema4.FromTypeAsync(type, new NJsonSchema.Generation.JsonSchemaGeneratorSettings()
            {
                DefaultEnumHandling = EnumHandling.String,
                FlattenInheritanceHierarchy = true
            });

            List<JsonProperty> propertiesToRemove = new List<JsonProperty>();
            List<RelationshipSettings> relationships = new List<RelationshipSettings>();

            //Modify the schema to make each none or object property aware of its original type
            foreach (var schemaProp in schema.Properties.Values)
            {
                var prop = type.GetProperty(schemaProp.Name);
                if (prop == null)
                {
                    throw new InvalidOperationException($"Cannot find property {schemaProp.Name} from generated schema on type {type.FullName}");
                }
                var propType = prop.PropertyType;
                schemaProp.SetClrFullTypeName(propType.FullName);

                var finalType = propType;
                var finalObjectProp = schemaProp;
                //If we allow collections check the collection type
                if (typeof(System.Collections.IEnumerable).IsAssignableFrom(propType) && propType.GenericTypeArguments.Length > 0)
                {
                    finalType = propType.GenericTypeArguments.First();
                    schemaProp.Type = JsonObjectType.Array;
                    schemaProp.Item = finalObjectProp = new JsonProperty();
                }

                if (finalObjectProp.IsType(JsonObjectType.None) || finalObjectProp.IsType(JsonObjectType.Object) || propType == typeof(Guid?))
                {
                    finalObjectProp.Type = JsonObjectType.Object;
                    finalObjectProp.Format = finalType.GetSchemaFormat();

                    //For some reason enums do not get the custom attributes, so do it here
                    var enumTestType = finalType;
                    if (enumTestType.IsGenericType && enumTestType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        enumTestType = enumTestType.GetGenericArguments()[0];
                    }
                    if (enumTestType.IsEnum)
                    {
                        foreach (var attr in prop.GetCustomAttributes().Select(i => i as JsonSchemaExtensionDataAttribute).Where(i => i != null))
                        {
                            if (!finalObjectProp.ExtensionData.ContainsKey(attr.Property))
                            {
                                finalObjectProp.ExtensionData.Add(attr.Property, attr.Value);
                            }
                        }
                    }
                    //Check for relationship
                    else if (finalType.Namespace == type.Namespace) //If in the model schema namespace
                    {
                        //Remove this property and use it as a defintion.
                        propertiesToRemove.Add(schemaProp);

                        //Sort names to get which is left and right, should always be consistent this way.
                        var names = new List<Type>() { type, finalType };
                        names.Sort((l, r) => l.Name.CompareTo(r.Name));

                        var left = names[0];
                        var right = names[1];

                        //Figure out relationship type
                        var kind = RelationKind.None;
                        var leftSideProp = left.GetProperties().Where(i =>
                            i.PropertyType == right ||
                            (i.PropertyType.IsGenericType && i.PropertyType.GenericTypeArguments[0] == right)).FirstOrDefault();

                        if(leftSideProp == null)
                        {
                            throw new NotSupportedException($"Cannot find a member with type {right.Name} on {left.Name}.");
                        }

                        var leftMany = typeof(System.Collections.IEnumerable).IsAssignableFrom(leftSideProp.PropertyType)
                                && leftSideProp.PropertyType.GenericTypeArguments.Length > 0;

                        var rightSideProp = right.GetProperties().Where(i =>
                            i.PropertyType == left ||
                            (i.PropertyType.IsGenericType && i.PropertyType.GenericTypeArguments[0] == left)).FirstOrDefault();

                        if(rightSideProp == null)
                        {
                            throw new InvalidOperationException($"Cannot find a member with type {left.Name} on {right.Name}.");
                        }

                        var rightMany = typeof(System.Collections.IEnumerable).IsAssignableFrom(rightSideProp.PropertyType)
                                && rightSideProp.PropertyType.GenericTypeArguments.Length > 0;

                        if (leftMany)
                        {
                            if (rightMany)
                            {
                                kind = RelationKind.ManyToMany;
                            }
                            else
                            {
                                kind = RelationKind.ManyToOne;
                            }
                        }
                        else
                        {
                            if (rightMany)
                            {
                                kind = RelationKind.OneToMany;
                            }
                            else
                            {
                                kind = RelationKind.OneToOne;
                            }
                        }

                        relationships.Add(new RelationshipSettings()
                        {
                            LeftModelName = left.Name,
                            RightModelName = right.Name,
                            LeftClrName = left.FullName,
                            RightClrName = right.FullName,
                            Kind = kind
                        });
                    }
                }
            }

            relationships.AddRange(type.GetCustomAttributes<RelatedToAttribute>().Select(i => i.Settings));
            if (relationships.Count > 0)
            {
                schema.SetRelationshipSettings(relationships);
            }

            foreach (var remove in propertiesToRemove)
            {
                schema.Properties.Remove(remove.Name);
            }

            return schema;
        }
    }
}
