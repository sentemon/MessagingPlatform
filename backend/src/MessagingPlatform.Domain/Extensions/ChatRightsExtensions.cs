using MessagingPlatform.Domain.Enums;

namespace MessagingPlatform.Domain.Extensions;

public static class ChatRightsExtensions
{
    public static ChatRights All => ChatRights.Read | ChatRights.Write | ChatRights.Update | ChatRights.Delete;

    public static bool HasRight(this ChatRights userRights, ChatRights rights)
    {
        return (userRights & rights) == rights;
    }
}