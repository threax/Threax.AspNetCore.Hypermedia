using Threax.NJsonSchema;
using Threax.NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Models
{
    public class PropertyUiInfo
    {
        public PropertyUiInfo(String type)
        {
            this.Type = type;
        }

        public String Type { get; private set; }

        public String OverrideComponent { get; set; }

        public virtual String CreateAttribute()
        {
            return $@"[UiType(""{Type}""{AddSharedProperties(true)})]";
        }

        protected String AddSharedProperties(bool needComma)
        {
            var result = new StringBuilder();
            foreach(var prop in GetSharedProperties())
            {
                if (needComma)
                {
                    result.Append(", ");
                }
                result.Append(prop);
                needComma = true;
            }
            return result.ToString();
        }

        protected virtual IEnumerable<String> GetSharedProperties()
        {
            if(OverrideComponent != null)
            {
                yield return $"OverrideComponent = \"{OverrideComponent}\"";
            }
        }
    }

    /// <summary>
    /// Use this to alter the type of this a before sending the schema
    /// to the ui for processing.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UiTypeAttribute : JsonSchemaExtensionDataAttribute
    {
        internal const String Name = "xUi";

        public UiTypeAttribute(String value) : this(new PropertyUiInfo(value))
        {

        }

        public UiTypeAttribute(PropertyUiInfo options) : base(Name, options)
        {
            
        }

        public String OverrideComponent
        {
            get
            {
                return ((PropertyUiInfo)this.Value).OverrideComponent;
            }
            set
            {
                ((PropertyUiInfo)this.Value).OverrideComponent = value;
            }
        }
    }

    public static class UiTypeAttributeJsonSchemaExtensions
    {
        /// <summary>
        /// Get the ui type of this property. Will return null if no ui type has been defined.
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static PropertyUiInfo GetUiTypeInfo(this JsonProperty prop)
        {
            Object val = null;
            if (prop.ExtensionData?.TryGetValue(UiTypeAttribute.Name, out val) == true)
            {
                return (PropertyUiInfo)val;
            }
            return null;
        }
    }
}
