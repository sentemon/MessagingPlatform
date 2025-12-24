using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common;
using MessagingPlatform.Application.Common.Interfaces;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Enums;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.CQRS.Chats.Commands.CreateChat;

public class CreateChatCommandHandler : ICommandHandler<CreateChatCommand, Chat>
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

    public async Task<IResult<Chat, Error>> Handle(CreateChatCommand command)
    {
        var userIds = new List<Guid>();
        foreach (var username in command.Usernames)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user != null)
            {
                userIds.Add(user.Id);
            }
        }

        if (userIds.Count == 0)
        {
            return Result<Chat>.Failure(new Error("No valid users found"));
        }

        var chat = command.ChatType switch
        {
            ChatType.Private => await _chatService.CreatePrivateChatAsync(command.CreatorId, userIds[0]),
            ChatType.Group => await _chatService.CreateGroupChatAsync(command.CreatorId, userIds),
            ChatType.Channel => await _chatService.CreateChannelChatAsync(command.CreatorId, userIds),
            _ => throw new ArgumentException("Unknown chat type")
        };

        return Result<Chat>.Success(chat);
    }
}