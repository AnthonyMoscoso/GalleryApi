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
using Model.Dto.Album;
using Model.Dto.Auth;
using Model.Dto.Files;
using Model.Dto.Images;
using MyStreamManagerFunctions.Functions.Users;
using Newtonsoft.Json;

namespace SaalDigitalFunctions.Functions.Albums
{
    public class AlbumsAdd
    {
        private readonly ILogger _logger;
        private readonly ITokenService _tokenService;

        private readonly IAlbumServicies _service;
        public AlbumsAdd(ILoggerFactory loggerFactory,
            ITokenService tokenService,
            IAlbumServicies services)
        {
            _logger = loggerFactory.CreateLogger<AlbumsAdd>();
            _tokenService = tokenService;
            _service = services;

        }

        [OpenApiOperation(operationId: nameof(AlbumsAdd), tags: new[] { SwaggerGroups.albums }, Summary = nameof(AlbumsAdd), Description = ResponseMessages.AlbumsRegister)]
        [OpenApiParameter(Constant.authorization, In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiRequestBody(contentType: Constant.APPLICATION_JSON, bodyType: typeof(AlbumInputDto))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: Constant.APPLICATION_JSON, bodyType: typeof(AlbumDto), Summary = ResponseMessages.AlbumsRegister, Description = ResponseMessages.AlbumsRegister)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.BadRequestError)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Unauthorized, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.UnauthorizedAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Forbidden, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.ForbiddenAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.InternalServerError)]
        [Function(nameof(AlbumsAdd))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, HttpMethods.post, Route = FunctionsRutes.albums)] HttpRequestData req)
        {
            UserInfo userInfo = await _tokenService.GetUserFromToken(req);
            AlbumInputDto? item = await req.DeserializeBodyAsync<AlbumInputDto>();
            Ensure.That(item, nameof(item)).IsNotNull();
            AlbumDto result = await _service.Insert(userInfo, item!);
            return await req.CreateResponseAsync(HttpStatusCode.Created, result);
        }
    }
}