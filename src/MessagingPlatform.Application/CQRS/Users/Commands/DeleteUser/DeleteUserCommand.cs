using MediatR;

namespace MessagingPlatform.Application.CQRS.Users.Commands.DeleteUser;

public record DeleteUserCommand(Guid? Id) : IRequest<bool>;
