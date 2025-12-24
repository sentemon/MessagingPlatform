using MessagingPlatform.Application.Common;

namespace MessagingPlatform.Application.Abstractions;

public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery
{
    Task<IResult<TResponse, Error>> Handle(TQuery query);
}