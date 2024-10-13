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
        var userIds = new List<Guid>();
        foreach (var username in request.Usernames)
        {
            var user = await _userRepository.GetByUsernameAsync(username);

            if (user == null)
            {
                continue;
            }
            
            userIds.Add(user.Id);
        }
        
        var users = (await _userRepository.GetUsersByIdsAsync(userIds)).ToList();
        
        var chat = new Chat
        {
            Id = Guid.NewGuid(),
            ChatType = request.Chat.ChatType,
            Title = request.Chat.Title
        };
        
        var userChats = users.Select(user => new UserChat
        {
            UserId = user.Id,
            ChatId = chat.Id,
            JoinedAt = DateTime.UtcNow,
            User = user,
            Chat = chat
        }).ToList();
        
        chat.UserChats = userChats;
        
        var createdChat = await _chatRepository.CreateAsync(chat);
        
        return createdChat;
    }
}