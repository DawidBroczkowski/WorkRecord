using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace WorkRecord.API
{
    public static class ExceptionHandlerExtensions
    {
        public static WebApplication ConfigureGlobalExceptionHandling(this WebApplication app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerFeature>()!.Error;
                    var problemDetails = new ValidationProblemDetails();

                    if (exception is ValidationException validationException)
                    {
                        problemDetails.Title = "One or more validation errors occurred.";
                        problemDetails.Status = (int)HttpStatusCode.BadRequest;
                        appendData(problemDetails, exception);
                    }
                    //else if (exception is AuthorizationException authorizationException)
                    //{
                    //    problemDetails.Title = "One or more authorization errors occurred.";
                    //    problemDetails.Status = (int)HttpStatusCode.Forbidden;
                    //    appendData(problemDetails, exception);
                    //}
                    else if (exception is KeyNotFoundException keyNotFoundException)
                    {
                        problemDetails.Title = "One or more key-not-found errors occurred.";
                        problemDetails.Status = (int)HttpStatusCode.NotFound;
                        appendData(problemDetails, exception);

                    }
                    else if (exception is FileNotFoundException fileNotFoundException)
                    {
                        problemDetails.Title = "One or more file-not-found errors occurred.";
                        problemDetails.Status = (int)HttpStatusCode.NotFound;
                        appendData(problemDetails, exception);
                    }
                    else
                    {
                        problemDetails.Title = "An unexpected error occurred.";
                        problemDetails.Status = (int)HttpStatusCode.InternalServerError;
                        problemDetails.Detail = exception.Message;
                    }

                    context.Response.ContentType = "application/problem+json";
                    context.Response.StatusCode = problemDetails.Status!.Value;

                    await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
                });
            });

            return app;
        }

        private static void appendData(ValidationProblemDetails problemDetails, Exception exception)
        {
            problemDetails.Detail = exception.Message;
            foreach (var key in exception.Data.Keys)
            {
                var value = exception.Data[key];
                if (value is string errorMessage)
                {
                    problemDetails.Errors.Add(key.ToString()!, new[] { errorMessage });
                }
            }
        }
    }
}
