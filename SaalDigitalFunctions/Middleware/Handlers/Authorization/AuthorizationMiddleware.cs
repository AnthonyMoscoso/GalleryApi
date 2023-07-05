using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;

namespace SaalDigitalFunctions.Middleware.Handlers.Authorization
{
    public class AuthorizationMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {

            await next(context);

        }
    }
}
