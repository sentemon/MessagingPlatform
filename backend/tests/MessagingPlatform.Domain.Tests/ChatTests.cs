using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Enums;
using MessagingPlatform.Domain.Extensions;
using MessagingPlatform.Domain.Primitives;
using Xunit;

namespace MessagingPlatform.Domain.Tests;

public class ChatTests
{
    [Fact]
    public void CreateChat_ShouldThrow_WhenTitleEmpty()
    {
        Assert.Throws<DomainException>(() => Chat.Create(ChatType.Group, " "));
    }

    [Fact]
    public void PrivateChat_ShouldLimitToTwoDistinctUsers()
    {
        var chat = Chat.Create(ChatType.Private, "Private");
        var user1 = DomainFixtures.CreateUser(username: "u1");
        var user2 = DomainFixtures.CreateUser(username: "u2", email: "u2@example.com");
        var user3 = DomainFixtures.CreateUser(username: "u3", email: "u3@example.com");

        chat.AddParticipant(user1, ChatRole.Owner, ChatRightsExtensions.All, DateTime.UtcNow);
        chat.AddParticipant(user2, ChatRole.Owner, ChatRightsExtensions.All, DateTime.UtcNow);

        Assert.Throws<DomainException>(() => chat.AddParticipant(user3, ChatRole.Member, ChatRights.Read, DateTime.UtcNow));
    }

    [Fact]
    public void AddParticipant_ShouldReturnExisting_WhenSameUser()
    {
        var chat = Chat.Create(ChatType.Group, "Group");
        var user = DomainFixtures.CreateUser();

        var first = chat.AddParticipant(user, ChatRole.Owner, ChatRightsExtensions.All, DateTime.UtcNow);
        var second = chat.AddParticipant(user, ChatRole.Member, ChatRights.Read, DateTime.UtcNow);

        Assert.Same(first, second);
        Assert.Single(chat.UserChats);
    }

    [Fact]
    public void RemoveParticipant_ShouldThrow_ForPrivateChat()
    {
        var chat = Chat.Create(ChatType.Private, "Private");
        var user = DomainFixtures.CreateUser();
        chat.AddParticipant(user, ChatRole.Owner, ChatRightsExtensions.All, DateTime.UtcNow);

        Assert.Throws<DomainException>(() => chat.RemoveParticipant(user.Id));
    }

    [Fact]
    public void AddMessage_ShouldThrow_WhenSenderHasNoRights()
    {
        var chat = Chat.Create(ChatType.Channel, "Channel");
        var viewer = DomainFixtures.CreateUser(username: "viewer", email: "viewer@example.com");
        chat.AddParticipant(viewer, ChatRole.Viewer, ChatRights.Read, DateTime.UtcNow);

        Assert.Throws<DomainException>(() => chat.AddMessage(viewer, "Hello", DateTime.UtcNow));
    }

    [Fact]
    public void AddMessage_ShouldAddMessage_WhenSenderHasWriteRights()
    {
        var chat = Chat.Create(ChatType.Group, "Group");
        var sender = DomainFixtures.CreateUser(username: "writer", email: "writer@example.com");
        chat.AddParticipant(sender, ChatRole.Member, ChatRights.Read | ChatRights.Write, DateTime.UtcNow);

        var message = chat.AddMessage(sender, "Hello", DateTime.UtcNow);

        Assert.Contains(message, chat.Messages);
        Assert.Equal(sender.Id, message.SenderId);
        Assert.Equal(chat.Id, message.ChatId);
        Assert.Equal("Hello", message.Content);
    }

    [Fact]
    public void UpdateParticipantRights_ShouldChangeRoleAndRights()
    {
        var chat = Chat.Create(ChatType.Group, "Group");
        var user = DomainFixtures.CreateUser(username: "member", email: "member@example.com");
        chat.AddParticipant(user, ChatRole.Member, ChatRights.Read, DateTime.UtcNow);

        chat.UpdateParticipantRights(user.Id, ChatRightsExtensions.All, ChatRole.Admin);

        var participant = chat.GetParticipant(user.Id);
        Assert.NotNull(participant);
        Assert.Equal(ChatRole.Admin, participant!.Role);
        Assert.Equal(ChatRightsExtensions.All, participant.Rights);
    }
}
