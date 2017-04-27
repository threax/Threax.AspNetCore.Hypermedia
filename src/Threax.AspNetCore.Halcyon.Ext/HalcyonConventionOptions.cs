using Halcyon.HAL;
using Newtonsoft.Json;
using NJsonSchema.Generation;
using NJsonSchema.Generation.TypeMappers;
using System;
using System.Collections.Generic;
using Threax.AspNetCore.Halcyon.Ext;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Options for the Halcyon convention.
    /// </summary>
    public class HalcyonConventionOptions
    {
        public const String HostVariable = "{{host}}";

        /// <summary>
        /// Set the base url to use when creating links. You can add the variable {{host}} to the string
        /// to replace that path with the current Request.GetDisplayUrl().Authority value. Otherwise you can
        /// set the url to whatever you want.
        /// </summary>
        public string BaseUrl { get; set; } = HostVariable;

        /// <summary>
        /// Provides info about the controller endpoint that exposes documentation. Can be null to not support documentation.
        /// </summary>
        public IHalDocEndpointInfo HalDocEndpointInfo { get; set; } = null;

        /// <summary>
        /// Custom json serializer settings, if needed.
        /// </summary>
        public JsonSerializerSettings JsonSerializerSettings { get; set; } = HalcyonConvention.DefaultJsonSerializerSettings;

        /// <summary>
        /// Custom json schema generator settings if needed, keep it synchronized with the JsonSerializerSettings in terms
        /// of case and enum handling.
        /// </summary>
        public JsonSchemaGeneratorSettings JsonSchemaGeneratorSettings { get; set; } = HalcyonConvention.DefaultJsonSchemaGeneratorSettings;

        /// <summary>
        /// When this is true (default) the HalModelResultFilterAttribute and ProducesHalAttribute will be added to all controllers.
        /// Otherwise you must manually add these attributes to controllers that send hal results.
        /// </summary>
        public bool MakeAllControllersHalcyon { get; set; } = true;
    }
}