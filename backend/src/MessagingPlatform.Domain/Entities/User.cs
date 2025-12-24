using MessagingPlatform.Domain.Primitives;

namespace MessagingPlatform.Domain.Entities;

public sealed class User : AggregateRoot
{
    private readonly List<UserChat> _userChats = new();
    private readonly List<Message> _messages = new();

    private User()
    {
    }

    private User(Guid id, string firstName, string lastName, string username, string email, string passwordHash, DateTime createdAt)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        FirstName = NormalizeName(firstName, "first name");
        LastName = NormalizeName(lastName, "last name");
        Username = NormalizeUsername(username);
        Email = NormalizeEmail(email);
        SetPasswordHash(passwordHash);
        AccountCreatedAt = createdAt;
    }

    public string FirstName { get; private set; } = string.Empty;
    
    public string LastName { get; private set; } = string.Empty;
    
    public string Username { get; private set; } = string.Empty;
    
    public string Email { get; private set; } = string.Empty;
    
    public string PasswordHash { get; private set; } = string.Empty;
    
    public string? Bio { get; private set; }
    
    public bool? IsOnline { get; private set; }

    public DateTime AccountCreatedAt { get; private set; }
    
    public IReadOnlyCollection<UserChat> UserChats => _userChats.AsReadOnly();

    public IReadOnlyCollection<Message> Messages => _messages.AsReadOnly();

    public static User Create(
        string firstName,
        string lastName,
        string username,
        string email,
        string passwordHash,
        DateTime? createdAtUtc = null,
        Guid? id = null)
    {
        return new User(id ?? Guid.NewGuid(), firstName, lastName, username, email, passwordHash, createdAtUtc ?? DateTime.UtcNow);
    }

    public void UpdateProfile(string firstName, string lastName, string? bio)
    {
        FirstName = NormalizeName(firstName, "first name");
        LastName = NormalizeName(lastName, "last name");
        Bio = string.IsNullOrWhiteSpace(bio) ? null : bio.Trim();
    }

    public void UpdateUsername(string username)
    {
        Username = NormalizeUsername(username);
    }

    public void UpdateEmail(string email)
    {
        Email = NormalizeEmail(email);
    }

    public void SetPasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new DomainException("Password hash cannot be empty.");
        }

        PasswordHash = passwordHash;
    }

    public void SetOnlineStatus(bool? isOnline)
    {
        IsOnline = isOnline;
    }

    internal void AttachToChat(UserChat userChat)
    {
        if (!_userChats.Contains(userChat))
        {
            _userChats.Add(userChat);
        }
    }

    internal void AttachMessage(Message message)
    {
        if (!_messages.Contains(message))
        {
            _messages.Add(message);
        }
    }

    private static string NormalizeName(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException($"User {fieldName} cannot be empty.");
        }

        return value.Trim();
    }

    private static string NormalizeUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new DomainException("Username cannot be empty.");
        }

        return username.Trim();
    }

    private static string NormalizeEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new DomainException("Email cannot be empty.");
        }

        return email.Trim().ToLowerInvariant();
    }
}