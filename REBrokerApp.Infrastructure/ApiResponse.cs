using System.Net;

namespace REBrokerApp.Infrastructure
{
    public class ApiResponse
    {
        public ApiResponse() { }
        public ApiResponse(object data, List<string>? errors = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            StatusCode = statusCode;
            Data = data;
            Errors = errors == null ? new List<string>() : errors;
        }
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public object? Data { get; set; } = null;
        public List<string>? Errors { get; set; } = null;
    }
}
