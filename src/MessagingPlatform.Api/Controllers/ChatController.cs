using MediatR;
using MessagingPlatform.Application.CQRS.Chats.Commands.CreateChat;
// using MessagingPlatform.Application.CQRS.Chats.Commands.DeleteChat;
// using MessagingPlatform.Application.CQRS.Chats.Commands.UpdateChat;
using MessagingPlatform.Application.CQRS.Chats.Queries.GetChatById;
using MessagingPlatform.Application.CQRS.Chats.Queries.GetChats;
using MessagingPlatform.Application.Common.Models.ChatDTOs;
using Microsoft.AspNetCore.Mvc;

namespace MessagingPlatform.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatController(IMediator mediator)
    {
        _mediator = mediator;
    }
        
    [HttpPost("create")]
    public async Task<IActionResult> CreateChat([FromBody] CreateChatDto createChatDto)
    {
        var chat = await _mediator.Send(new CreateChatCommand(createChatDto));

        return CreatedAtAction(nameof(GetChatById), new { id = chat.Id }, chat);
    }
        
    [HttpGet("getchats")]
    public async Task<IActionResult> GetChats()
    {
        var chats = await _mediator.Send(new GetChatsQuery());

        return Ok(chats);
    }
        
    [HttpGet("getchatbyid/{id}")]
    public async Task<IActionResult> GetChatById(Guid id)
    {
        var chat = await _mediator.Send(new GetChatByIdQuery(id));

        if (chat == null)
            return NotFound();

        return Ok(chat);
    }
        
    // [HttpPut("update")]
    // public async Task<IActionResult> UpdateChat([FromBody] UpdateChatDto updateChatDto)
    // {
    //     var updatedChat = await _mediator.Send(new UpdateChatCommand(updateChatDto));
    //
    //     return Ok(updatedChat);
    // }
        
    // [HttpDelete("delete")]
    // public async Task<IActionResult> DeleteChat([FromBody] DeleteChatDto deleteChatDto)
    // {
    //     var result = await _mediator.Send(new DeleteChatCommand(deleteChatDto));
    //
    //     if (result)
    //         return NoContent();
    //
    //     return NotFound();
    // }
}