using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Ride23.Identity.Infrastructure.Persistence;

public class SeedClientsAndScopes : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public SeedClientsAndScopes(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        _ = await context.Database.EnsureCreatedAsync(cancellationToken);

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        if (await manager.FindByClientIdAsync(Constants.Client, cancellationToken) is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = Constants.Client,
                ClientSecret = Constants.ClientSecret,
                DisplayName = Constants.ClientDisplayName,
                Permissions =
                {
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.ClientCredentials,
                    Permissions.ResponseTypes.Token,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles,
                    Permissions.Prefixes.Scope + Constants.CustomerReadScope,
                    Permissions.Prefixes.Scope + Constants.CustomerWriteScope
                }
            }, cancellationToken);
        }

        if (await manager.FindByClientIdAsync(Constants.GatewayResourceServer, cancellationToken) is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = Constants.GatewayResourceServer,
                ClientSecret = Constants.GatewayResourceServerSecret,
                Permissions =
                {
                    Permissions.Endpoints.Introspection
                }
            }, cancellationToken);
        }

        if (await manager.FindByClientIdAsync(Constants.CustomerResourceServer, cancellationToken) is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = Constants.CustomerResourceServer,
                ClientSecret = Constants.CustomerResourceServerSecret,
                Permissions =
                {
                    Permissions.Endpoints.Introspection
                }
            }, cancellationToken);
        }

        var scopesManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();

        if (await scopesManager.FindByNameAsync(Constants.CustomerWriteScope, cancellationToken) is null)
        {
            await scopesManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = Constants.CustomerWriteScope,
                Resources =
                {
                    Constants.CustomerResourceServer,
                    Constants.GatewayResourceServer
                }
            }, cancellationToken);
        }

        if (await scopesManager.FindByNameAsync(Constants.CustomerReadScope, cancellationToken) is null)
        {
            await scopesManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = Constants.CustomerReadScope,
                Resources =
                {
                    Constants.CustomerResourceServer,
                    Constants.GatewayResourceServer
                }
            }, cancellationToken);
        }
    }
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
