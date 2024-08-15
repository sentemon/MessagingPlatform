using MediatR;
using MessagingPlatform.Application.Common.Models.MessageDTOs;
using MessagingPlatform.Application.CQRS.Messages.Commands.AddMessage;
using MessagingPlatform.Application.CQRS.Messages.Commands.DeleteMessage;
using MessagingPlatform.Application.CQRS.Messages.Commands.UpdateMessage;
using MessagingPlatform.Application.CQRS.Messages.Queries.GetAllMessages;
using MessagingPlatform.Application.CQRS.Messages.Queries.GetByUsername;
using Microsoft.AspNetCore.Mvc;

namespace MessagingPlatform.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly IMediator _mediator;

    public MessageController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("getall")]
    public async Task<IActionResult> GetAll()
    {
        var messages = await _mediator.Send(new GetAllMessagesQuery());

        return Ok(messages);
    }
    
    [HttpGet("getbyusername")]
    public async Task<IActionResult> GetByUsername([FromBody] string senderId, string receiverId)  // ToDo: doesn't work correctly
    {
        var messages = await _mediator.Send(new GetMessageByUsernameQuery(senderId, receiverId));

        return Ok(messages);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddMessageDto addMessage)
    {
        var message = await _mediator.Send(new AddMessageCommand(addMessage));

        return Ok(message);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateMessageDto updateMessage)
    {
        var updatedMessage = await _mediator.Send(new UpdateMessageCommand(updateMessage));

        return Ok(updatedMessage);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteMessageDto deleteMessage)
    {
        var result = await _mediator.Send(new DeleteMessageCommand(deleteMessage));

        return Ok(result);
    }
}