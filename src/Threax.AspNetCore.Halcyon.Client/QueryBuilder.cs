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
            if (dictionary != null)
            {
                BuildQueryStringFromDictionary(dictionary, query);
            }
            else
            {
                BuildQueryStringFromObject(source, query);
            }

            return query.ToString();
        }

        private static void BuildQueryStringFromDictionary(Dictionary<String, Object> source, StringBuilder query)
        {
            var leading = "";
            foreach (var prop in source)
            {
                if (prop.Value != null)
                {
                    WriteValue(query, prop.Key, prop.Value, leading);
                    leading = "&";
                }
            }
        }

        private static void BuildQueryStringFromObject(Object source, StringBuilder query)
        {
            var type = source.GetType();
            var typeInfo = type.GetTypeInfo();
            var leading = "";

            foreach (var prop in typeInfo.GetProperties())
            {
                var value = prop.GetValue(source);
                if (value != null)
                {
                    WriteValue(query, prop.Name, value, leading);
                    leading = "&";
                }
            }
        }

        private static void WriteValue(StringBuilder query, String name, object value, String leading)
        {
            bool notSpecial = true;

            if (value.GetType() != typeof(String))
            {
                var array = value as IEnumerable;
                if (array != null)
                {
                    foreach (var i in array)
                    {
                        query.Append($"{leading}{Uri.EscapeUriString(name)}={Uri.EscapeUriString(i.ToString())}");
                    }
                    notSpecial = false;
                }
            }

            if (notSpecial)
            {
                query.Append($"{leading}{Uri.EscapeUriString(name)}={Uri.EscapeUriString(value.ToString())}");
            }
        }
    }
}
