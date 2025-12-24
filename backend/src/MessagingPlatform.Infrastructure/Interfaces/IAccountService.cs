namespace MessagingPlatform.Infrastructure.Interfaces;

public interface IAccountService
{
    Task<string> SignUp(string firstName, string lastName, string username, string email, string password, string confirmPassword);
    Task<string> SignIn(string username, string password);
}