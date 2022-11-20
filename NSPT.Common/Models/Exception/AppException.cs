
using Common.Models.Api;
using System.Runtime.Serialization;

namespace Common.Models.Exception;
[Serializable()]
public class AppException : System.Exception
{
    protected AppException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    public ApiResultStatusCode StatusCode { get; set; }
    public AppException()
    {
        StatusCode = ApiResultStatusCode.ServerError;
    }
    public AppException(string message) : base(message)
    {
        StatusCode = ApiResultStatusCode.ServerError;
    }

    public AppException(string message, ApiResultStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}

