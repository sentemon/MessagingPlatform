using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using MessagingPlatform.Application.Common.Interfaces;
using MessagingPlatform.Application.Common.Models;
using MessagingPlatform.Domain.Entities;

namespace MessagingPlatform.Application.Services;

public class AccountService : IAccountService
{
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountService(IUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> SignUp(SignUpDto? signUpDto)
    {
        try
        {
            var userId = await _userService.Create(signUpDto);
            return userId != Guid.Empty;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public async Task<bool> SignIn(SignInDto? signInDto)
    {
        var isValidUser = await _userService.IsExist(signInDto);
        
        if (!isValidUser)
        {
            return false;
        }
        
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, signInDto.Username)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
        };
        
        var httpContext = _httpContextAccessor.HttpContext;
        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return true;
    }

    public async Task SignOut()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
