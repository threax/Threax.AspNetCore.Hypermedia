using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Client
{
    public class QueryBuilder
    {
        public static String BuildQueryString(Object source)
        {
            StringBuilder query = new StringBuilder();

            var dictionary = source as Dictionary<String, Object>;
            if(dictionary != null)
            {
                BuildQueryStringFromDictionary(dictionary, query);
            }
            else
            {
                BuildQueryStringFromObject(source, query);
            }

            if (query.Length > 0)
            {
                query[0] = '?';
            }

            return query.ToString();
        }

        private static void BuildQueryStringFromDictionary(Dictionary<String, Object> source, StringBuilder query)
        {
            foreach (var prop in source)
            {
                WriteValue(query, prop.Key, prop.Value);
            }
        }

        private static void BuildQueryStringFromObject(Object source, StringBuilder query)
        {
            var type = source.GetType();
            var typeInfo = type.GetTypeInfo();

            foreach(var prop in typeInfo.GetProperties())
            {
                WriteValue(query, prop.Name, prop.GetValue(source));
            }
        }

        private static void WriteValue(StringBuilder query, String name, object value)
        {
            bool notSpecial = true;
            if (value != null)
            {
                if (value.GetType() != typeof(String))
                {
                    var array = value as IEnumerable;
                    if (array != null)
                    {
                        foreach (var i in array)
                        {
                            query.Append($"&{Uri.EscapeUriString(name)}={Uri.EscapeUriString(i.ToString())}");
                        }
                        notSpecial = false;
                    }
                }

                if (notSpecial)
                {
                    query.Append($"&{Uri.EscapeUriString(name)}={Uri.EscapeUriString(value.ToString())}");
                }
            }
        }
    }
}
