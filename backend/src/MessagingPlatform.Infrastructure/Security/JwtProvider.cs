using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MessagingPlatform.Infrastructure.Security;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }
    
    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Sid, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddHours(_options.ExpiresHours));

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }

    public (string Sid, string Name) ExtractClaimsValues(string token)
    {
        var principal = GetPrincipalFromToken(token);

        if (principal == null)
        {
            return (null, null)!;
        }

        var sid = principal.Claims.Where(c => c.Type == ClaimTypes.Sid).
                                        Select(c => c.Value)
                                        .SingleOrDefault();
        
        var name = principal.Claims.Where(c => c.Type == ClaimTypes.Name)
                                        .Select(c => c.Value)
                                        .SingleOrDefault();

        return (sid, name)!;
    }

    private ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_options.SecretKey);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

            return principal;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}