using System.Text;
using MessagingPlatform.Application.Common.Interfaces;
using MessagingPlatform.Application.Services;
using MessagingPlatform.Domain.Interfaces;
using MessagingPlatform.Infrastructure.Interfaces;
using MessagingPlatform.Infrastructure.Persistence;
using MessagingPlatform.Infrastructure.Repositories;
using MessagingPlatform.Infrastructure.Security;
using MessagingPlatform.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MessagingPlatform.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();

        services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
        services.AddScoped<IJwtProvider, JwtProvider>();
        
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<ICookieService, CookieService>();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                var jwtOptions = services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>().Value;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["token"];

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization();
        
        return services;
    }
}