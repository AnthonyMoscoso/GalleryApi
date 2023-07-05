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
using MyStreamManagerFunctions.Functions.Users;
using Newtonsoft.Json;

namespace SaalDigitalFunctions.Functions.Albums
{
    public class AlbumImagesDelete
    {
        private readonly ILogger _logger;
        private readonly ITokenService _tokenService;

        private readonly IAlbumImageServices _service;
        public AlbumImagesDelete(ILoggerFactory loggerFactory,
            ITokenService tokenService,
            IAlbumImageServices services)
        {
            _logger = loggerFactory.CreateLogger<AlbumImagesDelete>();
            _tokenService = tokenService;
            _service = services;

        }

        [OpenApiOperation(operationId: nameof(AlbumImagesDelete), tags: new[] { SwaggerGroups.albums }, Summary = nameof(AlbumImagesDelete), Description = ResponseMessages.SuccessDelete)]
        [OpenApiParameter(Constant.authorization, In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiParameter(name: Constant.idAlbum, In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiParameter(name: Constant.idImage, In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: Constant.APPLICATION_JSON, bodyType: typeof(SuccessResult), Summary = ResponseMessages.SuccessDelete, Description = ResponseMessages.SuccessDelete)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.BadRequestError)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Unauthorized, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.UnauthorizedAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Forbidden, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.ForbiddenAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.InternalServerError)]
        [Function(nameof(AlbumImagesDelete))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, HttpMethods.delete, Route = FunctionsRutes.albumImages)] HttpRequestData req, string idAlbum,string idImage)
        {
            await _tokenService.GetUserFromToken(req);
            Ensure.That(idAlbum, nameof(idAlbum)).NotNullOrEmpty();
            Ensure.That(idImage, nameof(idImage)).NotNullOrEmpty();
            await _service.RemoveImageInalbum(idAlbum,idImage);
            return await req.CreateResponseAsync(HttpStatusCode.OK, new SuccessResult() { Value = true });
        }
    }
}


