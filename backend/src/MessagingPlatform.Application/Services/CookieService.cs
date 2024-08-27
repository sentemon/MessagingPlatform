using MessagingPlatform.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace MessagingPlatform.Application.Services;

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
            HttpOnly = true,
            Secure = false, // ToDo: Change in production
            SameSite = SameSiteMode.None
        };
            
        _httpContextAccessor.HttpContext?.Response.Cookies.Append(key, value, cookieOptions);
    }

    public void Delete(string key)
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete(key);
    }
}

