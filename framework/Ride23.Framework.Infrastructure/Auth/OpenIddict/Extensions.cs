using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Validation.AspNetCore;
using Ride23.Framework.Infrastructure.Options;
using System.Reflection;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Ride23.Framework.Infrastructure.Auth.OpenIddict;

public static class Extensions
{
    public static IServiceCollection AddAuthValidation(this IServiceCollection services, IConfiguration config)
    {
        var authOptions = services.BindValidateReturn<OpenIddictOptions>(config);

        services.AddOpenIddict()
        .AddValidation(options =>
        {
            options.SetIssuer(authOptions.IssuerUrl!);
            options.UseIntrospection()
                   .SetClientId(authOptions.ClientId!)
                   .SetClientSecret(authOptions.ClientSecret!);
            options.UseSystemNetHttp();
            options.UseAspNetCore();
        });

        services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        services.AddAuthorization();
        return services;
    }

    public static void ConfigureAuthServer<T>(this WebApplicationBuilder builder, Assembly dbContextAssembly, string connectionName = "DefaultConnection", string schemaName = "Identity") where T : DbContext
    {
        builder.Services.AddOpenIddict()
        .AddCore(options => options.UseEntityFrameworkCore().UseDbContext<T>())
        .AddServer(options =>
        {
            options.SetIntrospectionEndpointUris("/connect/introspect")
                   .SetUserinfoEndpointUris("/connect/userinfo")
                   .SetTokenEndpointUris("/connect/token");

            options.AllowClientCredentialsFlow()
                   .AllowPasswordFlow()
                   .AllowRefreshTokenFlow();

            options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles);

            options.DisableAccessTokenEncryption();

            options.AddDevelopmentEncryptionCertificate()
                   .AddDevelopmentSigningCertificate();

            options.UseAspNetCore()
                   .EnableTokenEndpointPassthrough()
                   .EnableUserinfoEndpointPassthrough()
                   .DisableTransportSecurityRequirement();
        })
        .AddValidation(options =>
        {
            options.UseLocalServer();
            options.UseAspNetCore();
        });

        builder.Services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        builder.Services.AddAuthorization();

        string connectionString = builder.Configuration.GetConnectionString(connectionName) ??
            throw new ArgumentNullException(nameof(connectionName));

        builder.Services.AddDbContext<T>(options =>
        {
            options.UseNpgsql(connectionString, m =>
            {
                m.MigrationsAssembly(dbContextAssembly.FullName);
                m.MigrationsHistoryTable(HistoryRepository.DefaultTableName, schemaName);
            });
            options.UseOpenIddict();
        });
    }
}
