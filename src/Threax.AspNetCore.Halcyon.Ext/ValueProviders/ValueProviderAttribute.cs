using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Ext.ValueProviders
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValueProviderAttribute : Attribute
    {
        public ValueProviderAttribute(Type valueProvider)
        {
            this.ProviderType = valueProvider;
        }

        public Type ProviderType { get; private set; }

        /// <summary>
        /// The property name to put this value under in the schema, should start with x- since it will be an extension.
        /// Defaults to null, which means use the ValueProvider default name.
        /// </summary>
        public String PropertyName { get; set; } = null;
    }
}
