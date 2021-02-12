using System;
using System.Net;
using System.Runtime.Serialization;

namespace LaunchPad.Client.Helpers
{
    [Serializable]
    internal class HttpRequestExceptionEx : Exception
    {
        private HttpStatusCode statusCode;
        private string content;

        public HttpRequestExceptionEx()
        {
        }

        public HttpRequestExceptionEx(string message) : base(message)
        {
        }

        public HttpRequestExceptionEx(HttpStatusCode statusCode, string content)
        {
            this.statusCode = statusCode;
            this.content = content;
        }

        public HttpRequestExceptionEx(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected HttpRequestExceptionEx(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}