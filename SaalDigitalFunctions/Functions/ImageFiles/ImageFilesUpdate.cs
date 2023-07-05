using System.Net;
using Azure;
using Bsn.Authentification.Interfaces;
using Bsn.DataServices.ImageFiles.Interfaces;
using Bsn.Utilities.Constants;

using Core.Utilities.Ensures;
using Core.Utilities.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model.Dto.Auth;
using Model.Dto.Files;
using Model.Dto.Images;
using Model.Dto.User;

namespace MyStreamManagerFunctions.Functions.Users
{
    public class ImageFilesUpdate
    {
        private readonly ILogger _logger;
        private readonly ITokenService _tokenService;

        private readonly IImageFilesDataServices _servicie;
        public ImageFilesUpdate(ILoggerFactory loggerFactory,
            ITokenService tokenService,
            IImageFilesDataServices servicie)
        {
            _logger = loggerFactory.CreateLogger<ImageFilesUpdate>();
            _tokenService = tokenService;
            _servicie = servicie;

        }

        [OpenApiOperation(operationId: nameof(ImageFilesUpdate), tags: new[] { SwaggerGroups.imageFiles }, Summary = nameof(ImageFilesUpdate), Description = ResponseMessages.UpdateImages)]
        [OpenApiParameter(Constant.authorization, In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiParameter(name: Constant.id, In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiRequestBody(contentType: Constant.APPLICATION_JSON, bodyType: typeof(ImageFileInputDto))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: Constant.APPLICATION_JSON, bodyType: typeof(UserDto), Summary = ResponseMessages.UpdateImages, Description = ResponseMessages.UpdateImages)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.BadRequestError)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Unauthorized, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.UnauthorizedAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Forbidden, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.ForbiddenAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.InternalServerError)]
        [Function(nameof(ImageFilesUpdate))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, HttpMethods.put, Route = FunctionsRutes.imagesId)] HttpRequestData req, string id)
        {
            UserInfo userInfo = await _tokenService.GetUserFromToken(req);
            ImageFileInputDto? item = await req.DeserializeBodyAsync<ImageFileInputDto>();
            Ensure.That(id, nameof(id)).NotNullOrEmpty(ErrorMessage.IdValueIsRequired);
            Ensure.That(item, nameof(item)).IsNotNull();
            ImageFileDto result = await _servicie.Update(userInfo,item!, id);
            return await req.CreateResponseAsync(HttpStatusCode.OK, result);
        }
    }
}

