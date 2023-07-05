using System.Net;
using Azure;
using Bsn.Authentification.Interfaces;
using Bsn.DataServices.AlbumImages.Interfaces;
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
using Model.Dto.Album;
using Model.Dto.Auth;
using Model.Dto.Files;
using Model.Dto.Images;
using MyStreamManagerFunctions.Functions.Users;
using Newtonsoft.Json;

namespace SaalDigitalFunctions.Functions.Albums
{
    public class AlbumImagesAdd
    {
        private readonly ILogger _logger;
        private readonly ITokenService _tokenService;

        private readonly IAlbumImageServices _service;
        public AlbumImagesAdd(ILoggerFactory loggerFactory,
            ITokenService tokenService,
            IAlbumImageServices services)
        {
            _logger = loggerFactory.CreateLogger<AlbumImagesAdd>();
            _tokenService = tokenService;
            _service = services;

        }

        [OpenApiOperation(operationId: nameof(AlbumImagesAdd), tags: new[] { SwaggerGroups.albums }, Summary = nameof(AlbumImagesAdd), Description = ResponseMessages.AddImageFromAlbum)]
        [OpenApiParameter(Constant.authorization, In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiParameter(name: Constant.idAlbum, In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiRequestBody(contentType: Constant.APPLICATION_JSON, bodyType: typeof( AlbumImageInputDto))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: Constant.APPLICATION_JSON, bodyType: typeof(SuccessResult), Summary = ResponseMessages.AddImageFromAlbum, Description = ResponseMessages.AddImageFromAlbum)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.BadRequestError)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Unauthorized, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.UnauthorizedAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Forbidden, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.ForbiddenAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.InternalServerError)]
        [Function(nameof(AlbumImagesAdd))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, HttpMethods.post, Route = FunctionsRutes.albumImagesAdd)] HttpRequestData req,string idAlbum)
        {
            UserInfo userInfo = await _tokenService.GetUserFromToken(req);
            Ensure.That(idAlbum, nameof(idAlbum)).NotNullOrEmpty();
            AlbumImageInputDto? item = await req.DeserializeBodyAsync<AlbumImageInputDto>();
            Ensure.That(item, nameof(item)).IsNotNull();
            await _service.AddImageToAlbum(idAlbum,item!);
            return await req.CreateResponseAsync(HttpStatusCode.Created, new SuccessResult() { Value = true });
        }
    }
}