using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Domain.Exceptions.ProblemDetail;

public static class ProblemDetailExtensions
{
    public static string AsJson<TProblemDetail>(this TProblemDetail details) where TProblemDetail : ProblemDetails
        => JsonSerializer.Serialize(details);
}
