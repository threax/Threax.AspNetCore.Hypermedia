using Halcyon.HAL;
using Halcyon.HAL.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class CustomHalAttributeConverter : IHALConverter
    {
        public bool CanConvert(Type type)
        {
            if (type == null || type == typeof(HALResponse))
            {
                return false;
            }

            return type.GetTypeInfo().GetCustomAttributes().Any(x => x is HalModelAttribute);
        }

        public HALResponse Convert(object model)
        {
            if (!this.CanConvert(model?.GetType()))
            {
                throw new InvalidOperationException();
            }

            //If the object is an ICollectionView, use that to parse.
            var dataCollection = model as ICollectionView;
            if(dataCollection != null)
            {
                var itemType = dataCollection.CollectionType;
                var response = ConvertInstance(model);
                response.AddEmbeddedCollection(dataCollection.CollectionName, GetEmbeddedResponses(dataCollection.AsObjects));
                return response;
            }

            //If the object is an IEnumerable try to identify properties from that.
            var enumerableValue = model as IEnumerable;
            if (enumerableValue != null)
            {
                var itemType = GetModelType(model);
                var response = new HALResponse(new Object());
                response.AddEmbeddedCollection("values", GetEmbeddedResponses(enumerableValue));
                return response;
            }

            //If we got here we probably have a plain object, convert and return it.
            return ConvertInstance(model);
        }

        private static Type GetModelType(object model)
        {
            var modelType = model.GetType();
            //The LinkFinders can be built somewhere else not per request, they are intended to be reusable.
            var newModelType = modelType.GetElementType();
            if (newModelType == null)
            {
                if (modelType.GenericTypeArguments.Length > 1 && typeof(IDictionary).IsAssignableFrom(modelType))
                {
                    newModelType = modelType.GenericTypeArguments[1];
                }
                else if (modelType.GenericTypeArguments.Length > 0)
                {
                    newModelType = modelType.GenericTypeArguments[0];
                }
            }
            if (newModelType != null)
            {
                modelType = newModelType;
            }
            return modelType;
        }

        private static HALResponse ConvertInstance(object model)
        {
            var resolver = new HALAttributeResolver();

            var halConfig = resolver.GetConfig(model);

            var response = new HALResponse(model, halConfig);
            response.AddLinks(resolver.GetLinks(model));
            response.AddEmbeddedCollections(resolver.GetEmbeddedCollections(model, halConfig));

            return response;
        }

        private static IEnumerable<HALResponse> GetEmbeddedResponses(IEnumerable enumerableValue)
        {
            foreach (var item in enumerableValue)
            {
                yield return ConvertInstance(item);
            }
        }
    }
}
