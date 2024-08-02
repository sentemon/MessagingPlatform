namespace MessagingPlatform.Application.Common.Interfaces;

public interface IPasswordHasherService
{
    public string Hash(string password);

    public bool Verify(string password, string hashedPassword);
}