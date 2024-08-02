using FluentValidation;
using MessagingPlatform.Application.Common.Interfaces;
using MessagingPlatform.Application.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace MessagingPlatform.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        // Register MediatR services
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(assembly));

        // Register FluentValidation validators
        services.AddValidatorsFromAssembly(assembly);

        // Register HttpContextAccessor
        services.AddHttpContextAccessor();

        // Register application services
        services.AddScoped<IPasswordHasherService, PasswordHasherService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAccountService, AccountService>();

        return services;
    }
}