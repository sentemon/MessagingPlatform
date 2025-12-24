namespace MessagingPlatform.Infrastructure.Interfaces;

public interface ICookieService
{
    void Append(string key, string value);
    void Delete(string key);
}