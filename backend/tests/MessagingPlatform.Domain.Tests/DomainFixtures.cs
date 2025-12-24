using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Enums;

namespace MessagingPlatform.Domain.Tests;

public static class DomainFixtures
{
    public static User CreateUser(
        string firstName = "John",
        string lastName = "Doe",
        string username = "john.doe",
        string email = "john.doe@example.com",
        string passwordHash = "hash")
    {
        return User.Create(firstName, lastName, username, email, passwordHash, DateTime.UtcNow);
    }

    public static Chat CreateChat(ChatType chatType = ChatType.Group, string title = "Test Chat")
    {
        return Chat.Create(chatType, title);
    }
}
