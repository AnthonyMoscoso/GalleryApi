using Core.Utilities.Exceptions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using MyStreamManagerFunctions.Middleware.Modesl;
using System.Net;

namespace SaalDigitalFunctions.Middleware.Handlers.Errors
{
    public class GlobalHandlerException : IFunctionsWorkerMiddleware
    {

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next(context);
            }

            catch (Exception exception)
            {
                HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
                while (exception is AggregateException aggregateException)
                {
                    exception = aggregateException.InnerExceptions.First();
                }
                switch (exception)
                {
                    case BadRequestException:
                        statusCode = HttpStatusCode.BadRequest; 
                        break;
                    case NotFoundException:
                        statusCode = HttpStatusCode.NotFound;
                        break;
                    case ForbiddenException:
                        statusCode = HttpStatusCode.Forbidden;
                        break;
                    case UnauthorizedAccessException:
                        statusCode = HttpStatusCode.Unauthorized;
                        break;
                }

                string message = exception?.InnerException?.InnerException?.Message ?? exception?.InnerException?.Message ?? exception?.Message ?? string.Empty;
                await HandleExceptionAsync(context, message, statusCode);
            }
        }

        private static async Task HandleExceptionAsync(FunctionContext context, string message, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError)
        {
            HttpRequestData? httpReqData = await context.GetHttpRequestDataAsync();
            if (httpReqData != null)
            {
                HttpResponseData newHttpResponse = httpReqData.CreateResponse(httpStatusCode);
                context.GetInvocationResult().Value = newHttpResponse;
                ErrorDetails errorDetails = new()
                {
                    StatusCode = (int)newHttpResponse.StatusCode,
                    Message = message
                };
                await newHttpResponse.WriteAsJsonAsync(errorDetails, newHttpResponse.StatusCode);

                // Update invocation result.
                var invocationResult = context.GetInvocationResult();
                invocationResult.Value = newHttpResponse;

            }
        }
    }
}
