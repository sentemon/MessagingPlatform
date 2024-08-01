using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Infrastructure.Data;

public class AppDbContextInitializer
{
    private readonly ILogger<AppDbContextInitializer> _logger;
    private readonly AppDbContext _context;

    public AppDbContextInitializer(ILogger<AppDbContextInitializer> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            _logger.LogInformation("Starting database initialization.");

            await _context.Database.MigrateAsync();
            _logger.LogInformation("Database migration completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            _logger.LogInformation("Starting database seeding.");

            await TrySeedAsync();

            _logger.LogInformation("Database seeding completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        if (!await _context.Users.AnyAsync())
        {
            _context.Users.Add(new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Admin",
                LastName = "User",
                Username = "admin",
                Email = "admin@example.com",
                AccountCreatedAt = DateTime.UtcNow,
                ExternalAuthId = "external-id"
            });

            await _context.SaveChangesAsync();
            _logger.LogInformation("Initial data has been seeded.");
        }
        else
        {
            _logger.LogInformation("Database already contains data. Skipping seeding.");
        }
    }
}
