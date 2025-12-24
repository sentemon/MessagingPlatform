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

    public async Task SendMessageToChat(CreateMessageDto createMessage)
    {
        var userIdClaim = Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
        if (userIdClaim == null)
        {
            await Clients.Caller.SendAsync("ReceiveError", "Unauthorized: missing Sid claim");
            return;
        }

        if (createMessage == null || createMessage.ChatId == Guid.Empty || string.IsNullOrWhiteSpace(createMessage.Content))
        {
            await Clients.Caller.SendAsync("ReceiveError", "Invalid message payload");
            return;
        }

        var senderId = Guid.Parse(userIdClaim.Value);
        var command = new AddMessageCommand(createMessage, senderId);
        var result = await _addMessageCommandHandler.Handle(command);

        if (!result.IsSuccess || result.Response is null)
        {
            await Clients.Caller.SendAsync("ReceiveError", result.Error?.Message ?? "Failed to send message");
            return;
        }

        var senderName = Context.User?.Identity?.Name ?? string.Empty;

        var payload = new
        {
            id = result.Response.Id,
            chatId = result.Response.ChatId,
            content = result.Response.Content,
            sentAt = result.Response.SentAt,
            updatedAt = result.Response.UpdatedAt,
            isRead = result.Response.IsRead,
            sender = new
            {
                id = result.Response.SenderId,
                firstName = result.Response.Sender?.FirstName,
                lastName = result.Response.Sender?.LastName,
                username = result.Response.Sender?.Username
            }
        };

        // echo to sender and broadcast to others to keep local state in sync
        await Clients.Caller.SendAsync("ReceiveMessage", senderName, payload);
        await Clients.Others.SendAsync("ReceiveMessage", senderName, payload);
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