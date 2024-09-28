using System.Security.Claims;
using AutoMapper;
using MediatR;
using MessagingPlatform.Application.Common.Models.MessageDTOs;
using MessagingPlatform.Application.CQRS.Messages.Commands.AddMessage;
using MessagingPlatform.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MessagingPlatform.Api.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ChatHub(IUserRepository userRepository, IMediator mediator, IMapper mapper)
        {
            _userRepository = userRepository;
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
                Console.WriteLine($"Message sent: {messageDto.Content} from {Context.User?.Identity?.Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SendMessageToChat: {ex.Message}");
            }
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            Console.WriteLine($"User connected: {Context.User?.Identity?.Name}");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var username = Context.User?.Identity?.Name;
            Console.WriteLine($"User disconnected: {username}");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
