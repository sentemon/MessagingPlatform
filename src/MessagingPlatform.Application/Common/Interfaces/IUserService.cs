using MessagingPlatform.Application.Common.Models.UserDTOs;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.Common.Interfaces;

public interface IUserService
{
    Task<Guid> Create(AddUserDto? signUpDto);
    Task<bool> IsExist(SignInDto? signInDto);
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<bool> Update(User user);

}