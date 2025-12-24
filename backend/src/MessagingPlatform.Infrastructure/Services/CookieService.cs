using MessagingPlatform.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;

namespace MessagingPlatform.Infrastructure.Services;

public class CookieService : ICookieService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public void Append(string key, string value)
    {
        var cookieOptions = new CookieOptions
        {
            Path = "/",
            HttpOnly = true,
            Secure = false, // Change in production
            SameSite = SameSiteMode.Lax,
        };
            
        _httpContextAccessor.HttpContext?.Response.Cookies.Append(key, value, cookieOptions);
    }

    public void Delete(string key)
    {
        var cookieOptions = new CookieOptions
        {
            Path = "/",
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax
        };
        
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete(key, cookieOptions);
    }
}

