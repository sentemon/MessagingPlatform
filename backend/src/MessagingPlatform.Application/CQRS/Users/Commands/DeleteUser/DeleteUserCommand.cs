using MessagingPlatform.Application.Abstractions;

namespace MessagingPlatform.Application.CQRS.Users.Commands.DeleteUser;

public record DeleteUserCommand(Guid Id) : ICommand;
