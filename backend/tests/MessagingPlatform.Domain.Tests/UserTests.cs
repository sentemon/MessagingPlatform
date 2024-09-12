using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Domain.Tests;

public class UserTests
{
    [Fact]
    public void User_ShouldBeInitializedCorrectly()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Username = "johndoe",
            Email = "johndoe@example.com",
            PasswordHash = "hashedPassword",
            AccountCreatedAt = DateTime.UtcNow
        };

        // Act & Assert
        Assert.NotNull(user);
        Assert.Equal("John", user.FirstName);
        Assert.Equal("Doe", user.LastName);
        Assert.Equal("johndoe", user.Username);
        Assert.Equal("johndoe@example.com", user.Email);
    }
}