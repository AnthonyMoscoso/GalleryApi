using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaalDigitalFunctions.Middleware.Handlers.Authorization
{
    public static class AuthorizationHandlerConfigure
    {
        public static void ConfigureCustomAuthorizationMiddleware(this IFunctionsWorkerApplicationBuilder builder)
        {
            builder.UseMiddleware<AuthorizationMiddleware>();
        }
    }
}
