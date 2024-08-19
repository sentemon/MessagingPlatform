using MediatR;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.Users.Queries.GetUserByUsername;

public record GetUserByUsenameQuery(string Username) : IRequest<User>;