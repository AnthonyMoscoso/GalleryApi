using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using SaalDigitalFunctions.Middleware.Handlers.Authorization;
using SaalDigitalFunctions.Middleware.Handlers.Errors;

namespace SaalDigitalFunctions.Middleware
{
    public static class CustomMiddlewareConfigure
    {
        public static void ConfigureCustomMiddleware(this IFunctionsWorkerApplicationBuilder builder)
        {
            builder.ConfigureCustomExceptionMiddleware();
            builder.UseMiddleware<CustomMiddleware>();
            builder.ConfigureCustomAuthorizationMiddleware();
        }

    }
}
