namespace Threax.SharedHttpClient
{
    /// <summary>
    /// Options for the shared client.
    /// </summary>
    public class SharedHttpClientOptions
    {
        /// <summary>
        /// If this is set to true HttpClient will be registered directly into services alongside SharedHttpClient.
        /// This is useful if you have a third party library that directly requires HttpClient, however, you need to
        /// be careful that this shared instance will work correctly in your app. Most of the time it will be better
        /// to keep this at false and inject the SharedHttpClient instead. This way your code will work with as many
        /// libraries as possible. Default: false.
        /// </summary>
        public bool RegisterHttpClientDirectly { get; set; } = false;
    }
}