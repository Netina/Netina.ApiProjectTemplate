using Infrastructure.Models.RestApi.KaveNegar;
using Refit;

namespace Server.WebFramework.MiddleWares;


public static class ExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder applicationBuilder)
    {
        return applicationBuilder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}
public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(
        RequestDelegate next,
        IWebHostEnvironment env,
        ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _env = env;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        string message = null;
        HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
        ApiResultStatusCode apiStatusCode = ApiResultStatusCode.ServerError;

        try
        {
            await _next(context);
        }
        catch (BaseApiException exception)
        {
            _logger.LogError(exception, exception.Message);
            httpStatusCode = exception.HttpStatusCode;
            apiStatusCode = exception.ApiStatusCode;

            if (_env.IsDevelopment())
            {
                var dic = new Dictionary<string, string>
                {
                    ["Exception"] = exception.Message,
                    ["StackTrace"] = exception.StackTrace,
                };
                if (exception.InnerException != null)
                {
                    dic.Add("InnerException.Exception", exception.InnerException.Message);
                    dic.Add("InnerException.StackTrace", exception.InnerException.StackTrace);
                }

                if (exception.AdditionalData != null)
                    dic.Add("AdditionalData", JsonConvert.SerializeObject(exception.AdditionalData));
                DefaultContractResolver contractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };

                message = JsonConvert.SerializeObject(dic, new JsonSerializerSettings
                {
                    ContractResolver = contractResolver,
                    Formatting = Formatting.Indented
                });
            }
            else
            {
                message = exception.Message;
            }

            await WriteToResponseAsync();
        }
        catch (SecurityTokenExpiredException exception)
        {
            _logger.LogError(exception, exception.Message);
            SetUnAuthorizeResponse(exception);
            await WriteToResponseAsync();
        }
        catch (UnauthorizedAccessException exception)
        {
            _logger.LogError(exception, exception.Message);
            SetUnAuthorizeResponse(exception);
            await WriteToResponseAsync();
        }
        catch (ApiException exception)
        {
            _logger.LogError(exception, exception.Message);
            var kaveNegarResponse = await exception.GetContentAsAsync<KaveNegarResponse>();
            if (kaveNegarResponse != null)
                message = kaveNegarResponse.Return.message;
            else
                message = "در ارسال درخواست به سرورهای دیگر مشکلی رخ داده است";
            httpStatusCode = HttpStatusCode.InternalServerError;
            apiStatusCode = ApiResultStatusCode.RefitError;
            await WriteToResponseAsync();


        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            if (_env.IsDevelopment())
            {
                var dic = new Dictionary<string, string>
                {
                    ["Exception"] = exception.Message,
                    ["InnerException"] = exception.InnerException?.Message,
                    ["StackTrace"] = exception.StackTrace,
                };
                message = JsonConvert.SerializeObject(dic);
            }

            message = exception.Message;
            await WriteToResponseAsync();
        }

        async Task WriteToResponseAsync()
        {
            if (context.Response.HasStarted)
                throw new InvalidOperationException("The response has already started, the http status code middleware will not be executed.");

            var result = new ApiResult(false, apiStatusCode, message);

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            string json = JsonConvert.SerializeObject(result, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });

            context.Response.StatusCode = (int)httpStatusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(json);
        }

        void SetUnAuthorizeResponse(Exception exception)
        {
            httpStatusCode = HttpStatusCode.Unauthorized;
            apiStatusCode = ApiResultStatusCode.UnAuthorized;

            if (_env.IsDevelopment())
            {
                var dic = new Dictionary<string, string>
                {
                    ["Exception"] = exception.Message,
                    ["StackTrace"] = exception.StackTrace
                };
                if (exception is SecurityTokenExpiredException tokenException)
                    dic.Add("Expires", tokenException.Expires.ToString());

                message = JsonConvert.SerializeObject(dic);
            }
        }

        JwtSecurityToken ReadJwtToken(bool fromHeader = true)
        {
            try
            {
                if (fromHeader)
                {
                    var stream = context.Request.Headers.Values.First(v => v.FirstOrDefault().Contains("Bearer")).FirstOrDefault();
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(stream.Split(" ").Last());
                    return jsonToken as JwtSecurityToken;
                }
                else
                {
                    string stream = context.Request.Query["access_token"]; ;
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(stream.Split(" ").Last());
                    return jsonToken as JwtSecurityToken;
                }
            }
            catch (Exception e)
            {
                throw new BaseApiException(ApiResultStatusCode.UnAuthorized, e.Message + " Jwt is wrong", HttpStatusCode.Unauthorized);
            }
        }
    }

}