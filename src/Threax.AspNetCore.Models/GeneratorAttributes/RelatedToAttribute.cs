using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Threax.AspNetCore.Models
{
    /// <summary>
    /// Enum for model relationships, unless otherwise defined goes
    /// ThisModel -> OtherModel
    /// </summary>
    public enum RelationKind
    {
        None,
        OneToOne,
        OneToMany,
        ManyToOne,
        ManyToMany
    }

    /// <summary>
    /// Base class for model relationships.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RelatedToAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "x-relatedto";

        public class Settings
        {
            public String LeftModelName { get; set; }

            public String LeftClrName { get; set; }

            public String RightModelName { get; set; }

            public String RightClrName { get; set; }

            public RelationKind Kind { get; set; }

            public bool IsLeftModel { get; set; }

            /// <summary>
            /// Get the name of the model on the other side of the relationship.
            /// </summary>
            /// <returns></returns>
            public String OtherModelName
            {
                get
                {
                    if (Kind == RelationKind.None)
                    {
                        return null;
                    }

                    if (IsLeftModel)
                    {
                        return RightModelName;
                    }

                    return LeftModelName;
                }
            }

            /// <summary>
            /// Get the name of the model on the other side of the relationship.
            /// </summary>
            /// <returns></returns>
            public String OtherModelClrName
            {
                get
                {
                    if (this.Kind == RelationKind.None)
                    {
                        return null;
                    }

                    if (IsLeftModel)
                    {
                        return RightClrName;
                    }

                    return LeftClrName;
                }
            }

            /// <summary>
            /// Get the name of the side of the relationship, either "Left" or "Right"
            /// will be null if the RelationKind is None.
            /// </summary>
            /// <returns></returns>
            public String SideName
            {
                get
                {
                    if (this.Kind == RelationKind.None)
                    {
                        return null;
                    }

                    if (IsLeftModel)
                    {
                        return "Left";
                    }

                    return "Right";
                }
            }
        }

        public RelatedToAttribute(Type left, Type right, RelationKind kind) : base(Name, new Settings
        {
            LeftModelName = left.Name,
            RightModelName = right.Name,
            LeftClrName = left.FullName,
            RightClrName = right.FullName,
            Kind = kind
        })
        {
            this.Left = left;
            this.Right = right;
            this.RelKind = kind;
        }

        public Type Left { get; private set; }

        public Type Right { get; private set; }

        public RelationKind RelKind { get; set; }
    }

    public static class ModelRelationshipAttributeJsonSchemaExtensions
    {
        /// <summary>
        /// Get the name of the model on the left side of the relationship.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static RelatedToAttribute.Settings GetRelationshipSettings(this JsonSchema4 schema)
        {
            Object val = null;
            RelatedToAttribute.Settings settings;
            if (schema.ExtensionData?.TryGetValue(RelatedToAttribute.Name, out val) == true)
            {
                settings = val as RelatedToAttribute.Settings;
            }
            else
            {
                settings = new RelatedToAttribute.Settings()
                {
                    Kind = RelationKind.None
                };
            }
            settings.IsLeftModel = settings.LeftModelName == schema.Title;
            return settings;
        }
    }
}
