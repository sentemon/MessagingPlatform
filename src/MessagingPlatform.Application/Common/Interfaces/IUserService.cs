using MessagingPlatform.Application.Common.Models;

namespace MessagingPlatform.Application.Common.Interfaces;

public interface IUserService
{
    public Task<Guid> Create(SignUpDto? signUpDto);
    public Task<bool> IsExist(SignInDto? signInDto);

}