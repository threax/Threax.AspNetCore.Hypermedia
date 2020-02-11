using System;
using System.Collections.Generic;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Client.OpenIdConnect
{
    /// <summary>
    /// This class can be injected with an object to make sure the version set up for
    /// bearer tokens is always returned.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BearerToken<T> : IDisposable
    {
        public BearerToken(T wrapped)
        {
            this.Wrapped = wrapped;
        }

        public void Dispose()
        {
            (this.Wrapped as IDisposable)?.Dispose();
        }

        public T Wrapped { get; private set; }
    }
}
