using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Domain.Exceptions.ProblemDetail
{
    public class ValidationProblemDetails : ProblemDetails
    {
        public IEnumerable<ValidationFailure>? Errors { get; set; }

        private ValidationProblemDetails()
        {
            Type = "https://example.com/probs/validation";
            Title = "Validation error(s)";
            Instance = "";
            Status = StatusCodes.Status400BadRequest;
        }

        public ValidationProblemDetails(IEnumerable<ValidationFailure> errors) : this()
        {
            Errors = errors;
            Detail = "Validation error(s)";
        }

        public ValidationProblemDetails(IEnumerable<ValidationFailure> errors, string message) : this(errors)
        {
            Detail = message;
        }
        public ValidationProblemDetails(string message) : this()
        {
            Detail = message;
        }
    }
}