using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Ext.ValueProviders
{
    class EnumLabelValuePairProvider : LabelValuePairProviderSync
    {
        Type enumType;
        bool nullable;
        PropertyInfo propertyInfo;

        public EnumLabelValuePairProvider(Type enumType, PropertyInfo propertyInfo, bool nullable)
        {
            this.enumType = enumType;
            if (!enumType.GetTypeInfo().IsEnum)
            {
                throw new InvalidOperationException($"Cannot get enum values for type that is not an enum {enumType.FullName}");
            }
            this.nullable = nullable;
            this.propertyInfo = propertyInfo;
        }

        protected override IEnumerable<ILabelValuePair> GetSourcesSync()
        {
            if (nullable)
            {
                //Include the null enum label since we can take null values
                var labelAttribute = propertyInfo.GetCustomAttribute<NullEnumLabelAttribute>();
                if (labelAttribute == null)
                {
                    labelAttribute = enumType.GetTypeInfo().GetCustomAttribute<NullEnumLabelAttribute>();
                    if (labelAttribute == null)
                    {
                        labelAttribute = new NullEnumLabelAttribute();
                    }
                }
                yield return new LabelValuePair()
                {
                    Label = labelAttribute.Label,
                    Value = null
                };
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
