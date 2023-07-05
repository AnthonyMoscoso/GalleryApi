using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;

namespace SaalDigitalFunctions.Middleware.Handlers.Errors
{
    public static class ExceptionHandlerConfigure
    {
        public static void ConfigureCustomExceptionMiddleware(this IFunctionsWorkerApplicationBuilder builder)
        {
            builder.UseMiddleware<GlobalHandlerException>();
        }

    }
}
