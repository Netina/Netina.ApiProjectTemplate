using Common.Models.Api;
using System.Net;
using System.Runtime.Serialization;

namespace Common.Models.Exception;

[Serializable()]
public class BaseApiException : System.Exception
{
    protected BaseApiException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    public BaseApiException()
        : this(ApiResultStatusCode.ServerError)
    {
    }

    public BaseApiException(ApiResultStatusCode statusCode)
        : this(statusCode, null)
    {
    }

    public BaseApiException(string message)
        : this(ApiResultStatusCode.ServerError, message)
    {
    }

    public BaseApiException(ApiResultStatusCode statusCode, string message)
        : this(statusCode, message, HttpStatusCode.InternalServerError)
    {
    }

    public BaseApiException(string message, object additionalData) : this(ApiResultStatusCode.ServerError, message, additionalData)
    {
    }

    public BaseApiException(ApiResultStatusCode statusCode, object additionalData) : this(statusCode, null, additionalData)
    {
    }

    public BaseApiException(ApiResultStatusCode statusCode, string message, object additionalData)
        : this(statusCode, message, HttpStatusCode.InternalServerError, additionalData)
    {
    }

    public BaseApiException(ApiResultStatusCode statusCode, string message, HttpStatusCode httpStatusCode)
        : this(statusCode, message, httpStatusCode, null)
    {
    }

    public BaseApiException(ApiResultStatusCode statusCode, string message, HttpStatusCode httpStatusCode, object additionalData)
        : this(statusCode, message, httpStatusCode, null, additionalData)
    {
    }

    public BaseApiException(string message, System.Exception exception)
        : this(ApiResultStatusCode.ServerError, message, exception)
    {
    }

    public BaseApiException(string message, System.Exception exception, object additionalData)
        : this(ApiResultStatusCode.ServerError, message, exception, additionalData)
    {
    }

    public BaseApiException(ApiResultStatusCode statusCode, string message, System.Exception exception)
        : this(statusCode, message, HttpStatusCode.InternalServerError, exception)
    {
    }

    public BaseApiException(ApiResultStatusCode statusCode, string message, System.Exception exception, object additionalData)
        : this(statusCode, message, HttpStatusCode.InternalServerError, exception, additionalData)
    {
    }

    public BaseApiException(ApiResultStatusCode statusCode, string message, HttpStatusCode httpStatusCode, System.Exception exception)
        : this(statusCode, message, httpStatusCode, exception, null)
    {
    }

    public BaseApiException(ApiResultStatusCode statusCode, string message, HttpStatusCode httpStatusCode, System.Exception exception, object additionalData)
        : base(message, exception)
    {
        ApiStatusCode = statusCode;
        HttpStatusCode = httpStatusCode;
        AdditionalData = additionalData;
    }

    public HttpStatusCode HttpStatusCode { get; set; }
    public ApiResultStatusCode ApiStatusCode { get; set; }
    public object AdditionalData { get; set; }
}