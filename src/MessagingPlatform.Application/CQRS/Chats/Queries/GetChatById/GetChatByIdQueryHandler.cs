using MediatR;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MessagingPlatform.Application.CQRS.Chats.Queries.GetChatById
{
    public class GetChatByIdQueryHandler : IRequestHandler<GetChatByIdQuery, Chat>
    {
        private readonly IChatRepository _chatRepository;

        public GetChatByIdQueryHandler(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<Chat> Handle(GetChatByIdQuery request, CancellationToken cancellationToken)
        {
            var chat = await _chatRepository.GetByIdAsync(request.Id);

            return chat;
        }
    }
}