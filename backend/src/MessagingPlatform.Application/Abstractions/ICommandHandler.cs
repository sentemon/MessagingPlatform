using MessagingPlatform.Application.Common;

namespace MessagingPlatform.Application.Abstractions;

public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand
{
    Task<IResult<TResponse, Error>> Handle(TCommand command);
}