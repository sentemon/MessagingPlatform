using System.Security.Claims;
using AutoMapper;
using MediatR;
using MessagingPlatform.Application.CQRS.Chats.Commands.CreateChat;
using MessagingPlatform.Application.CQRS.Chats.Commands.DeleteChat;
using MessagingPlatform.Application.CQRS.Chats.Commands.UpdateChat;
using MessagingPlatform.Application.CQRS.Chats.Queries.GetChatById;
using MessagingPlatform.Application.CQRS.Chats.Queries.GetChats;
using MessagingPlatform.Application.Common.Models.ChatDTOs;
using MessagingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingPlatform.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ChatController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
        
    [HttpPost("create")]
    public async Task<IActionResult> CreateChat([FromBody] CreateChatDto createChatDto)
    {
        var chatRequest = _mapper.Map<Chat>(createChatDto);
        
        var usernames = createChatDto.Users;
        usernames.Add(User.Claims.First(c => c.Type == ClaimTypes.Name).Value); // "Added creator of the chat"

        var creatorId = Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.Sid).Value);
        
        
        var chatResponse = await _mediator.Send(new CreateChatCommand(chatRequest, usernames, creatorId));
        
        return CreatedAtAction(nameof(GetChat), new { id = chatResponse.Id }, "You created chat successfully!");

    }
        
    [HttpGet("getall")]
    public async Task<IActionResult> GetChats()
    {
        var userId = Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.Sid).Value);
        
        var chats = await _mediator.Send(new GetChatsQuery(userId));

        return Ok(chats);
    }
        
    [HttpGet("getchat")]
    public async Task<IActionResult> GetChat(Guid id)
    {
        var chat = await _mediator.Send(new GetChatByIdQuery(id));

        if (chat == null)
        {
            return NotFound();
        }

        var chatDto = _mapper.Map<ChatDto>(chat);
        
        return Ok(chatDto);
    }
        
    [HttpPut("update")]
    public async Task<IActionResult> UpdateChat([FromBody] UpdateChatDto updateChatDto)
    {
        var updatedChat = await _mediator.Send(new UpdateChatCommand(updateChatDto));
    
        return Ok(updatedChat);
    }
        
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteChat([FromBody] DeleteChatDto deleteChatDto)
    {
        var result = await _mediator.Send(new DeleteChatCommand(deleteChatDto));

        if (result)
        {
            return NoContent();
        }

        return NotFound();
    }
}