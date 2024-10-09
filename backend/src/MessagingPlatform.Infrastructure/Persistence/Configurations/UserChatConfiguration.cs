using MessagingPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessagingPlatform.Infrastructure.Persistence.Configurations;

public class UserChatConfiguration : IEntityTypeConfiguration<UserChat>
{
    public void Configure(EntityTypeBuilder<UserChat> builder)
    {
        builder.HasKey(uc => new
        {
            uc.UserId,
            uc.ChatId
        });

        builder.Property(uc => uc.JoinedAt)
            .ValueGeneratedOnAdd();

        builder.Property(uc => uc.Rights)
            .IsRequired();

        builder.Property(uc => uc.Role)
            .IsRequired();
        
        // Navigation Properties
        builder
            .HasOne(uc => uc.User)
            .WithMany(u => u.UserChats)
            .HasForeignKey(uc => uc.UserId)
            .OnDelete(DeleteBehavior.Cascade); 

        builder
            .HasOne(uc => uc.Chat)
            .WithMany(c => c.UserChats)
            .HasForeignKey(uc => uc.ChatId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}