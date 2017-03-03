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
        private const BindingFlags AllConstructorsFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance;

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

        public String CollectionType
        {
            get
            {
                if (!IsCollectionView)
                {
                    return null;
                }

                //Try to activate the type and return the type the class specifies, only works if there is an empty constructor
                try
                {
                    if (type.GetTypeInfo().GetConstructors(AllConstructorsFlags).Any(i => i.GetParameters().Length == 0))
                    {
                        var instance = Activator.CreateInstance(type, true) as ICollectionView;
                        return instance.CollectionType.Name;
                    }
                }
                catch (Exception)
                {
                    //Do nothing, means we can't instantiate it
                }

                //No empty constructor found, search the type to see if it is a ICollectionView and return its generic type instead.
                var currentType = type;
                while (currentType != null)
                {
                    var currentTypeInfo = currentType.GetTypeInfo();
                    var collectionType = currentTypeInfo.ImplementedInterfaces.FirstOrDefault(t => t.GetGenericTypeDefinition() == typeof(ICollectionView<>));
                    if (collectionType != null)
                    {
                        return collectionType.GetGenericArguments()[0].Name;
                    }
                    else
                    {
                        currentType = currentTypeInfo.BaseType;
                    }
                }

                //Couldn't figure it out, return null to represent anything.
                return null;
            }
        }

        public List<EndpointClientLinkDefinition> Links { get; set; } = new List<EndpointClientLinkDefinition>();
    }
}
