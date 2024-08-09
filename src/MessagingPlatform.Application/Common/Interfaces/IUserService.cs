using MessagingPlatform.Application.Common.Models;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.Common.Interfaces;

public interface IUserService
{
    public Task<Guid> Create(AddUserDto? signUpDto);
    public Task<bool> IsExist(SignInDto? signInDto);
    public Task<User?> GetUserByIdAsync(Guid userId);

    public Task<bool> Update(User user);

}