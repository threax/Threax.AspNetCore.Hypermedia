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
    public abstract class RelatedToAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "x-relatedto";

        internal class Settings
        {
            public String Left { get; set; }

            public String Right { get; set; }

            public RelationKind Kind { get; set; }
        }

        public RelatedToAttribute(Type left, Type right, RelationKind kind) : base(Name, new Settings
        {
            Left = left.Name,
            Right = right.Name,
            Kind = kind
        })
        {
            this.Left = left;
            this.Right = right;
            this.RelKind = kind;
        }

        /// <summary>
        /// The type on the other side of the relationship.
        /// </summary>
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
        public static String GetLeftModelName(this JsonSchema4 schema)
        {
            Object val = null;
            if (schema.ExtensionData?.TryGetValue(RelatedToAttribute.Name, out val) == true)
            {
                var settings = val as RelatedToAttribute.Settings;
                if (settings != null)
                {
                    return settings.Left;
                }
            }
            return null;
        }

        /// <summary>
        /// Get the name of the model on the right side of the relationship.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static String GetRightModelName(this JsonSchema4 schema)
        {
            Object val = null;
            if (schema.ExtensionData?.TryGetValue(RelatedToAttribute.Name, out val) == true)
            {
                var settings = val as RelatedToAttribute.Settings;
                if (settings != null)
                {
                    return settings.Right;
                }
            }
            return null;
        }

        /// <summary>
        /// Get the name of the model on the other side of the relationship.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static String GetOtherModelName(this JsonSchema4 schema)
        {
            if(schema.GetRelationshipKind() == RelationKind.None)
            {
                return null;
            }

            if (schema.IsLeftModel())
            {
                return schema.GetRightModelName();
            }

            return schema.GetLeftModelName();
        }

        /// <summary>
        /// Get the name of the side of the relationship, either "Left" or "Right"
        /// will be null if the RelationKind is None.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static String GetSideName(this JsonSchema4 schema)
        {
            if (schema.GetRelationshipKind() == RelationKind.None)
            {
                return null;
            }

            if (schema.IsLeftModel())
            {
                return "Left";
            }

            return "Right";
        }

        /// <summary>
        /// Determine if this is the left model, does not have any real meaning if the relationship is none.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static bool IsLeftModel(this JsonSchema4 schema)
        {
            var leftModelName = GetLeftModelName(schema);
            return leftModelName == schema.Title;
        }

        /// <summary>
        /// Get the kind of relationship this is.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static RelationKind GetRelationshipKind(this JsonSchema4 schema)
        {
            Object val = null;
            if (schema.ExtensionData?.TryGetValue(RelatedToAttribute.Name, out val) == true)
            {
                var settings = val as RelatedToAttribute.Settings;
                if (settings != null)
                {
                    return settings.Kind;
                }
            }
            return RelationKind.None;
        }
    }
}
