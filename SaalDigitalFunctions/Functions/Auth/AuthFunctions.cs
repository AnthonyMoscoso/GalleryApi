using System.Net;
using Azure;
using Bsn.Authentification.Interfaces;
using Bsn.Utilities.Constants;
using Core.Utilities.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model.Dto.Auth;

namespace MyStreamManagerApi.Functions.Auth
{
    public class AuthFunctions
    {
        private readonly ILogger _logger;
        private readonly IAuthentificationService _authentification;
        public AuthFunctions(ILoggerFactory loggerFactory, IAuthentificationService authentification)
        {
            _logger = loggerFactory.CreateLogger<AuthFunctions>();
            _authentification = authentification;
        }
        [OpenApiOperation(operationId: nameof(AuthFunctions), tags: new[] { SwaggerGroups.auth }, Summary = nameof(AuthFunctions), Description = ResponseMessages.GetToken)]
        [OpenApiRequestBody(contentType: Constant.APPLICATION_JSON, bodyType: typeof(LoginRequest))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: Constant.APPLICATION_JSON, bodyType: typeof(LoginResult), Summary = ResponseMessages.UserToken, Description = ResponseMessages.UserToken)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.BadRequestError)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Unauthorized, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.UnauthorizedAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Forbidden, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.ForbiddenAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.InternalServerError)]
        [Function(nameof(AuthFunctions))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, HttpMethods.post,Route = FunctionsRutes.auth)] HttpRequestData req)
        {

            LoginRequest? loginRequest = await req.DeserializeBodyAsync<LoginRequest>();  
            LoginResult loginResult = await _authentification.Login(loginRequest!);
            return await req.CreateResponseAsync(HttpStatusCode.OK, loginResult);
        }
    }
}
