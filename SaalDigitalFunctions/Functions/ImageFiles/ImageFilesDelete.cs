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
using Model.Dto;

namespace MyStreamManagerFunctions.Functions.Users
{
    public class ImageFilesDelete
    {
        private readonly ILogger _logger;
        private readonly ITokenService _tokenService;

        private readonly IImageFilesDataServices _service;
        public ImageFilesDelete(ILoggerFactory loggerFactory,
            ITokenService tokenService,
            IImageFilesDataServices services)
        {
            _logger = loggerFactory.CreateLogger<ImageFilesDelete>();
            _tokenService = tokenService;
            _service = services;

        }

        [OpenApiOperation(operationId: nameof(ImageFilesDelete), tags: new[] { SwaggerGroups.imageFiles }, Summary = nameof(ImageFilesDelete), Description = ResponseMessages.SuccessDelete)]
        [OpenApiParameter(Constant.authorization, In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiParameter(name: Constant.id, In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: Constant.APPLICATION_JSON, bodyType: typeof(SuccessResult), Summary = ResponseMessages.SuccessDelete, Description = ResponseMessages.SuccessDelete)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.BadRequestError)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Unauthorized, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.UnauthorizedAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Forbidden, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.ForbiddenAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.InternalServerError)]
        [Function(nameof(ImageFilesDelete))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, HttpMethods.delete, Route = FunctionsRutes.imagesId)] HttpRequestData req, string id)
        {
            await _tokenService.GetUserFromToken(req);
            Ensure.That(id, nameof(id)).NotNullOrEmpty();
            await _service.Delete(id);
            return await req.CreateResponseAsync(HttpStatusCode.OK, new SuccessResult() { Value = true });
        }
    }
}

