using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Domain.Exceptions.ProblemDetail
{
    public class InternalServerErrorProblemDetails : ProblemDetails
    {
        public InternalServerErrorProblemDetails()
        {
            Title = "Internal Server Error";
            Detail = "Internal Server Error";
            Status = StatusCodes.Status400BadRequest;
            Type = "https://example.com/probs/business"; //Bu link içerisinde bu hatayı dokümante ettiğini varsayalım.
            Instance = "";
        }
        public InternalServerErrorProblemDetails(string message)
        {
            Title = "Internal Server Error";
            Detail = message;
            Status = StatusCodes.Status500InternalServerError;
            Type = "https://example.com/probs/internal"; 
            Instance = "";
        }
    }
}
