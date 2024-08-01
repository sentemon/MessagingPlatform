using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using MessagingPlatform.Application.Common.Interfaces;
using MessagingPlatform.Application.Common.Models;
using Microsoft.Extensions.Options;

namespace MessagingPlatform.Application.Services;

public class Auth0Service : IAuth0Service
{
    private readonly Auth0Settings _auth0Settings;

    public Auth0Service(IOptions<Auth0Settings> auth0Settings)
    {
        _auth0Settings = auth0Settings.Value;
    }

    public async Task<string> SignupUser(string email, string password)
    {
        var auth0Client = new AuthenticationApiClient(new Uri($"https://{_auth0Settings.Domain}/"));
        var signUpRequest = new SignupUserRequest
        {
            ClientId = _auth0Settings.ClientId,
            Email = email,
            Password = password,
            Connection = "Username-Password-Authentication",
        };

        var signUpResponse = await auth0Client.SignupUserAsync(signUpRequest);
        return "auth0|" + signUpResponse.Id;
    }
    
    public async Task<string> LoginUser(string email, string password)
    {
        var auth0Client = new AuthenticationApiClient(new Uri($"https://{_auth0Settings.Domain}/"));
        var tokenRequest = new ResourceOwnerTokenRequest
        {
            ClientId = _auth0Settings.ClientId,
            ClientSecret = _auth0Settings.ClientSecret,
            Realm = "Username-Password-Authentication",
            Scope = "openid profile",
            Username = email,
            Password = password
        };

        var tokenResponse = await auth0Client.GetTokenAsync(tokenRequest);
        return tokenResponse.AccessToken;
    }

    public async Task<bool> DeleteUser(string id)
    {
        var auth0Client = new AuthenticationApiClient(new Uri($"https://{_auth0Settings.Domain}/"));

        var tokenRequest = new ClientCredentialsTokenRequest
        {
            ClientId = _auth0Settings.ClientId,
            ClientSecret = _auth0Settings.ClientSecret,
            Audience = $"https://{_auth0Settings.Domain}/api/v2/"
        };

        var tokenResponse = await auth0Client.GetTokenAsync(tokenRequest);

        var managementClient = new ManagementApiClient(tokenResponse.AccessToken, new Uri($"https://{_auth0Settings.Domain}/api/v2"));


        await managementClient.Users.DeleteAsync(id);

        return true;
    }
}