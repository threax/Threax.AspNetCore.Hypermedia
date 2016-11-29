using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class HalModelViewMapper : IHalModelViewMapper
    {
        private Dictionary<Type, Func<Object, Object>> converters = new Dictionary<Type, Func<object, object>>();

        public HalModelViewMapper()
        {

        }

        public void AddConverter<T>(Func<T, Object> converter)
        {
            AddConverter(typeof(T), i => converter((T)i)); //Keep strong typing
        }

        public void AddConverter(Type t, Func<Object, Object> converter)
        {
            converters.Add(t, converter);
        }

        public void RemoveConverter<T>()
        {
            RemoveConverter(typeof(T));
        }

        public void RemoveConverter(Type t)
        {
            converters.Remove(t);
        }

        public Object Convert(Object src)
        {
            Func<Object, Object> converter = null;

            var enumerable = src as IEnumerable;
            if (enumerable != null)
            {
                //Figure out what the enumerable generic type is and look that type up for our converter.
                var elementType = Utils.GetEnumerableModelType(enumerable);
                Type srcType = typeof(IEnumerable<>).MakeGenericType(elementType);
                converters.TryGetValue(srcType, out converter);
            }
            else
            {
                //Search current type and all base types for a converter, this does not search interfaces.
                Type srcType = src.GetType();

                while (srcType != null && !converters.TryGetValue(srcType, out converter))
                {
                    srcType = srcType.GetTypeInfo().BaseType;
                }
            }

            if (converter == null)
            {
                throw new InvalidOperationException($"Cannot find converter for type {src.GetType().FullName}. Base classes were searched as well.");
            }

            return converter(src);
        }
    }
}
