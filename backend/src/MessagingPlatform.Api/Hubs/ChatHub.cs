using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MessagingPlatform.Api.Hubs;

[Authorize]
public class ChatHub : Hub
{
    
}