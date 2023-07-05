using System.Net;
using Azure;
using Bsn.Authentification.Interfaces;
using Bsn.DataServices.Albums.Interfaces;
using Bsn.DataServices.ImageFiles.Interfaces;
using Bsn.Utilities.Constants;
using Core.Utilities.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model.Dto.Album;
using Model.Dto.Files;
using MyStreamManagerFunctions.Functions.Users;
using Newtonsoft.Json;

namespace SaalDigitalFunctions.Functions.Albums
{
    public class AlbumGetDetail
    {
        private readonly ILogger _logger;
        private readonly IAlbumGetServicies _servicie;
        private readonly ITokenService _tokenService;


        public AlbumGetDetail(ILoggerFactory loggerFactory, IAlbumGetServicies service, ITokenService tokenService)
        {
            _servicie = service;
            _tokenService = tokenService;

            _logger = loggerFactory.CreateLogger<AlbumGetDetail>();
        }

        [OpenApiOperation(operationId: nameof(AlbumGetDetail), tags: new[] { SwaggerGroups.albums }, Summary = nameof(AlbumGetDetail), Description = ResponseMessages.AlbumsDetail)]
        [OpenApiParameter(Constant.authorization, In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiParameter(name: Constant.id, In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: Constant.APPLICATION_JSON, bodyType: typeof(AlbumDto), Summary = ResponseMessages.AlbumsDetail, Description = ResponseMessages.AlbumsDetail)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.BadRequestError)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Unauthorized, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.UnauthorizedAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Forbidden, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.ForbiddenAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.InternalServerError)]
        [Function(nameof(AlbumGetDetail))]
        [Authorize]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, HttpMethods.get, Route = FunctionsRutes.albumsId)] HttpRequestData req, string id)
        {
            await _tokenService.GetUserFromToken(req);
            var result = await _servicie.Get(id);
            return await req.CreateResponseAsync(HttpStatusCode.OK, result);
        }
    }
}
