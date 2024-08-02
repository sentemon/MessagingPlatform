using MessagingPlatform.Application.Common.Models;

namespace MessagingPlatform.Application.Common.Interfaces;

public interface IUserService
{
    public Task<Guid> SignUp(SignUpDto signUpDto);
    public Task<Guid> SignIn(SignInDto signInDto);

}