using MessagingPlatform.Application.Abstractions;

namespace MessagingPlatform.Application.CQRS.Users.Commands.UpdateUser;

public record UpdateUserCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Bio
) : ICommand;