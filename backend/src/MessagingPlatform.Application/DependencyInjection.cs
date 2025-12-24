using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MessagingPlatform.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddValidatorsFromAssembly(assembly);
        services.AddHttpContextAccessor();

        var handlerTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Handler"));

        foreach (var handlerType in handlerTypes)
        {
            services.AddScoped(handlerType, handlerType);
        }

        return services;
    }
}