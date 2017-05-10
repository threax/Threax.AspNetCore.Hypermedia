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
        TypeInfo enumTypeInfo;

        public EnumLabelValuePairProvider(TypeInfo enumTypeInfo)
        {
            this.enumTypeInfo = enumTypeInfo;
        }

        protected override IEnumerable<LabelValuePair> GetSourcesSync()
        {
            foreach (var member in enumTypeInfo.DeclaredFields.Where(i => i.IsStatic)) //The static decalared fields are our enum values
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
