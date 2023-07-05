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
using Model.Dto.User;
using MyStreamManagerFunctions.Functions.Users;
using Newtonsoft.Json;

namespace SaalDigitalFunctions.Functions.Albums
{
    public class AlbumsUpdate
    {
        private readonly ILogger _logger;
        private readonly ITokenService _tokenService;

        private readonly IAlbumServicies _servicie;
        public AlbumsUpdate(ILoggerFactory loggerFactory,
            ITokenService tokenService,
            IAlbumServicies servicie)
        {
            _logger = loggerFactory.CreateLogger<AlbumsUpdate>();
            _tokenService = tokenService;
            _servicie = servicie;

        }

        [OpenApiOperation(operationId: nameof(AlbumsUpdate), tags: new[] { SwaggerGroups.albums }, Summary = nameof(AlbumsUpdate), Description = ResponseMessages.UpdateAlbums)]
        [OpenApiParameter(Constant.authorization, In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiParameter(name: Constant.id, In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiRequestBody(contentType: Constant.APPLICATION_JSON, bodyType: typeof(AlbumInputDto))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: Constant.APPLICATION_JSON, bodyType: typeof(AlbumDto), Summary = ResponseMessages.UpdateAlbums, Description = ResponseMessages.UpdateAlbums)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.BadRequestError)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Unauthorized, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.UnauthorizedAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Forbidden, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.ForbiddenAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.InternalServerError)]
        [Function(nameof(AlbumsUpdate))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, HttpMethods.put, Route = FunctionsRutes.albumsId)] HttpRequestData req, string id)
        {
            UserInfo userInfo = await _tokenService.GetUserFromToken(req);
            AlbumInputDto? item = await req.DeserializeBodyAsync<AlbumInputDto>();
            Ensure.That(id, nameof(id)).NotNullOrEmpty(ErrorMessage.IdValueIsRequired);
            Ensure.That(item, nameof(item)).IsNotNull();
            AlbumDto result = await _servicie.Update(userInfo, item!, id);
            return await req.CreateResponseAsync(HttpStatusCode.OK, result);
        }
    }
}

