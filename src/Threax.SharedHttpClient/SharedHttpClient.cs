using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Threax.SharedHttpClient
{
    /// <summary>
    /// An injectable version of the shared http client. The implementation is just a container that returns
    /// the shared instance from SharedHttpClientManager.
    /// </summary>
    public interface ISharedHttpClient
    {
        HttpClient Client { get; }
    }

    /// <summary>
    /// Implementation for ISharedHttpClient.
    /// </summary>
    public class DefaultSharedHttpClient : ISharedHttpClient
    {
        public HttpClient Client { get => SharedHttpClientManager.SharedClient; }
    }
}
