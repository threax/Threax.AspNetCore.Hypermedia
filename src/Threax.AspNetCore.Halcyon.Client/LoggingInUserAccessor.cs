using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Threax.AspNetCore.Halcyon.Client
{
    /// <summary>
    /// This interface provides a way to pass the user from a logging in context to a consumer.
    /// This is used by the AddUserTokenHttpClientFactory to get access to the user while they
    /// are still logging in. This way you can use generated halcyon clients while building users.
    /// </summary>
    public interface ILoggingInUserAccessor
    {
        /// <summary>
        /// The claims principal for the logging in user. Can be null.
        /// </summary>
        ClaimsPrincipal Principal { get; set; }
    }

    public class LoggingInUserAccessor : ILoggingInUserAccessor
    {
        public ClaimsPrincipal Principal { get; set; }
    }
}
