using MessagingPlatform.Application.Abstractions;
using MessagingPlatform.Application.Common;
using MessagingPlatform.Domain.Enums;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Extensions;
using MessagingPlatform.Domain.Interfaces;
using MessagingPlatform.Domain.Primitives;

namespace MessagingPlatform.Application.CQRS.UserChats.Commands.AddUserToChat;

public class AddUserToChatCommandHandler : ICommandHandler<AddUserToChatCommand, UserChat>
{
    private readonly IChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;

    public AddUserToChatCommandHandler(IChatRepository chatRepository, IUserRepository userRepository)
    {
        _chatRepository = chatRepository;
        _userRepository = userRepository;
    }

    public async Task<IResult<UserChat, Error>> Handle(AddUserToChatCommand command)
    {
        var chat = await _chatRepository.GetByIdAsync(command.ChatId);
        if (chat == null)
        {
            return Result<UserChat>.Failure(new Error("Chat not found"));
        }

        var user = await _userRepository.GetByIdAsync(command.UserId);
        if (user == null)
        {
            return Result<UserChat>.Failure(new Error("User not found"));
        }

        var (role, rights) = GetDefaults(chat.ChatType);

        try
        {
            var participant = chat.AddParticipant(user, role, rights, DateTime.UtcNow);
            await _chatRepository.UpdateAsync(chat);
            return Result<UserChat>.Success(participant);
        }
        catch (DomainException ex)
        {
            return Result<UserChat>.Failure(new Error(ex.Message));
        }
    }

    private static (ChatRole Role, ChatRights Rights) GetDefaults(ChatType chatType)
    {
        return chatType switch
        {
            ChatType.Private => (ChatRole.Member, ChatRightsExtensions.All),
            ChatType.Group => (ChatRole.Member, ChatRights.Read | ChatRights.Write),
            ChatType.Channel => (ChatRole.Viewer, ChatRights.Read),
            _ => (ChatRole.Member, ChatRights.Read)
        };
    }
}