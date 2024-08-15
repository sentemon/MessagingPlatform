using MessagingPlatform.Application.Common.Models;
using MessagingPlatform.Application.Common.Models.UserDTOs;
using MessagingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace MessagingPlatform.Application.Common.Interfaces;

public interface IAccountService
{
    public Task<bool> SignUp(AddUserDto? signUpDto);

    public Task<string?> SignIn(SignInDto signInDto);

    public Task SignOut();

    public string GetCurrentUsername(HttpContext context);

    public Task<User?> GetCurrentUser(HttpContext context);
}