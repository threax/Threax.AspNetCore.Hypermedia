using Halcyon.HAL.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Ext
{
    public class IgnoreEmbedsJsonContractResolver : DefaultContractResolver
    {
        public IgnoreEmbedsJsonContractResolver()
        {
            
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);

            //Take out any properties with a HalEmbeddedAttribute. This is cached by Json.Net so it is pretty much instant after the first run.
            properties = properties.Where(p => !p.AttributeProvider.GetAttributes(typeof(HalEmbeddedAttribute), true).Any()).ToList();

            return properties;
        }
    }
}
