using System.Runtime.Serialization;

namespace WebApi.Utilities
{
    public class HttpClientException : Exception
    {
        public HttpResponseMessage? HttpResponseMessage { get; set; }

        protected HttpClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

        public HttpClientException(System.Net.HttpStatusCode httpStatusCode, string message)
        {
            this.HttpResponseMessage = new HttpResponseMessage()
            {
                StatusCode = httpStatusCode,
                Content = new StringContent(message, System.Text.Encoding.UTF8, "application/json")
            };
        }


    }
}
