namespace MessagingPlatform.Application.Common.Models;

public class UpdateUserDto
{
    public Guid Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    // public string PasswordHash { get; set; }

    public string? Bio { get; set; }
}