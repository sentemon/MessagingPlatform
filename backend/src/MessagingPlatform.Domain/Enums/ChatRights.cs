namespace MessagingPlatform.Domain.Enums;

[Flags]
public enum ChatRights
{
    None = 0,
    Read = 1,
    Write = 2,
    Update = 4,
    Delete = 8
}