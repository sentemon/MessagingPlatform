using MessagingPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessagingPlatform.Infrastructure.Persistence.Configurations;

public class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.ChatType)
            .IsRequired();

        builder.Property(c => c.UserChats)
            .IsRequired(false);
        
        builder.Property(c => c.Messages)
            .IsRequired(false);

        builder.Property(c => c.CreatorId)
            .IsRequired();
        
        builder.Property(c => c.Creator)
            .IsRequired();

        builder.Property(c => c.Title)
            .HasMaxLength(50)
            .IsRequired();

        // Navigation Properties
        builder
            .HasOne(c => c.Creator)
            .WithMany()
            .HasForeignKey(c => c.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(c => c.Messages)
            .WithOne(m => m.Chat)
            .HasForeignKey(m => m.ChatId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(c => c.UserChats)
            .WithOne(uc => uc.Chat)
            .HasForeignKey(uc => uc.ChatId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}