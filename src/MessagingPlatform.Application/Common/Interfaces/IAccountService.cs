using MessagingPlatform.Application.Common.Models.UserDTOs;
using MessagingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace MessagingPlatform.Application.Common.Interfaces;

public interface IAccountService
{
    Task<bool> SignUp(AddUserDto? signUpDto);

    Task<string?> SignIn(SignInDto signInDto);

    Task SignOut();

    string GetCurrentUsername(HttpContext context);

    Task<User?> GetCurrentUser(HttpContext context);
}