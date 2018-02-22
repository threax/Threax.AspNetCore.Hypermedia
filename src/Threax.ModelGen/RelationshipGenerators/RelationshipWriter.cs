using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Threax.AspNetCore.Models;

namespace Threax.ModelGen
{
    public static class RelationshipWriter
    {
        public static String Get(JsonSchema4 schema, string ns)
        {
            switch (schema.GetRelationshipKind())
            {
                case RelationKind.ManyToMany:
                    return ManyToManyRelationshipGenerator.Get(schema, ns);
                case RelationKind.OneToMany:
                    return OneToManyGenerator.Get(schema, ns);
                case RelationKind.ManyToOne:
                    return ManyToOneGenerator.Get(schema, ns);
                case RelationKind.OneToOne:
                    return OneToOneGenerator.Get(schema, ns);
            }
            return null;
        }
    }
}
