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
            var type = source.GetType();
            var typeInfo = type.GetTypeInfo();

            StringBuilder query = new StringBuilder();

            foreach(var prop in typeInfo.DeclaredProperties)
            {
                bool notSpecial = true;
                var value = prop.GetValue(source);
                if(value.GetType() != typeof(String))
                {
                    var array = value as IEnumerable;
                    if (array != null)
                    {
                        foreach (var i in array)
                        {
                            query.Append($"&{Uri.EscapeUriString(prop.Name)}[]={Uri.EscapeUriString(i.ToString())}");
                        }
                        notSpecial = false;
                    }
                }

                if(notSpecial)
                {
                    query.Append($"&{Uri.EscapeUriString(prop.Name)}={Uri.EscapeUriString(value.ToString())}");
                }
            }

            if (query.Length > 0)
            {
                query[0] = '?';
            }

            return query.ToString();
        }
    }
}
