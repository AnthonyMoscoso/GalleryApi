using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Bsn.Authentification.Interfaces;
using Core.Utilities.Extensions;
using Bsn.Utilities.Constants;
using Core.Utilities.Ensures;

using Azure;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using Model.Dto.User;
using Bsn.DataServices.ImageFiles.Interfaces;
using Model.Dto.Images;
using Model.Dto.Files;
using Model.Dto.Auth;

namespace MyStreamManagerFunctions.Functions.Users
{
    public class ImageFilesAdd
    {
        private readonly ILogger _logger;
        private readonly ITokenService _tokenService;

        private readonly IImageFilesDataServices _service;
        public ImageFilesAdd(ILoggerFactory loggerFactory,
            ITokenService tokenService,
            IImageFilesDataServices services)
        {
            _logger = loggerFactory.CreateLogger<ImageFilesAdd>();
            _tokenService = tokenService;
            _service = services;

        }

        [OpenApiOperation(operationId: nameof(ImageFilesAdd), tags: new[] { SwaggerGroups.imageFiles }, Summary = nameof(ImageFilesAdd), Description = ResponseMessages.ImagesRegister)]
        [OpenApiParameter(Constant.authorization, In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiRequestBody(contentType: Constant.APPLICATION_JSON, bodyType: typeof(ImageFileInputDto))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ImageFileDto), Summary = ResponseMessages.ImagesRegister, Description = ResponseMessages.ImagesRegister)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.BadRequestError)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Unauthorized, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.UnauthorizedAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Forbidden, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.ForbiddenAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.InternalServerError)]
        [Function(nameof(ImageFilesAdd))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, HttpMethods.post, Route = FunctionsRutes.images)] HttpRequestData req)
        {
            UserInfo userInfo = await _tokenService.GetUserFromToken(req);
            ImageFileInputDto? item = await req.DeserializeBodyAsync<ImageFileInputDto>();
            Ensure.That(item, nameof(item)).IsNotNull();
            ImageFileDto result = await _service.Insert(userInfo,item!);
            return await req.CreateResponseAsync(HttpStatusCode.Created, result);
        }
    }
}
