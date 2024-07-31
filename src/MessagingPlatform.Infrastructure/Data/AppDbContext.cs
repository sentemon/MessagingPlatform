using MessagingPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MessagingPlatform.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<DbContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
}