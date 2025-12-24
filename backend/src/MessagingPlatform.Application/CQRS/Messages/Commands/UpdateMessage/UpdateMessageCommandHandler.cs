using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Messages.Commands.UpdateMessage;

public class UpdateMessageCommandHandler : ICommandHandler<UpdateMessageCommand, Message>
{
    private readonly IMessageRepository _messageRepository;

    public UpdateMessageCommandHandler(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public async Task<IResult<Message, Error>> Handle(UpdateMessageCommand command)
    {
        var message = await _messageRepository.GetById(command.UpdateMessage.MessageId);

        if (!message.CanBeModifiedBy(command.UserId))
        {
            return Result<Message>.Failure(new Error("You do not have rights to update this message."));
        }

        message.UpdateContent(command.UpdateMessage.Content, DateTime.UtcNow);

        var updatedMessage = await _messageRepository.UpdateAsync(message);

        return Result<Message>.Success(updatedMessage);
    }
}