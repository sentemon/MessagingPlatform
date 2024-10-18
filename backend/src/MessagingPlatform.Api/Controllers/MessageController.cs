using System.Security.Claims;
using AutoMapper;
using MediatR;
using MessagingPlatform.Application.Common.Models.MessageDTOs;
using MessagingPlatform.Application.CQRS.Messages.Commands.AddMessage;
using MessagingPlatform.Application.CQRS.Messages.Commands.DeleteMessage;
using MessagingPlatform.Application.CQRS.Messages.Commands.UpdateMessage;
using MessagingPlatform.Application.CQRS.Messages.Queries.GetAllMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingPlatform.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public MessageController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    // ToDo: use instead of navigation property "Messages" in Chat entity for better productivity
    [HttpGet("getall")]
    public async Task<IActionResult> GetAll(Guid chatId)
    {
        var messages = await _mediator.Send(new GetAllMessagesQuery(chatId));

        return Ok(messages);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add(CreateMessageDto createMessage)
    {
        var senderId = Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.Sid).Value);
        
        var message = await _mediator.Send(new AddMessageCommand(createMessage, senderId));

        var messageDto = _mapper.Map<GetMessageDto>(message);
        
        return Ok(messageDto);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateMessageDto updateMessage)
    {
        var userId = Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.Sid).Value);
        
        var updatedMessage = await _mediator.Send(new UpdateMessageCommand(updateMessage, userId));

        return Ok(updatedMessage);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteMessageDto deleteMessage)
    {
        var result = await _mediator.Send(new DeleteMessageCommand(deleteMessage));

        return Ok(result);
    }
}