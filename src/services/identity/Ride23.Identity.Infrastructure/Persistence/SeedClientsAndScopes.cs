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
        if (await manager.FindByClientIdAsync(Constants.AdminClient, cancellationToken) is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = Constants.AdminClient,
                ClientSecret = Constants.AdminClientSecret,
                DisplayName = Constants.AdminClientDisplayName,
                Permissions =
                {
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.ClientCredentials,
                    Permissions.ResponseTypes.Code,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles,
                    Permissions.Prefixes.Scope + Constants.CustomerReadScope,
                    Permissions.Prefixes.Scope + Constants.CustomerWriteScope,
                    Permissions.Prefixes.Scope + Constants.DriverReadScope,
                    Permissions.Prefixes.Scope + Constants.DriverWriteScope,
                    Permissions.Prefixes.Scope + Constants.LocationReadScope,
                    Permissions.Prefixes.Scope + Constants.LocationWriteScope
                }
            }, cancellationToken);
        }

        if (await manager.FindByClientIdAsync(Constants.UserClient, cancellationToken) is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = Constants.UserClient,
                ClientSecret = Constants.UserClientSecret,
                DisplayName = Constants.UserClientDisplayName,
                Permissions =
                {
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.Password,
                    Permissions.GrantTypes.RefreshToken,
                    Permissions.ResponseTypes.Code,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles,
                    Permissions.Prefixes.Scope + Constants.CustomerReadScope,
                    Permissions.Prefixes.Scope + Constants.CustomerWriteScope,
                    Permissions.Prefixes.Scope + Constants.DriverReadScope,
                    Permissions.Prefixes.Scope + Constants.DriverWriteScope,
                    Permissions.Prefixes.Scope + Constants.LocationReadScope,
                    Permissions.Prefixes.Scope + Constants.LocationWriteScope
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

        if (await manager.FindByClientIdAsync(Constants.DriverResourceServer, cancellationToken) is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = Constants.DriverResourceServer,
                ClientSecret = Constants.DriverResourceServerSecret,
                Permissions =
                {
                    Permissions.Endpoints.Introspection
                }
            }, cancellationToken);
        }

        if (await manager.FindByClientIdAsync(Constants.LocationResourceServer, cancellationToken) is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = Constants.LocationResourceServer,
                ClientSecret = Constants.LocationResourceServerSecret,
                Permissions =
                {
                    Permissions.Endpoints.Introspection
                }
            }, cancellationToken);
        }
        
        if (await manager.FindByClientIdAsync(Constants.OrderResourceServer, cancellationToken) is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = Constants.OrderResourceServer,
                ClientSecret = Constants.OrderResourceServerSecret,
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

        if (await scopesManager.FindByNameAsync(Constants.DriverWriteScope, cancellationToken) is null)
        {
            await scopesManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = Constants.DriverWriteScope,
                Resources =
                {
                    Constants.DriverResourceServer,
                    Constants.GatewayResourceServer
                }
            }, cancellationToken);
        }

        if (await scopesManager.FindByNameAsync(Constants.DriverReadScope, cancellationToken) is null)
        {
            await scopesManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = Constants.DriverReadScope,
                Resources =
                {
                    Constants.DriverResourceServer,
                    Constants.GatewayResourceServer
                }
            }, cancellationToken);
        }

        if (await scopesManager.FindByNameAsync(Constants.LocationWriteScope, cancellationToken) is null)
        {
            await scopesManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = Constants.LocationWriteScope,
                Resources =
                {
                    Constants.LocationResourceServer,
                    Constants.GatewayResourceServer
                }
            }, cancellationToken);
        }

        if (await scopesManager.FindByNameAsync(Constants.LocationReadScope, cancellationToken) is null)
        {
            await scopesManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = Constants.LocationReadScope,
                Resources =
                {
                    Constants.LocationResourceServer,
                    Constants.GatewayResourceServer
                }
            }, cancellationToken);
        }

        if (await scopesManager.FindByNameAsync(Constants.OrderWriteScope, cancellationToken) is null)
        {
            await scopesManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = Constants.OrderWriteScope,
                Resources =
                {
                    Constants.OrderResourceServer,
                    Constants.GatewayResourceServer
                }
            }, cancellationToken);
        }

        if (await scopesManager.FindByNameAsync(Constants.OrderReadScope, cancellationToken) is null)
        {
            await scopesManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = Constants.OrderReadScope,
                Resources =
                {
                    Constants.OrderResourceServer,
                    Constants.GatewayResourceServer
                }
            }, cancellationToken);
        }
    }
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
