using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Primitives;
using Xunit;

namespace MessagingPlatform.Domain.Tests;

public class UserTests
{
    [Fact]
    public void Create_ShouldNormalizeEmailToLower()
    {
        var user = User.Create("John", "Doe", "jdoe", "User@Example.COM", "hash", DateTime.UtcNow);
        Assert.Equal("user@example.com", user.Email);
    }

    [Fact]
    public void Create_ShouldThrow_WhenFirstNameEmpty()
    {
        Assert.Throws<DomainException>(() => User.Create(" ", "Doe", "jdoe", "user@example.com", "hash", DateTime.UtcNow));
    }

    [Fact]
    public void UpdateProfile_ShouldTrimAndHandleEmptyBio()
    {
        var user = DomainFixtures.CreateUser();

        user.UpdateProfile("  Jane  ", "  Smith  ", " ");

        Assert.Equal("Jane", user.FirstName);
        Assert.Equal("Smith", user.LastName);
        Assert.Null(user.Bio);
    }

    [Fact]
    public void UpdateEmail_ShouldLowercase()
    {
        var user = DomainFixtures.CreateUser();
        user.UpdateEmail("NEW@Example.COM");

        Assert.Equal("new@example.com", user.Email);
    }

    [Fact]
    public void SetPasswordHash_ShouldThrow_WhenEmpty()
    {
        var user = DomainFixtures.CreateUser();
        Assert.Throws<DomainException>(() => user.SetPasswordHash(" "));
    }
}
