using MessagingPlatform.Application.Common.Models;

namespace MessagingPlatform.Application.Common.Interfaces;

public interface IAccountService
{
    public Task<bool> SignUp(SignUpDto? signUpDto);
    
    public Task<bool> SignIn(SignInDto? signInDto);

    public Task SignOut();
}