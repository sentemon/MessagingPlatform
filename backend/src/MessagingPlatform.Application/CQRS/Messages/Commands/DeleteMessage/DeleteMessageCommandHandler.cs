using MediatR;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Messages.Commands.DeleteMessage;

public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, bool>
{
    private readonly IMessageRepository _messageRepository;

    public DeleteMessageCommandHandler(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }
    
    public async Task<bool> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        var result = await _messageRepository.DeleteMessage(request.DeleteMessage.SenderId, request.DeleteMessage.MessageId);

        return result;
    }
}