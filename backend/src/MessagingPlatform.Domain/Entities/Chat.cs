using MessagingPlatform.Domain.Enums;
using MessagingPlatform.Domain.Extensions;
using MessagingPlatform.Domain.Primitives;

namespace MessagingPlatform.Domain.Entities;

public sealed class Chat : AggregateRoot
{
    private readonly List<UserChat> _userChats = new();
    private readonly List<Message> _messages = new();

    private Chat()
    {
    }

    private Chat(ChatType chatType, string title, Guid id)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        ChatType = chatType;
        Title = NormalizeTitle(title);
    }

    public ChatType ChatType { get; private set; }

    public string Title { get; private set; } = "New Chat";

    public IReadOnlyCollection<UserChat> UserChats => _userChats.AsReadOnly();

    public IReadOnlyCollection<Message> Messages => _messages.AsReadOnly();

    public static Chat Create(ChatType chatType, string title, Guid? id = null)
    {
        return new Chat(chatType, title, id ?? Guid.NewGuid());
    }

    public void Rename(string title)
    {
        Title = NormalizeTitle(title);
    }

    public UserChat AddParticipant(User user, ChatRole role, ChatRights rights, DateTime joinedAtUtc)
    {
        ArgumentNullException.ThrowIfNull(user);

        EnsureCanAddParticipant(user.Id);

        if (_userChats.Any(uc => uc.UserId == user.Id))
        {
            return _userChats.First(uc => uc.UserId == user.Id);
        }

        var participant = UserChat.Create(user, this, role, rights, joinedAtUtc);
        _userChats.Add(participant);
        user.AttachToChat(participant);

        return participant;
    }

    public bool RemoveParticipant(Guid userId)
    {
        var participant = _userChats.FirstOrDefault(uc => uc.UserId == userId);
        if (participant == null)
        {
            return false;
        }

        if (ChatType == ChatType.Private)
        {
            throw new DomainException("Cannot remove users from a private chat.");
        }

        _userChats.Remove(participant);
        participant.Detach();
        return true;
    }

    public UserChat? GetParticipant(Guid userId) => _userChats.FirstOrDefault(uc => uc.UserId == userId);

    public bool CanUserPerform(Guid userId, ChatRights requiredRights)
    {
        return GetParticipant(userId)?.Rights.HasRight(requiredRights) == true;
    }

    public void UpdateParticipantRights(Guid userId, ChatRights rights, ChatRole role)
    {
        var participant = GetParticipant(userId) ?? throw new DomainException("User is not part of this chat.");
        participant.UpdatePermissions(rights, role);
    }

    public Message AddMessage(User sender, string content, DateTime sentAtUtc)
    {
        ArgumentNullException.ThrowIfNull(sender);

        if (!CanUserPerform(sender.Id, ChatRights.Read | ChatRights.Write))
        {
            throw new DomainException("User does not have permission to send messages in this chat.");
        }

        var message = Message.Create(sender, this, content, sentAtUtc);
        _messages.Add(message);
        sender.AttachMessage(message);

        return message;
    }

    private void EnsureCanAddParticipant(Guid userId)
    {
        if (ChatType != ChatType.Private)
        {
            return;
        }

        var uniqueUsers = _userChats.Select(uc => uc.UserId).Distinct().ToList();
        if (uniqueUsers.Contains(userId))
        {
            return;
        }

        if (uniqueUsers.Count >= 2)
        {
            throw new DomainException("A private chat cannot have more than two participants.");
        }
    }

    private static string NormalizeTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainException("Chat title cannot be empty.");
        }

        return title.Trim();
    }
}