using System.Net;

namespace Domain.Exceptions;

public class HttpExceptionCustom : Exception
{
    public HttpStatusCode StatusCode { get; private set; }

    public HttpExceptionCustom(HttpStatusCode statusCode)
        => StatusCode = statusCode;
    
    public HttpExceptionCustom(HttpStatusCode statusCode, string message)
        : base(message)
        => StatusCode = statusCode;
    
    public HttpExceptionCustom(HttpStatusCode statusCode, string message, Exception inner)
        : base(message, inner)
        => StatusCode = statusCode;
}