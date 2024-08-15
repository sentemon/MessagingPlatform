using MediatR;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid Id) : IRequest<User>;