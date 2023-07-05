using Bsn.Authentification;
using Bsn.Authentification.Interfaces;
using Bsn.AzureServices.BlobStorage;
using Bsn.AzureServices.BlobStorage.Interfaces;
using Bsn.DataServices.AlbumImages;
using Bsn.DataServices.AlbumImages.Interfaces;
using Bsn.DataServices.Albums.Interfaces;
using Bsn.DataServices.Albums;
using Bsn.DataServices.ImageFiles;
using Bsn.DataServices.ImageFiles.Interfaces;
using Bsn.DataServices.Users;
using Bsn.DataServices.Users.Interfaces;
using Bsn.EncrypterService;
using Bsn.MapperService;
using Bsn.Utilities.Configuration;
using Bsn.Utilities.Configuration.Interfaces;
using Bsn.Utilities.Constants;
using Bsn.ValidationService;
using Bsn.ValidationServices;
using Core.Utilities.Ensures;
using FluentValidation;
using Infra.DataAccess;
using Infra.DataAccess.DBs;
using Infra.DataAccess.DBs.Interfaces;
using Infra.DataAccess.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Model.Context.Contexts;
using Model.Dto.Album;
using Model.Dto.Auth;
using Model.Dto.Images;
using Model.Dto.User;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Bsn.DependencyResolver
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.TokenValidationParameters = new TokenValidationParameters()
                       {
                           ValidateIssuer = true,
                           ValidateAudience = true,
                           ValidateLifetime = true,
                           ValidateIssuerSigningKey = true,
                           ValidIssuer = configuration[AppSettings.jwt_Issuer],
                           ValidAudience = configuration[AppSettings.jwt_audicence],
                           IssuerSigningKey = new SymmetricSecurityKey(
                               Encoding.UTF8.GetBytes(configuration[AppSettings.jwt_secret_key]!)
                           )
                       };
                   });
            return services;
        }

        public static IServiceCollection AddDbContextConnection([NotNull]this IServiceCollection services,[NotNull]IConfiguration configuration,[NotNull]string assemblyName) 
        {

            Ensure.That(configuration, nameof(configuration)).IsNotNull();
            Ensure.That(assemblyName, nameof(assemblyName)).NotNullOrEmpty();
            string? connection = configuration.GetConnectionString(AppSettings.stream_auth);
            Ensure.That(connection, nameof(connection)).NotNullOrEmpty();
            string? user = configuration[AppSettings.dbuser];
            string? password = configuration[AppSettings.dbpass];
            Ensure.That(user, nameof(user)).NotNullOrEmpty();
            Ensure.That(password, nameof(password)).NotNullOrEmpty();
            connection = connection!.Replace(Constant.User, user).Replace(Constant.Pass, password);
            string projectName = assemblyName!.Split(',')[0];
            services.AddDbContext<SaalDigitalContext>(w => { w.UseLazyLoadingProxies().UseSqlServer(connection, b => b.MigrationsAssembly(projectName)); });
            return services;
        }
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton(MapperConfigProfiles.GetProfileConfig().CreateMapper());
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthentificationService, AuthentificationService>();
            services.AddScoped<IEncrypterService, EncrypterManager>();

            return services;
        }

        public static IServiceCollection AddKeys(this IServiceCollection services) 
        {
            services.AddScoped<IAzureKeys, AzureKeys>();
            services.AddScoped<IJWTKeys, JWTKeys>();
            services.AddScoped<IKeys, Keys>();
            return services;
        }

        public static IServiceCollection AddAzureServices(this IServiceCollection services)
        {
            services.AddScoped<IAzureBlobStoreProfile, AzureBlobStoreProfile>();
            services.AddScoped<IAzureBlobStorageManager, AzureBlobStorageManager>();
            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<ImageFileInputDto>, ImageFileInputDtoValidator>();
            services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
            services.AddScoped<IValidator<UserInputDto>, UserInputDtoValidator>();
            services.AddScoped<IValidator<AlbumInputDto>, AlbumInputDtoValidator>();
            services.AddScoped<IValidator<AlbumImageInputDto>, AlbumImageInputDtoValidator>();
            return services;
        }

     
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {

            #region ImageFiles
            services.AddScoped<ImageFileDataServices>(); 
            services.AddScoped<IImageFilesDataServices>(x => x.GetRequiredService<ImageFileDataServices>()); 
            services.AddScoped<IImageFileGetDataServices>(x => x.GetRequiredService<ImageFileDataServices>());
            #endregion

            #region Albums
            services.AddScoped<AlbumServicies>();
            services.AddScoped<IAlbumServicies>(x => x.GetRequiredService<AlbumServicies>());
            services.AddScoped<IAlbumGetServicies>(x => x.GetRequiredService<AlbumServicies>());
            #endregion


            services.AddScoped<IAlbumImageServices, AlbumImageServices>();
            services.AddScoped<IUserService, UserServicies>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            #region Auth
            services.AddScoped<ICredentialsRepository, CredentialRepository>();
           
            services.AddScoped<ISessionRepository, SessionRepository>();
            #endregion
          
            #region Dbo
           
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IImageFileRepository, ImageFileRepository>();
            services.AddScoped<IAlbumRepository, AlbumRepository>();
            services.AddScoped<IAlbumImageRepository, AlbumImageRepository>();
            #endregion

            return services;
        }


    }
}
