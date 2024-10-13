using MediatR;
using MessagingPlatform.Application.Common.Interfaces;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Enums;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Chats.Commands.CreateChat;

public class CreateChatCommandHandler : IRequestHandler<CreateChatCommand, Chat>
{
    private readonly IChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;
    private readonly IChatService _chatService;

    public CreateChatCommandHandler(IChatRepository chatRepository, IUserRepository userRepository, IChatService chatService)
    {
        _chatRepository = chatRepository;
        _userRepository = userRepository;
        _chatService = chatService;
    }

    public async Task<Chat> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        var userIds = new List<Guid>();
        foreach (var username in request.Usernames)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user != null)
            {
                userIds.Add(user.Id);
            }
        }

        return request.Chat.ChatType switch
        {
            ChatType.Private => await _chatService.CreatePrivateChatAsync(request.CreatorId, userIds[0]), // хз или правильно
            ChatType.Group => await _chatService.CreateGroupChatAsync(request.CreatorId, userIds),
            ChatType.Channel => await _chatService.CreateChannelChatAsync(request.CreatorId, userIds),
            _ => throw new ArgumentException("Unknown chat type")
        };
    }
}