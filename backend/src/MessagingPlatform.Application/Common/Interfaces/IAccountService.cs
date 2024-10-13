using MessagingPlatform.Application.Common.Models.UserDTOs;
using MessagingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace MessagingPlatform.Application.Common.Interfaces;

public interface IAccountService
{
    Task<string> SignUp(CreateUserDto? signUpDto);
    Task<string> SignIn(SignInDto signInDto);
}