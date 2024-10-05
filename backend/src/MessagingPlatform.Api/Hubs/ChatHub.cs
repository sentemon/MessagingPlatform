using System.Security.Claims;
using AutoMapper;
using MediatR;
using MessagingPlatform.Application.Common.Models.MessageDTOs;
using MessagingPlatform.Application.CQRS.Messages.Commands.AddMessage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MessagingPlatform.Api.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ChatHub(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task SendMessageToChat([FromBody] AddMessageDto addMessage)
    {
        try
        {
            var userIdClaim = Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
            if (userIdClaim == null)
            {
                throw new Exception("No Sid claim found for user.");
            }

            var senderId = Guid.Parse(userIdClaim.Value);
            var message = await _mediator.Send(new AddMessageCommand(addMessage, senderId));
            var messageDto = _mapper.Map<MessageDto>(message);

            await Clients.Others.SendAsync("ReceiveMessage", Context.User?.Identity?.Name, messageDto);
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