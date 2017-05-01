using Halcyon.HAL.Attributes;
using NJsonSchema;
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
        private List<EndpointClientLinkDefinition> clientLinks = new List<EndpointClientLinkDefinition>();

        public EndpointClientDefinition()
        {
        }

        public EndpointClientDefinition(Type type, JsonSchema4 schema)
        {
            this.Name = type.Name;
            this.IsEntryPoint = type.GetTypeInfo().GetCustomAttribute(typeof(HalEntryPointAttribute)) != null;
            this.IsCollectionView = typeof(ICollectionView).GetTypeInfo().IsAssignableFrom(type);
            this.CollectionType = GetCollectionType(type, IsCollectionView);
            this.Schema = schema;
        }

        public void AddLink(EndpointClientLinkDefinition linkDefinition)
        {
            if(clientLinks.Any(i => i.Rel == linkDefinition.Rel))
            {
                throw new ClientGenException($"Duplicate rel {linkDefinition.Rel} on {Name}. Please remove or rename one of the links.");
            }
            this.clientLinks.Add(linkDefinition);
        }

        /// <summary>
        /// The name of the class for this endpoint.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// The json schema for the type returned by this endpoint.
        /// </summary>
        public JsonSchema4 Schema { get; set; }

        /// <summary>
        /// True if this endpoint is considered an entry point to the service.
        /// </summary>
        public bool IsEntryPoint { get; set; }

        /// <summary>
        /// Returns true if the view represented is a collection.
        /// </summary>
        public bool IsCollectionView { get; set; }

        /// <summary>
        /// Get the type of items in this view's collection.
        /// </summary>
        public String CollectionType { get; set; }

        /// <summary>
        /// The potential links this view will include.
        /// </summary>
        public IEnumerable<EndpointClientLinkDefinition> Links
        {
            get
            {
                return clientLinks;
            }
        }

        private static String GetCollectionType(Type type, bool isCollectionView)
        {
            if (!isCollectionView)
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
}
