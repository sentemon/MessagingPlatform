using MediatR;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Chats.Commands.CreateChat;

public class CreateChatCommandHandler : IRequestHandler<CreateChatCommand, Chat>
{
    private readonly IChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;

    public CreateChatCommandHandler(IChatRepository chatRepository, IUserRepository userRepository)
    {
        _chatRepository = chatRepository;
        _userRepository = userRepository;
    }

    public async Task<Chat> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        var users = (await _userRepository.GetUsersByIdsAsync(request.CreateChat.UserIds)).ToList();
            
        if (users.Count != request.CreateChat.UserIds.Count)
        {
            throw new ArgumentException("One or more users do not exist.");
        }

        var chat = new Chat
        {
            Id = Guid.NewGuid(),
            ChatType = request.CreateChat.ChatType,
            CreatorId = request.CreateChat.CreatorId,
            Title = request.CreateChat.Title,
            Users = users
        };

        var createdChat = await _chatRepository.CreateAsync(chat);
        return createdChat;
    }
}