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
            var dictionary = source as Dictionary<String, Object>;
            if (dictionary != null)
            {
                BuildFormContentFromObject(dictionary, form);
            }
            else
            {
                BuildFormContentFromObject(source, form);
            }
        }

        private static void BuildFormContentFromObject(Dictionary<String, Object> source, MultipartFormDataContent form)
        {
            foreach (var prop in source)
            {
                WriteValue(form, prop.Value, prop.Key);
            }
        }

        private static void BuildFormContentFromObject(Object source, MultipartFormDataContent form)
        {
            var type = source.GetType();
            var typeInfo = type.GetTypeInfo();

            foreach (var prop in typeInfo.GetProperties())
            {
                var value = prop.GetValue(source);
                var name = prop.Name;
                WriteValue(form, value, name);
            }
        }

        private static void WriteValue(MultipartFormDataContent form, object value, string name)
        {
            bool addAsString = true;
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
                    form.Add(new StringContent(sb.ToString()), $"\"{name}\"");
                    addAsString = false;
                }
            }

            var stream = value as Stream;
            if (stream != null)
            {
                form.Add(new StreamContent(stream), $"\"{name}\"");
                addAsString = false;
            }

            if (addAsString)
            {
                form.Add(new StringContent(value.ToString()), $"\"{name}\"");
            }
        }
    }
}
