using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Middleware;

public class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ApiExceptionMiddleware(RequestDelegate next,
        ILogger<ApiExceptionMiddleware> logger,
        IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {    
            await _next(context);
        }
        catch (HttpExceptionCustom ex)
        {
            HttpExceptionCustom _ex = (HttpExceptionCustom)ex;

            _logger.LogError(_ex, _ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)_ex.StatusCode;

            var result = _env.IsDevelopment() ?
                new ExceptionMiddlewareBase
                {
                    StatusCode = context.Response.StatusCode,
                    Message = _ex.Message,
                    Detail = _ex.StackTrace,
                } : new ExceptionMiddlewareBase
                {
                    StatusCode = context.Response.StatusCode,
                    Message = _ex.Message
                };

            await context.Response.WriteAsJsonAsync(result);
        }
    }
}

internal class ExceptionMiddlewareBase {
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
}