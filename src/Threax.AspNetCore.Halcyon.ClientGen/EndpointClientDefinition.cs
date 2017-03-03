using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;

namespace Threax.AspNetCore.Halcyon.ClientGen
{
    public class EndpointClientDefinition
    {
        private Type type;

        public EndpointClientDefinition(Type type)
        {
            this.type = type;
        }

        public String Name
        {
            get
            {
                return type.Name;
            }
        }

        public bool IsCollectionView
        {
            get
            {
                return typeof(ICollectionView).GetTypeInfo().IsAssignableFrom(type);
            }
        }

        public Type CollectionType
        {
            get
            {
                if (!IsCollectionView)
                {
                    return null;
                }

                var currentType = type;
                while (currentType != null)
                {
                    var currentTypeInfo = currentType.GetTypeInfo();
                    var collectionType = currentTypeInfo.ImplementedInterfaces.FirstOrDefault(t => t.GetGenericTypeDefinition() == typeof(ICollectionView<>));
                    if (collectionType != null)
                    {
                        return collectionType.GetGenericArguments()[0];
                    }
                    else
                    {
                        currentType = currentTypeInfo.BaseType;
                    }
                }

                return typeof(Object);
            }
        }

        public List<EndpointClientLinkDefinition> Links { get; set; } = new List<EndpointClientLinkDefinition>();
    }
}
