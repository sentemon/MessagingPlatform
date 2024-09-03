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

    public async Task SendMessageToUser(string targetUsername, string message)
    {
        var targetUser = await _userRepository.GetByUsernameAsync(targetUsername);

        if (targetUser?.ConnectionId != null)
        {
            Console.WriteLine($"Sending message to {targetUsername} with ConnectionId {targetUser.ConnectionId}");
            await Clients.Client(targetUser.ConnectionId).SendAsync("ReceiveMessage", Context.User?.Identity?.Name, message);
        }
        else
        {
            Console.WriteLine($"Failed to send message: target user or connection ID is null for {targetUsername}");
        }
    }


    public static List<string> ConnectedUsers = [];

    public override async Task OnConnectedAsync()
    {
        var username = Context.User?.Identity?.Name;
        var connectionId = Context.ConnectionId;
    
        ConnectedUsers.Add(connectionId);
        Console.WriteLine($"User {username} connected. Total connected users: {ConnectedUsers.Count}");
    
        await base.OnConnectedAsync();
    }

}