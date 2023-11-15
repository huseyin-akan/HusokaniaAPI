using FluentValidation;
using MiniETrade.Application.Common.Abstractions.Localization;
using MiniETrade.Domain.Exceptions;
using MiniETrade.Domain.Exceptions.ProblemDetail;
using MiniETrade.Domain.Messages;
using System.Net;
using ValidationProblemDetails = MiniETrade.Domain.Exceptions.ProblemDetail.ValidationProblemDetails;

namespace MiniETrade.API.Middlewares.ExceptionHandling;

public class GlobalExceptionHandler : IMiddleware 
{
    private readonly ILanguageService _languageService;

    public GlobalExceptionHandler(ILanguageService languageService)
    {
        _languageService = languageService;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            context.Response.ContentType = "application/json";

            _ = exception switch
            {
                BusinessException businessException => HandleException(context, businessException),
                ValidationException validationException => HandleException(context, validationException),
                UnAuthorizedException unauthorizedException => HandleException(context, unauthorizedException),
                UserCreateFailedException userCreateFailedException => HandleException(context, userCreateFailedException),
                _ => HandleException(context)
            };

            //TODO-HUS log the exception.
        }
    }

    private Task HandleException(HttpContext context, BusinessException exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        var translatedMessage = HandleMessageLanguages(exception.Message);
        var problemDetail = new BusinessProblemDetails(translatedMessage).AsJson();
        return context.Response.WriteAsync(problemDetail);
    }

    private static Task HandleException(HttpContext context, ValidationException exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        var problemDetail = new ValidationProblemDetails(exception.Errors).AsJson();
        return context.Response.WriteAsync(problemDetail);
    }

    private Task HandleException(HttpContext context, UnAuthorizedException exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        var translatedMessage = HandleMessageLanguages(exception.Message);
        var problemDetail = new BusinessProblemDetails(translatedMessage).AsJson();
        return context.Response.WriteAsync(problemDetail);
    }

    private Task HandleException(HttpContext context, UserCreateFailedException exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest ;
        var translatedMessage = HandleMessageLanguages(AppMessages.UserCreateFailed);
        var problemDetail = new BusinessProblemDetails(translatedMessage).AsJson();
        return context.Response.WriteAsync(problemDetail);
    }

    private Task HandleException(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        var unknownProblemDetail = new InternalServerErrorProblemDetails().AsJson();
        return context.Response.WriteAsync(unknownProblemDetail);
    }

    private string HandleMessageLanguages(string key)
    {
       return _languageService.GetKey(key);
    }
}