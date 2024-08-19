using MessagingPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MessagingPlatform.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<UserChat> UserChats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Chat entity configuration
        modelBuilder.Entity<Chat>()
            .HasOne(c => c.Creator)
            .WithMany()
            .HasForeignKey(c => c.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configuring many-to-many relationship between Chat and User through UserChat
        modelBuilder.Entity<UserChat>()
            .HasKey(uc => new { uc.UserId, uc.ChatId }); 

        modelBuilder.Entity<UserChat>()
            .HasOne(uc => uc.User)
            .WithMany(u => u.UserChats)
            .HasForeignKey(uc => uc.UserId)
            .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<UserChat>()
            .HasOne(uc => uc.Chat)
            .WithMany(c => c.UserChats)
            .HasForeignKey(uc => uc.ChatId)
            .OnDelete(DeleteBehavior.Cascade);

        // Message entity configuration
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany(u => u.Messages)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Chat)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ChatId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
    }
}