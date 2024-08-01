namespace MessagingPlatform.Application.Common.Interfaces;

public interface IAuth0Service
{
    public Task<string> SignupUser(string email, string password);
    public Task<string> LoginUser(string email, string password);
    public Task<bool> DeleteUser(string id);
}