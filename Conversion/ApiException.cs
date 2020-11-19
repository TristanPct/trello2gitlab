using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Trello2GitLab.Conversion
{
    public class ApiException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; }

        public HttpResponseHeaders HttpResponseHeaders { get; }

        public string Details { get; }

        internal ApiException(HttpResponseMessage response, string details)
            : this(response, details, null)
        { }

        internal ApiException(HttpResponseMessage response, string details, Exception innerException)
            : base($"{(int)response.StatusCode} {response.ReasonPhrase} {details}", innerException)
        {
            HttpStatusCode = response.StatusCode;
            HttpResponseHeaders = response.Headers;
            Details = details;
        }

        protected ApiException() { }

        protected ApiException(string message) : base(message) { }

        protected ApiException(string message, Exception innerException) : base(message, innerException) { }
    }
}
