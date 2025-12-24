using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Messages.Commands.DeleteMessage;

public class DeleteMessageCommandHandler : ICommandHandler<DeleteMessageCommand, bool>
{
    private readonly IMessageRepository _messageRepository;

    public DeleteMessageCommandHandler(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public async Task<IResult<bool, Error>> Handle(DeleteMessageCommand command)
    {
        var result = await _messageRepository.DeleteMessage(command.DeleteMessage.SenderId, command.DeleteMessage.MessageId);

        return Result<bool>.Success(result);
    }
}