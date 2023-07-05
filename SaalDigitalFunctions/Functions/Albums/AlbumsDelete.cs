using System.Net;
using Azure;
using Bsn.Authentification.Interfaces;
using Bsn.DataServices.Albums.Interfaces;
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
using MyStreamManagerFunctions.Functions.Users;
using Newtonsoft.Json;

namespace SaalDigitalFunctions.Functions.Albums
{
    public class AlbumsDelete
    {
        private readonly ILogger _logger;
        private readonly ITokenService _tokenService;

        private readonly IAlbumServicies _service;
        public AlbumsDelete(ILoggerFactory loggerFactory,
            ITokenService tokenService,
            IAlbumServicies services)
        {
            _logger = loggerFactory.CreateLogger<AlbumsDelete>();
            _tokenService = tokenService;
            _service = services;

        }

        [OpenApiOperation(operationId: nameof(AlbumsDelete), tags: new[] { SwaggerGroups.albums }, Summary = nameof(AlbumsDelete), Description = ResponseMessages.SuccessDelete)]
        [OpenApiParameter(Constant.authorization, In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiParameter(name: Constant.id, In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: Constant.APPLICATION_JSON, bodyType: typeof(SuccessResult), Summary = ResponseMessages.SuccessDelete, Description = ResponseMessages.SuccessDelete)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.BadRequestError)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Unauthorized, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.UnauthorizedAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Forbidden, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.ForbiddenAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.InternalServerError)]
        [Function(nameof(AlbumsDelete))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, HttpMethods.delete, Route = FunctionsRutes.albumsId)] HttpRequestData req, string id)
        {
            await _tokenService.GetUserFromToken(req);
            Ensure.That(id, nameof(id)).NotNullOrEmpty();
            await _service.Delete(id);
            return await req.CreateResponseAsync(HttpStatusCode.OK, new SuccessResult() { Value = true });
        }
    }
}


