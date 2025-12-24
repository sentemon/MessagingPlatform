using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Messages.Commands.AddMessage;

public class AddMessageCommandHandler : ICommandHandler<AddMessageCommand, Message>
{
    private readonly IMessageRepository _messageRepository;

    public AddMessageCommandHandler(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public async Task<IResult<Message, Error>> Handle(AddMessageCommand command)
    {
        var message = await _messageRepository.CreateAsync(
            senderId: command.SenderId,
            chatId: command.CreateMessage.ChatId, 
            content: command.CreateMessage.Content
        );

        return Result<Message>.Success(message);
    }
}