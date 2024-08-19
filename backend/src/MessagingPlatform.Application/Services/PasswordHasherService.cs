using MessagingPlatform.Application.Common.Interfaces;

namespace MessagingPlatform.Application.Services;

public class PasswordHasherService : IPasswordHasherService
{
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }

    public bool Verify(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}