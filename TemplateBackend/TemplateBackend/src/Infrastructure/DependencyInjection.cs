using System.Reflection;
using Azure.Storage.Blobs;
using TemplateBackend.Application.Common.Interfaces;
using TemplateBackend.Application.Contracts;
using TemplateBackend.Domain.Constants;
using TemplateBackend.Infrastructure.Data;
using TemplateBackend.Infrastructure.Data.Interceptors;
using TemplateBackend.Infrastructure.Repository;
using TemplateBackend.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Bebac.Application.Common.Interfaces;

namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        services.AddAutoMapper(Assembly.GetExecutingAssembly());


        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddTransient<IApplicationUsersRepository, ApplicationUsersRepository>();
        services.AddTransient<IJWTManagerRepository, JWTManagerRepository>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddAuthorizationBuilder();

        services.AddSingleton(TimeProvider.System);

        services.AddAuthorization(options =>
            options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));
        
        services.AddScoped<IBlobService>(sp => {
            var blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=your_account_name;AccountKey=your_account_key;EndpointSuffix=your_endpoint_suffix");
            return new BlobService(blobServiceClient, sp.GetRequiredService<ILogger<BlobService>>());
        });


        return services;
    }
}
