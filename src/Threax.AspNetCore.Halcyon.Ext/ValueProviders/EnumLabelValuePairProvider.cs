﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Threax.NJsonSchema;

namespace Threax.AspNetCore.Halcyon.Ext.ValueProviders
{
    public class EnumLabelValuePairProvider
    {
        Type enumType;

        public EnumLabelValuePairProvider(Type enumType)
        {
            this.enumType = enumType;
            if (!enumType.GetTypeInfo().IsEnum)
            {
                throw new InvalidOperationException($"Cannot get enum values for type that is not an enum {enumType.FullName}");
            }
        }

        public Task AddExtensions(JsonSchema4 schema, ValueProviderArgs args)
        {
            var sources = GetSourcesSync(args);

            foreach(var source in sources)
            {
                schema.EnumerationNames.Add(source.Label);
                schema.Enumeration.Add(source.Value);
            }

            if (schema.ExtensionData == null)
            {
                schema.ExtensionData = new Dictionary<String, Object>();
            }

            return Task.FromResult(0);
        }

        protected virtual IEnumerable<LabelValuePair> GetSourcesSync(ValueProviderArgs args)
        {
            if (args.IsNullable)
            {
                NullValueLabelAttribute nullLabel = args.PropertyInfo.GetCustomAttribute<NullValueLabelAttribute>();

                if (nullLabel == null)
                {
                    nullLabel = enumType.GetTypeInfo().GetCustomAttribute<NullValueLabelAttribute>();
                }

                if (nullLabel == null)
                {
                    nullLabel = new NullValueLabelAttribute();
                }

                if (nullLabel.IncludeNullValueLabel)
                {
                    yield return new LabelValuePair()
                    {
                        Label = nullLabel.Label,
                        Value = null
                    };
                }
            }

            foreach (var member in enumType.GetTypeInfo().DeclaredFields.Where(i => i.IsStatic)) //The static decalared fields are our enum values
            {
                var label = member.Name;
                var display = member.GetCustomAttribute<DisplayAttribute>();
                if(display != null)
                {
                    label = display.Name;
                }
                yield return new LabelValuePair()
                {
                    Label = label,
                    Value = member.Name
                };
            }
        }
    }
}
