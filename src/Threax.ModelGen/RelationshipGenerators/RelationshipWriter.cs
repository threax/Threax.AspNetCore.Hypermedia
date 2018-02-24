using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    public static class RelationshipWriter
    {
        public static String GetFileName(JsonSchema4 schema, JsonSchema4 other)
        {
            return $"Database/{other.Title}Entity.Generated.To{schema.Title}Entity.cs";
        }

        public static String Get(JsonSchema4 schema, string ns)
        {
            switch (schema.GetRelationshipKind())
            {
                case RelationKind.ManyToMany:
                    return ManyToManyRelationshipGenerator.Get(schema, ns);
            }
            return null;
        }
    }
}
