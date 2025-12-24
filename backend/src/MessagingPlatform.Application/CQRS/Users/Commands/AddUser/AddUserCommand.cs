using MessagingPlatform.Application.Abstractions;

namespace MessagingPlatform.Application.CQRS.Users.Commands.AddUser;

public record AddUserCommand(
    string FirstName,
    string LastName,
    string Username,
    string Email,
    string Password,
    string ConfirmPassword
) : ICommand;
