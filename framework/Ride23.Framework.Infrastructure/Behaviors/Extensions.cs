using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Ride23.Framework.Infrastructure.Behaviors;
public static class Extensions
{
    public static IServiceCollection AddBehaviors(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }
}
