using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Application.Validation;

public class FluentValidationMiddleware
{
    private readonly RequestDelegate _next;

    public FluentValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await HandleValidationExceptionAsync(context, ex);
        }
    }

    private Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        var errors = exception.Errors.Select(error => new
        {
            error.PropertyName,
            error.ErrorMessage
        });

        var response = new
        {
            Message = "Validation failed",
            Errors = errors
        };

        return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}