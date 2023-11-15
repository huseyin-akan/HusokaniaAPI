using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Exceptions.ProblemDetail;

public class BusinessProblemDetails : ProblemDetails
{
    public BusinessProblemDetails()
    {}
    public BusinessProblemDetails(string detail)
    {
        Title = "Business Rule Violation";
        Detail = detail;
        Status = StatusCodes.Status400BadRequest;
        Type = "https://example.com/probs/business"; //Burada bu hatayı dokümante ettiğini varsayalım.
        Instance = "";
    }
}
