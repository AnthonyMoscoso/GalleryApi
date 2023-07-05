using Bsn.DataServices.Users.Interfaces;
using Bsn.DependencyResolver;
using Bsn.EncrypterService;
using Core.Utilities.Ensures;
using Infra.DataAccess.DBs.Interfaces;
using Infra.DataAccess.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace StartedConfig
{
    public class Startup
    {
        private IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public static async void ConfigureServices(IServiceCollection services)
        {
            IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
            services.AddSingleton(config);
            services.AddAuth(config);
            string? assemblyName = Assembly.GetExecutingAssembly().FullName;
            Ensure.That(assemblyName, nameof(assemblyName)).NotNullOrEmpty();
            services.AddDbContextConnection(config, assemblyName!);
            services.AddServices();
            services.AddRepositories();
            services.AddKeys();

            services.AddValidators();
            services.AddAzureServices();
            services.AddDataServices();
            services.AddAuthorization();

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            IEncrypterService? encrypterService = serviceProvider.GetService<IEncrypterService>();
            ICredentialsRepository? credentialsRepository = serviceProvider.GetService<ICredentialsRepository>();
            //if (encrypterService != null && credentialsRepository != null)
            //{
            //    CredentialsStarted credentialsStarted = new(credentialsRepository, encrypterService);
            //    await credentialsStarted.CreateDefault();
            //}
            await Task.FromResult(true);



        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}