using MessagingPlatform.Application.Abstractions;

namespace MessagingPlatform.Application.CQRS.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid Id) : IQuery;