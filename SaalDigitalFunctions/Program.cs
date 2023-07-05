using Bsn.DependencyResolver;
using Bsn.Utilities.Constants;
using Core.Utilities.Ensures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SaalDigitalFunctions.Middleware;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using System.Reflection;

namespace SaalDigitalFunctions
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                 .ConfigureAppConfiguration((hostingContext, config) =>
                 {
                     config.AddJsonFile(AppSettings.fileName, optional: true, reloadOnChange: true);
                 })
                 .ConfigureFunctionsWorkerDefaults(builder =>
                 {
                     builder.ConfigureCustomMiddleware();
                 })
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    // Add authentication
                    services.AddAuth(configuration);
                    string? assemblyName = Assembly.GetExecutingAssembly().FullName;
                    Console.WriteLine(assemblyName);
                    Ensure.That(assemblyName, nameof(assemblyName)).NotNullOrEmpty();
                    // Add database connection
                    services.AddDbContextConnection(configuration, assemblyName!);

                    // Add custom services
                    services.AddServices();
                    // Add repositories
                    services.AddRepositories();
                    // Add key services
                    services.AddKeys();
     
                    // Add validators
                    services.AddValidators();
                    // Add Azure services
                    services.AddAzureServices();
                    // Add data services
                    services.AddDataServices();
                    // Add authorization
                    services.AddAuthorization();

                })
                .ConfigureOpenApi()
                .Build();


            await host.RunAsync();
        }
    }
}