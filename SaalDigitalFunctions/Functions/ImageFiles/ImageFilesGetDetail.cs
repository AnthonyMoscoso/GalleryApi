using System.Net;
using Azure;
using Bsn.Authentification.Interfaces;
using Bsn.DataServices.ImageFiles.Interfaces;
using Bsn.Utilities.Constants;

using Core.Utilities.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model.Dto.Auth;
using Model.Dto.Files;
using Model.Dto.User;

namespace MyStreamManagerFunctions.Functions.Users
{
    public class ImageFilesGetDetail
    {
        private readonly ILogger _logger;
        private readonly IImageFileGetDataServices _servicie;
        private readonly ITokenService _tokenService;


        public ImageFilesGetDetail(ILoggerFactory loggerFactory, IImageFileGetDataServices service, ITokenService tokenService)
        {
            _servicie = service;
            _tokenService = tokenService;

            _logger = loggerFactory.CreateLogger<ImageFilesGetDetail>();
        }

        [OpenApiOperation(operationId: nameof(ImageFilesGetDetail), tags: new[] { SwaggerGroups.imageFiles }, Summary = nameof(ImageFilesGetDetail), Description = ResponseMessages.ImagesDetail)]
        [OpenApiParameter(Constant.authorization, In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiParameter(name: Constant.id, In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ImageFileDto), Summary = ResponseMessages.ImagesDetail, Description = ResponseMessages.ImagesDetail)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.BadRequestError)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Unauthorized, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.UnauthorizedAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Forbidden, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.ForbiddenAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.InternalServerError)]
        [Function(nameof(ImageFilesGetDetail))]
        [Authorize]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, HttpMethods.get, Route = FunctionsRutes.imagesId)] HttpRequestData req, string id)
        {
            await _tokenService.GetUserFromToken(req);

            var result = await _servicie.Get(id);
            return await req.CreateResponseAsync(HttpStatusCode.OK, result);
        }
    }
}
