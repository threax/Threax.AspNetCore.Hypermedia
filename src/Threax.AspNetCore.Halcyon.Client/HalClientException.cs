using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Halcyon.Client
{
    public class HalRemoteException : InvalidOperationException
    {
        public static async Task<HalRemoteException> Create(HttpResponseMessage response)
        {
            try
            {
                //Try to read the response as a string
                var responseString = await response.Content.ReadAsStringAsync();
                if (responseString == null)
                {
                    return new HalRemoteException($"An unknown server error occured with error code: {(int)response.StatusCode}", response.StatusCode, responseString, null);
                }
                try
                {
                    //Try to read the message as an understandable error.
                    var responseData = JObject.Parse(responseString);
                    var error = responseData["message"]?.Value<String>() ?? $"An unknown server error occured with error code: {(int)response.StatusCode}";
                    var exception = new HalRemoteException(error, response.StatusCode, responseString, responseData);
                    return exception;
                }
                catch (Exception)
                {
                    return new HalRemoteException($"An unknown server error occured with error code: {(int)response.StatusCode}", response.StatusCode, responseString, null);
                }
            }
            catch (Exception)
            {
                return new HalRemoteException($"An unknown server error occured with error code: {(int)response.StatusCode}", response.StatusCode, null, null);
            }
        }

        protected HalRemoteException(String message, HttpStatusCode statusCode, String errorString, JToken errorData) : base(message)
        {
            this.ErrorString = errorString;
            this.StatusCode = statusCode;
            this.ErrorData = errorData;
        }

        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// The raw error string from the server. Will be null if no string could be read.
        /// </summary>
        public String ErrorString { get; set; }

        /// <summary>
        /// The error data as a JToken. Will be null if no data could be parsed.
        /// </summary>
        public JToken ErrorData { get; private set; }

        /// <summary>
        /// Get a particular validation error. This will only be possible if ErrorData is not null.
        /// </summary>
        /// <param name="name">The name of the validation error to lookup.</param>
        /// <returns></returns>
        public String GetValidationError(String name)
        {
            if (HasValidationErrors)
            {
                return ErrorData["errors"][name]?.Value<String>();
            }
            return null;
        }

        /// <summary>
        /// Returns true if there is a validation error with the given name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public bool HasValidationError(String name)
        {
            return HasValidationErrors && ErrorData["errors"][name] != null;
        }

        /// <summary>
        /// Returns true if any validation errors can be parsed from the ErrorData.
        /// </summary>
        public bool HasValidationErrors
        {
            get => ErrorData != null && ErrorData["errors"] != null;
        }

        public IEnumerable<KeyValuePair<String, String>> AllValidationErrors
        {
            get
            {
                return GetAllValidationErrors();
            }
        }

        private IEnumerable<KeyValuePair<String, String>> GetAllValidationErrors()
        {
            if (!HasValidationErrors)
            {
                yield break;
            }

            var jObjErrors = ErrorData["errors"] as JObject;
            if (jObjErrors == null)
            {
                yield break;
            }

            foreach (var error in jObjErrors.Properties())
            {
                yield return new KeyValuePair<string, string>(error.Name, error.Value?.Value<String>());
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder(this.GetType().Name);
            sb.AppendLine();
            sb.AppendFormat("   Message: {0}", Message);
            sb.AppendLine();
            sb.AppendFormat("   Status: {0}", (int)StatusCode);
            sb.AppendLine();
            if (ErrorString != null)
            {
                sb.AppendFormat("   Error Data: {0}", ErrorString);
                sb.AppendLine();
            }
            sb.AppendFormat("   Stack Trace: {0}", this.StackTrace);
            sb.AppendLine();
            if(this.InnerException != null)
            {
                sb.AppendLine("---Begin InnerException---");
                sb.AppendLine(this.InnerException.ToString());
                sb.AppendLine("---End InnerException---");
            }
            return sb.ToString();
        }
    }
}
