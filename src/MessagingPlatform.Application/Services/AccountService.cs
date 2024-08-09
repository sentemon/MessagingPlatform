using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MessagingPlatform.Application.Common.Interfaces;
using MessagingPlatform.Application.Common.Models;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;

namespace MessagingPlatform.Application.Services;

public class AccountService : IAccountService
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AccountService(IUserService userService, IUserRepository userRepository, IConfiguration configuration)
    {
        _userService = userService;
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<bool> SignUp(AddUserDto? signUpDto)
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

    public async Task<string?> SignIn(SignInDto signInDto)
    {
        var isValidUser = await _userService.IsExist(signInDto);
        
        if (!isValidUser)
        {
            return null;
        }
        
        var user = await _userRepository.GetByUsernameAsync(signInDto.Username);
        
        if (user == null)
        {
            return null;
        }
        
        var token = GenerateJwtToken(user);
        return token;
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]!);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var key = new SymmetricSecurityKey(secretKey);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpirationMinutes"]!)),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task SignOut()
    {
        await Task.CompletedTask;
    }

    public string GetCurrentUsername(HttpContext context)
    {
        var username = context.User.FindFirst(ClaimTypes.Name)?.Value;
        return username ?? "Not Found";
    }

    public async Task<User?> GetCurrentUser(HttpContext context)
    {
        var username = GetCurrentUsername(context);
        return await _userRepository.GetByUsernameAsync(username);
    }
}
