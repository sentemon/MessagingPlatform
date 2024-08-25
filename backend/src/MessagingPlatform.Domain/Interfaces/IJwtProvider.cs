using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Domain.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(User user);
    (string Sid, string Name) ExtractClaimsValues(string token);
}