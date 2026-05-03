using System.Net;

namespace DesktopApp.Models;

public class ApiResult<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public string Error { get; init; } = string.Empty;
    public HttpStatusCode StatusCode { get; init; }

    public static ApiResult<T> Ok(T? data, HttpStatusCode statusCode) => new()
    {
        Success = true,
        Data = data,
        StatusCode = statusCode
    };

    public static ApiResult<T> Fail(string error, HttpStatusCode statusCode) => new()
    {
        Success = false,
        Error = error,
        StatusCode = statusCode
    };
}

