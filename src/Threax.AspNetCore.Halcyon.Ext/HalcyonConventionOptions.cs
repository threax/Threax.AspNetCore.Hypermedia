using Newtonsoft.Json;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Options for the Halcyon convention.
    /// </summary>
    public class HalcyonConventionMvcOptions
    {
        /// <summary>
        /// Settings for the json serializer.
        /// </summary>
        public JsonSerializerSettings JsonSerializerSettings { get; set; }
    }
}