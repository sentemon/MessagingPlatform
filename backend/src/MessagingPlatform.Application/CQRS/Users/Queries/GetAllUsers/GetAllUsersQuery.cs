using MediatR;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.CQRS.Users.Queries.GetAllUsers;

public record GetAllUsersQuery() : IRequest<IQueryable<User>>;