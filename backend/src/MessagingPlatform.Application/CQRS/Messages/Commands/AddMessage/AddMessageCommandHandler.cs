using MediatR;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Messages.Commands.AddMessage;

public class AddMessageCommandHandler : IRequestHandler<AddMessageCommand, Message>
{
    private readonly IMessageRepository _messageRepository;

    public AddMessageCommandHandler(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }
        
    public async Task<Message> Handle(AddMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await _messageRepository.CreateAsync(
            senderId: request.SenderId,
            chatId: request.AddMessage.ChatId, 
            content: request.AddMessage.Content
        );

        return message;
    }
}