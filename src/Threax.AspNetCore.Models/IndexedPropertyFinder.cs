using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Threax.AspNetCore.Models
{
    public class IndexedPropertyFinder
    {
        private Type type;
        private IEnumerable<Type> supportedPropertyTypes;

        public IndexedPropertyFinder(Type type, IEnumerable<Type> supportedPropertyTypes)
        {
            this.type = type;
            this.supportedPropertyTypes = supportedPropertyTypes;
        }

        public IEnumerable<IndexInfo> GetIndexProps()
        {
            foreach(var prop in type.GetProperties())
            {
                var propType = prop.PropertyType;
                var checkPropType = propType.IsGenericType ? propType.GetGenericTypeDefinition() : propType;
                if (supportedPropertyTypes.Any(i => i.IsAssignableFrom(checkPropType)))
                {
                    var entityType = propType;
                    if (propType.IsGenericType)
                    {
                        entityType = propType.GetGenericArguments()[0];
                    }

                    foreach(var info in GetEntityProps(entityType))
                    {
                        yield return info;
                    }
                }
            }
        }

        private IEnumerable<IndexInfo> GetEntityProps(Type entityType)
        {
            foreach(var prop in entityType.GetProperties())
            {
                var propAttribute = prop.GetCustomAttribute<IndexPropAttribute>(true);

                if (propAttribute != null)
                {
                    yield return new IndexInfo()
                    {
                        Type = entityType,
                        PropertyInfo = prop,
                        IndexAttribute = propAttribute
                    };
                }
            }
        }
    }
}
