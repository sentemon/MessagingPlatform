using System.Security.Claims;
using AutoMapper;
using MediatR;
using MessagingPlatform.Api.Controllers;
using MessagingPlatform.Application.CQRS.Chats.Commands.CreateChat;
using MessagingPlatform.Application.CQRS.Chats.Commands.DeleteChat;
using MessagingPlatform.Application.CQRS.Chats.Commands.UpdateChat;
using MessagingPlatform.Application.CQRS.Chats.Queries.GetChatById;
using MessagingPlatform.Application.CQRS.Chats.Queries.GetChats;
using MessagingPlatform.Application.Common.Models.ChatDTOs;
using MessagingPlatform.Domain.Entities;
using Moq;
using FluentAssertions;
using MessagingPlatform.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessagingPlatform.Api.Tests.Controllers;

// ToDo: fix
public class ChatControllerTests
{
    private readonly Mock<IMediator> _mediatrMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ChatController _controller;

    public ChatControllerTests()
    {
        _mediatrMock = new Mock<IMediator>();
        _mapperMock = new Mock<IMapper>();
        _controller = new ChatController(_mediatrMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetChats_ShouldReturnOk_WhenChatsAreFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var chats = new List<ChatSidebarDto>
        {
            new ChatSidebarDto
            {
                ChatId = default,
                Title = null,
                LastMessageFrom = null,
                LastMessageContent = null,
                LastMessageSentAt = null,
                UnreadMessagesCount = 0
            },
            new ChatSidebarDto
            {
                ChatId = default,
                Title = null,
                LastMessageFrom = null,
                LastMessageContent = null,
                LastMessageSentAt = null,
                UnreadMessagesCount = 0
            }
        };
            
        _mediatrMock
            .Setup(m => m.Send(It.IsAny<GetChatsQuery>(), default))
            .ReturnsAsync(chats);

        _mapperMock
            .Setup(m => m.Map<ChatSidebarDto>(chats))
            .Returns(new ChatSidebarDto
            {
                ChatId = Guid.NewGuid(),
                Title = "I dont know",
                LastMessageFrom = "null",
                LastMessageContent = "null",
                LastMessageSentAt = DateTime.MinValue,
                UnreadMessagesCount = 0
            });

        SetUserClaims(userId);

        // Act
        var result = await _controller.GetChats();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(chats);
    }

    [Fact]
    public async Task GetChat_ShouldReturnOk_WhenChatIsFound()
    {
        // Arrange
        var chatId = Guid.NewGuid();
        var chat = new Chat
        {
            Id = chatId,
            Title = "Test Chat",
            ChatType = ChatType.Private,
            CreatorId = default,
            Creator = null
        };

        _mediatrMock
            .Setup(m => m.Send(It.IsAny<GetChatByIdQuery>(), default))
            .ReturnsAsync(chat);

        var chatDto = new ChatDto
        {
            Id = chatId,
            Title = "Test Chat",
            Creator = null
        };
        _mapperMock
            .Setup(m => m.Map<ChatDto>(chat))
            .Returns(chatDto);

        // Act
        var result = await _controller.GetChat(chatId);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(chatDto);
    }

    [Fact]
    public async Task GetChat_ShouldReturnNotFound_WhenChatNotFound()
    {
        // Arrange
        var chatId = Guid.NewGuid();
        Chat? chat = null;

        _mediatrMock
            .Setup(m => m.Send(It.IsAny<GetChatByIdQuery>(), default))
            .ReturnsAsync(chat);

        // Act
        var result = await _controller.GetChat(chatId);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundResult>().Subject;
        notFoundResult.StatusCode.Should().Be(404);
    }

    /*
    [Fact]
    public async Task CreateChat_ShouldReturnCreated_WhenChatIsCreatedSuccessfully()
    {
        // Arrange
        var createChatDto = new CreateChatDto
        {
            Title = "New Chat",
            Users = new List<string>
            {
                "user1",
                "user2"
            },
            ChatType = ChatType.Private
        };
        var chat = new Chat
        {
            Id = Guid.NewGuid(),
            Title = "New Chat",
            ChatType = ChatType.Private,
            CreatorId = default,
            Creator = null
        };

        _mapperMock
            .Setup(m => m.Map<Chat>(createChatDto))
            .Returns(chat);

        var chatResponse = new Chat
        {
            Id = chat.Id,
            Title = "New Chat",
            ChatType = ChatType.Private,
            CreatorId = default,
            Creator = null
        };

        _mediatrMock
            .Setup(m => m.Send(It.IsAny<CreateChatCommand>(), default))
            .ReturnsAsync(chatResponse);

        SetUserClaims(Guid.NewGuid());

        // Act
        var result = await _controller.CreateChat(createChatDto);

        // Assert
        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.StatusCode.Should().Be(201);
        // createdResult.RouteValues?["id"].Should().Be(chatResponse.Id);
        // createdResult.Value.Should().Be("You created chat successfully!");
    }
    */

    // [Fact] // ToDo
    // public async Task UpdateChat_ShouldReturnOk_WhenUpdateIsSuccessful()
    // {
    //     // Arrange
    //     var updateChatDto = new UpdateChatDto { ChatId = Guid.NewGuid(), Title = "Updated Chat" };
    //     var updatedChat = new Chat
    //     {
    //         Id = updateChatDto.ChatId,
    //         Title = "Updated Chat",
    //         ChatType = ChatType.Private,
    //         CreatorId = default,
    //         Creator = null
    //     };
    //
    //     _mediatrMock
    //         .Setup(m => m.Send(It.IsAny<UpdateChatCommand>(), default))
    //         .ReturnsAsync(updatedChat);
    //
    //     // Act
    //     var result = await _controller.UpdateChat(updateChatDto);
    //
    //     // Assert
    //     var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    //     okResult.StatusCode.Should().Be(200);
    //     okResult.Value.Should().BeEquivalentTo(updatedChat);
    // }

    [Fact]
    public async Task DeleteChat_ShouldReturnNoContent_WhenDeleteIsSuccessful()
    {
        // Arrange
        var deleteChatDto = new DeleteChatDto { ChatId = Guid.NewGuid() };

        _mediatrMock
            .Setup(m => m.Send(It.IsAny<DeleteChatCommand>(), default))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteChat(deleteChatDto);

        // Assert
        var noContentResult = result.Should().BeOfType<NoContentResult>().Subject;
        noContentResult.StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task DeleteChat_ShouldReturnNotFound_WhenChatNotFound()
    {
        // Arrange
        var deleteChatDto = new DeleteChatDto { ChatId = Guid.NewGuid() };

        _mediatrMock
            .Setup(m => m.Send(It.IsAny<DeleteChatCommand>(), default))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteChat(deleteChatDto);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundResult>().Subject;
        notFoundResult.StatusCode.Should().Be(404);
    }

    private void SetUserClaims(Guid userId)
    {
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Sid, userId.ToString())
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };
    }
}