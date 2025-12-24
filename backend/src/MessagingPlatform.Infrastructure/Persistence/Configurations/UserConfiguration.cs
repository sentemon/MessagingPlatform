using MessagingPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessagingPlatform.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd();

        builder.Property(u => u.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.Username)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasMaxLength(320)
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.Bio)
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(u => u.IsOnline)
            .IsRequired(false);

        builder.Property(u => u.AccountCreatedAt)
            .ValueGeneratedOnAdd();
        
        // Indexes
        builder
            .HasIndex(u => u.Username)
            .IsUnique();
        
        // ToDo: create index for email
        
        // Navigation Properties
        builder
            .HasMany(u => u.UserChats)
            .WithOne(uc => uc.User)
            .HasForeignKey(uc => uc.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasMany(u => u.Messages)
            .WithOne(m => m.Sender)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(u => u.UserChats)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(u => u.Messages)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}