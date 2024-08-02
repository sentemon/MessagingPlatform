using FluentValidation;
using MessagingPlatform.Application.Common.Interfaces;
using MessagingPlatform.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MessagingPlatform.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(assembly));

        services.AddValidatorsFromAssembly(assembly);

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPasswordHasherService, PasswordHasherService>();

        return services;
    }
}