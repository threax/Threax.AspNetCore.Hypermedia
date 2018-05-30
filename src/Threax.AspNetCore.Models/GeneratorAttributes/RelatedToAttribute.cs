using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public class RelationshipSettings
    {
        public String LeftModelName { get; set; }

        public String LeftClrName { get; set; }

        public String RightModelName { get; set; }

        public String RightClrName { get; set; }

        public RelationKind Kind { get; set; }

        public bool IsLeftModel { get; set; }

        public JsonSchema4 OriginalPropertyDefinition { get; set; }

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

    /// <summary>
    /// Base class for model relationships.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RelatedToAttribute : Attribute
    {
        internal const String Name = "x-relatedto";

        public RelatedToAttribute(Type left, Type right, RelationKind kind)
        {
            this.Settings = new RelationshipSettings
            {
                LeftModelName = left.Name,
                RightModelName = right.Name,
                LeftClrName = left.FullName,
                RightClrName = right.FullName,
                Kind = kind
            };
        }

        public RelationshipSettings Settings { get; set; }
    }

    public static class ModelRelationshipAttributeJsonSchemaExtensions
    {
        /// <summary>
        /// Get the name of the model on the left side of the relationship.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static IEnumerable<RelationshipSettings> GetRelationshipSettings(this JsonSchema4 schema)
        {
            Object val = null;
            IEnumerable<RelationshipSettings> settings = null;
            if (schema.ExtensionData?.TryGetValue(RelatedToAttribute.Name, out val) == true)
            {
                settings = val as IEnumerable<RelationshipSettings>;
            }
            if (settings == null)
            {
                settings = new RelationshipSettings[0];
            }
            return settings.Select(i =>
            {
                i.IsLeftModel = i.LeftModelName == schema.Title;
                return i;
            });
        }

        public static void SetRelationshipSettings(this JsonSchema4 schema, IEnumerable<RelationshipSettings> value)
        {
            if (schema.ExtensionData == null)
            {
                schema.ExtensionData = new Dictionary<String, Object>();
            }

            schema.ExtensionData[RelatedToAttribute.Name] = value;
        }
    }
}
