
using System.Net;

namespace Domain.Messages;

public static class ExceptionMessage
{
    public const HttpStatusCode ERROR_500 = HttpStatusCode.InternalServerError;
    public const string ERROR_500_MSG = "Interna Server Error";

    public const HttpStatusCode ERROR_404 = HttpStatusCode.NotFound;
    public const string ERROR_404_MSG = "Record not found";

    public const HttpStatusCode ERROR_403 = HttpStatusCode.Forbidden;
    public const string ERROR_403_MSG = "Operation refused due to business rules";

    public const HttpStatusCode ERROR_415 = HttpStatusCode.UnsupportedMediaType;
    public const string ERROR_415_MSG = "Unsupported Media Type";
}

