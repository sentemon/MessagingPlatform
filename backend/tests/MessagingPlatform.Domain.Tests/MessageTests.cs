using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Enums;
using MessagingPlatform.Domain.Extensions;
using MessagingPlatform.Domain.Primitives;
using Xunit;

namespace MessagingPlatform.Domain.Tests;

public class MessageTests
{
    [Fact]
    public void Create_ShouldThrow_WhenContentEmpty()
    {
        var user = DomainFixtures.CreateUser();
        var chat = DomainFixtures.CreateChat();
        chat.AddParticipant(user, ChatRole.Owner, ChatRightsExtensions.All, DateTime.UtcNow);

        Assert.Throws<DomainException>(() => Message.Create(user, chat, "", DateTime.UtcNow));
    }

    [Fact]
    public void UpdateContent_ShouldSetContentAndTimestamp()
    {
        var user = DomainFixtures.CreateUser(username: "writer", email: "writer@example.com");
        var chat = DomainFixtures.CreateChat();
        chat.AddParticipant(user, ChatRole.Owner, ChatRightsExtensions.All, DateTime.UtcNow);
        var message = Message.Create(user, chat, "Hi", DateTime.UtcNow.AddMinutes(-1));

        var updatedAt = DateTime.UtcNow;
        message.UpdateContent("Updated", updatedAt);

        Assert.Equal("Updated", message.Content);
        Assert.Equal(updatedAt, message.UpdatedAt);
    }

    [Fact]
    public void CanBeModifiedBy_ShouldMatchSender()
    {
        var sender = DomainFixtures.CreateUser(username: "sender", email: "sender@example.com");
        var other = DomainFixtures.CreateUser(username: "other", email: "other@example.com");
        var chat = DomainFixtures.CreateChat();
        chat.AddParticipant(sender, ChatRole.Owner, ChatRightsExtensions.All, DateTime.UtcNow);
        chat.AddParticipant(other, ChatRole.Member, ChatRights.Read | ChatRights.Write, DateTime.UtcNow);

        var message = Message.Create(sender, chat, "Hello", DateTime.UtcNow);

        Assert.True(message.CanBeModifiedBy(sender.Id));
        Assert.False(message.CanBeModifiedBy(other.Id));
    }
}
