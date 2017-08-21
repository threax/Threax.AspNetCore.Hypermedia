using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Client
{
    public class FormContentBuilder
    {
        public static void BuildFormContent(Object source, MultipartFormDataContent form)
        {
            var type = source.GetType();
            var typeInfo = type.GetTypeInfo();

            foreach (var prop in typeInfo.DeclaredProperties)
            {
                bool addAsString = true;
                var value = prop.GetValue(source);
                if (value.GetType() != typeof(String))
                {
                    var array = value as IEnumerable;
                    if (array != null)
                    {
                        var sb = new StringBuilder();
                        foreach (var i in array)
                        {
                            sb.Append($",{i.ToString()}");
                        }
                        form.Add(new StringContent(sb.ToString()), $"\"{prop.Name}\"");
                        addAsString = false;
                    }
                }

                var stream = value as Stream;
                if(stream != null)
                {
                    form.Add(new StreamContent(stream), $"\"{prop.Name}\"");
                    addAsString = false;
                }

                if (addAsString)
                {
                    form.Add(new StringContent(value.ToString()), $"\"{prop.Name}\"");
                }
            }
        }
    }
}
