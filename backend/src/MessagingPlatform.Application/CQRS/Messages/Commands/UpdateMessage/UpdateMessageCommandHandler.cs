using MediatR;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Messages.Commands.UpdateMessage;

public class UpdateMessageCommandHandler : IRequestHandler<UpdateMessageCommand, Message>
{
    private readonly IMessageRepository _messageRepository;

    public UpdateMessageCommandHandler(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }
    
    public async Task<Message> Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await _messageRepository.GetById(request.UpdateMessage.MessageId);

        var isItUserMessage = request.UserId == message.SenderId;

        if (!isItUserMessage)
        {
            throw new Exception("You do not have rights to update this message.");
        }
        
        message.Content = request.UpdateMessage.Content;
        message.UpdatedAt = DateTime.UtcNow;

        var updatedMessage = await _messageRepository.UpdateAsync(message);

        return updatedMessage;
    }
}