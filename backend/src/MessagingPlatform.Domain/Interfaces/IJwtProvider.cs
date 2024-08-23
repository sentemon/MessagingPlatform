using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Domain.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(User user);
}