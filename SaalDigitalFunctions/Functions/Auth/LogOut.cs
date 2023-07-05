using System.Net;
using Azure;
using Bsn.Authentification.Interfaces;
using Bsn.Utilities.Constants;
using Core.Utilities.Exceptions;
using Core.Utilities.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model.Dto;

namespace MyStreamManagerFunctions.Functions.Auth
{
    public class LogOut
    {
        private readonly ILogger _logger;
        private readonly IAuthentificationService _authentification;
        public LogOut(ILoggerFactory loggerFactory, IAuthentificationService authentification)
        {
            _logger = loggerFactory.CreateLogger<LogOut>();
            _authentification = authentification;
        }
        [OpenApiOperation(operationId: nameof(LogOut), tags: new[] { SwaggerGroups.auth }, Summary = nameof(LogOut), Description = ResponseMessages.Logout)]
        [OpenApiParameter(Constant.authorization, In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: Constant.APPLICATION_JSON, bodyType: typeof(SuccessResult), Summary = ResponseMessages.Logout, Description = ResponseMessages.Logout)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.BadRequestError)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Unauthorized, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.UnauthorizedAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Forbidden, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.ForbiddenAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.InternalServerError)]
        [Function(nameof(LogOut))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, HttpMethods.get, Route = FunctionsRutes.logOut)] HttpRequestData req)
        {
            string? bearerToken = req.GetHeaderValue<string>(Constant.authorization);
            UnauthorizedException.ThrowIfTrue(string.IsNullOrWhiteSpace(bearerToken), ErrorMessage.EmptyToken);
            string? token = bearerToken!.StartsWith($"{Constant.Bearer} ", StringComparison.InvariantCultureIgnoreCase) ? bearerToken[7..] : bearerToken;
            bool result = await _authentification.LogOut(token);
            return await req.CreateResponseAsync(HttpStatusCode.OK, new SuccessResult() { Value = result});
        }
    }
}
