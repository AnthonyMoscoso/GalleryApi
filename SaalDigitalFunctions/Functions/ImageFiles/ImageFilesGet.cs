using System.Net;
using Azure;
using Bsn.Authentification.Interfaces;
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
using Model.Dto.Files;
using Model.Dto.Table;
using Model.Dto.User;

namespace MyStreamManagerApi.Functions.Users
{
    public class ImageFilesGet
    {
        private readonly ILogger _logger;
        private readonly IImageFileGetDataServices _servicie;
        private readonly ITokenService _tokenService;

        public ImageFilesGet(ILoggerFactory loggerFactory,
            IImageFileGetDataServices servicies,
            ITokenService tokenService)
        {
            _tokenService = tokenService;
            _logger = loggerFactory.CreateLogger<ImageFilesGet>();
            _servicie = servicies;
            _tokenService = tokenService;


        }
        [OpenApiOperation(operationId: nameof(ImageFilesGet), tags: new[] { SwaggerGroups.imageFiles }, Summary = nameof(ImageFilesGet), Description = ResponseMessages.ListOfImages)]
        [OpenApiParameter(Constant.authorization, In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiParameter(name: QueryParams.search, In = ParameterLocation.Query, Type = typeof(string))]
        [OpenApiParameter(name: TableParam.skip, In = ParameterLocation.Query, Type = typeof(int))]
        [OpenApiParameter(name: TableParam.take, In = ParameterLocation.Query, Type = typeof(int))]
        [OpenApiParameter(name: TableParam.orderBy, In = ParameterLocation.Query, Type = typeof(string))]
        [OpenApiParameter(name: TableParam.isAsc, In = ParameterLocation.Query, Type = typeof(bool))]
        [OpenApiParameter(name: QueryParams.all, In = ParameterLocation.Query, Type = typeof(bool))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: Constant.APPLICATION_JSON, bodyType: typeof(DataTable<ImageFileDto>), Summary = ResponseMessages.ListOfImages, Description = ResponseMessages.ListOfImages)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.BadRequestError)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Unauthorized, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.UnauthorizedAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Forbidden, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.ForbiddenAccess)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: Constant.APPLICATION_JSON, bodyType: typeof(ResponseError), Description = ErrorMessage.InternalServerError)]
        [Function(nameof(ImageFilesGet))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, HttpMethods.get, Route = FunctionsRutes.images)] HttpRequestData req)
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
