using System.Net;
using Azure;
using Bsn.Authentification.Interfaces;
using Bsn.DataServices.Albums.Interfaces;
using Bsn.DataServices.ImageFiles.Interfaces;
using Bsn.Utilities.Constants;
using Bsn.Utilities.Extensions;
using Core.Model;
using Core.Utilities.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model.Dto.Album;
using Model.Dto.Files;
using Model.Dto.Table;
using MyStreamManagerApi.Functions.Users;
using Newtonsoft.Json;

namespace SaalDigitalFunctions.Functions.Albums
{
    public class AlbumsGet
    {
        private readonly ILogger _logger;
        private readonly IAlbumGetServicies _servicie;
        private readonly ITokenService _tokenService;

        public AlbumsGet(ILoggerFactory loggerFactory,
            IAlbumGetServicies servicies,
            ITokenService tokenService)
        {
            _tokenService = tokenService;
            _logger = loggerFactory.CreateLogger<AlbumsGet>();
            _servicie = servicies;
            _tokenService = tokenService;


        }
        [OpenApiOperation(operationId: nameof(AlbumsGet), tags: new[] { SwaggerGroups.albums }, Summary = nameof(AlbumsGet), Description = ResponseMessages.ListOfImages)]
        [OpenApiParameter(Constant.authorization, In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiParameter(name: QueryParams.search, In = ParameterLocation.Query, Type = typeof(string))]
        [OpenApiParameter(name: TableParam.skip, In = ParameterLocation.Query, Type = typeof(int))]
        [OpenApiParameter(name: TableParam.take, In = ParameterLocation.Query, Type = typeof(int))]
        [OpenApiParameter(name: TableParam.orderBy, In = ParameterLocation.Query, Type = typeof(string))]
        [OpenApiParameter(name: TableParam.isAsc, In = ParameterLocation.Query, Type = typeof(bool))]
        [OpenApiParameter(name: QueryParams.all, In = ParameterLocation.Query, Type = typeof(bool))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: Constant.APPLICATION_JSON, bodyType: typeof(DataTable<AlbumDto>), Summary = ResponseMessages.ListOfImages, Description = ResponseMessages.ListOfImages)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.BadRequestError)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Unauthorized, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.UnauthorizedAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Forbidden, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.ForbiddenAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.InternalServerError)]
        [Function(nameof(AlbumsGet))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, HttpMethods.get, Route = FunctionsRutes.albums)] HttpRequestData req)
        {

            await _tokenService.GetUserFromToken(req);
            string? search = req.GetValue<string>(QueryParams.search);
            bool all = req.GetValue<bool>(QueryParams.all);
            TableParams tableParams = req.GetTableParams();
            var result = await _servicie.GetTable(tableParams, search, all: all);
            return await req.CreateResponseAsync(HttpStatusCode.OK, result);
        }
    }
}
