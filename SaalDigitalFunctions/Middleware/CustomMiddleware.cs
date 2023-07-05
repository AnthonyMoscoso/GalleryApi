using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Azure.Functions.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bsn.Authentification.Interfaces;
using Microsoft.Azure.Functions.Worker.Http;
using Core.Utilities.Ensures;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core;
using Bsn.Utilities.Constants;

namespace SaalDigitalFunctions.Middleware
{
    public class CustomMiddleware : IFunctionsWorkerMiddleware
    {
        private readonly IAuthentificationService _authentificationService;
        public CustomMiddleware(IAuthentificationService authentificationService)
        {
            _authentificationService = authentificationService;
        }
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            HttpRequestData? httpRequest = await context.GetHttpRequestDataAsync();
            Ensure.That(httpRequest, nameof(httpRequest)).IsNotNull();
            if (!httpRequest!.Url.PathAndQuery.ToLower().Contains(Constant.swagger.ToLower()))
            {
                //await _authentificationService.ValidateRequestData(httpRequest!);
            }

            await next(context);

        }
    }
}
