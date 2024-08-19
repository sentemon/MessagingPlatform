using MediatR;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Messages.Queries.GetByUsername;

public class GetMessageByUsernameQueryHandler : IRequestHandler<GetMessageByUsernameQuery, IQueryable<Message>>
{
    private readonly IMessageRepository _messageRepository;

    public GetMessageByUsernameQueryHandler(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }
    
    public async Task<IQueryable<Message>> Handle(GetMessageByUsernameQuery request, CancellationToken cancellationToken)
    {
        var messages = await _messageRepository.GetByUsernameAsync(request.SenderUsername, request.ReceiverUsername);

        return messages;
    }
}