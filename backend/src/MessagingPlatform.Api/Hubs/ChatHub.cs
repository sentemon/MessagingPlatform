using MessagingPlatform.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MessagingPlatform.Api.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IUserRepository _userRepository;

    public ChatHub(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task SendMessageToChat(string targetUsername, string message)
    {
        var targetUser = await _userRepository.GetByUsernameAsync(targetUsername);

        if (targetUser != null)
        {
            await Clients.Client(targetUser.ConnectionId).SendAsync("ReceiveMessage", Context.User?.Identity?.Name, message);
        }
    }

    public override async Task OnConnectedAsync()
    {
        var username = Context.User?.Identity?.Name;
        var user = await _userRepository.GetByUsernameAsync(username);
        
        if (user == null)
        {
            throw new Exception();
        }
        user.ConnectionId = Context.ConnectionId;
        await base.OnConnectedAsync();
    }
}