using MessagingPlatform.Domain.Interfaces;
using MessagingPlatform.Infrastructure.Data;
using MessagingPlatform.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MessagingPlatform.Infrastructure;

public static class DependencyInjection
{
    public static async Task<IServiceCollection> AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        var userInitializer =  services.BuildServiceProvider().GetRequiredService<AppDbContextInitializer>();
        await userInitializer.InitialiseAsync();

        services.AddScoped<IUserRepository, UserRepository>();
        
        return services;
    }
}