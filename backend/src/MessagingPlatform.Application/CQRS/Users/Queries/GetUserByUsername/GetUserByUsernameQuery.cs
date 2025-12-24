using MessagingPlatform.Application.Abstractions;

namespace MessagingPlatform.Application.CQRS.Users.Queries.GetUserByUsername;

public record GetUserByUsernameQuery(string Username) : IQuery;