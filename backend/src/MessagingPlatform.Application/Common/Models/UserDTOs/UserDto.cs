namespace MessagingPlatform.Application.Common.Models.UserDTOs;

public class UserDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public string? Bio { get; set; }
    public bool? IsOnline { get; set; }
    public required DateTime AccountCreatedAt { get; init; }
}