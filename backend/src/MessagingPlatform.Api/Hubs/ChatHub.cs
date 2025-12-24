using System.Security.Claims;
using MessagingPlatform.Application.Common.Models.MessageDTOs;
using MessagingPlatform.Application.CQRS.Messages.Commands.AddMessage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MessagingPlatform.Api.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly AddMessageCommandHandler _addMessageCommandHandler;
    public ChatHub(AddMessageCommandHandler addMessageCommandHandler)
    {
        _addMessageCommandHandler = addMessageCommandHandler;
    }

    public async Task SendMessageToChat([FromBody] CreateMessageDto createMessage)
    {
        try
        {
            var userIdClaim = Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
            if (userIdClaim == null)
            {
                throw new Exception("No Sid claim found for user.");
            }

            var senderId = Guid.Parse(userIdClaim.Value);
            var command = new AddMessageCommand(createMessage, senderId);
            var result = await _addMessageCommandHandler.Handle(command);
            // var messageDto = _mapper.Map<GetMessageDto>(message);

            await Clients.Others.SendAsync("ReceiveMessage", Context.User?.Identity?.Name, result.Response);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error in SendMessageToChat: {ex.Message}");
        }
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}