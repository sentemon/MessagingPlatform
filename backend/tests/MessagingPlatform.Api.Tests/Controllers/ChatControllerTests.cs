using AutoMapper;
using MediatR;
using MessagingPlatform.Api.Controllers;
using MessagingPlatform.Application.Common.Interfaces;
using Moq;

namespace MessagingPlatform.Api.Tests.Controllers;

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
       
        // Act
        
        // Assert
    }

    [Fact]
    public async Task Get_ShouldReturnOk_WhenChatIsFound()
    {
        // Arrange
        
        // Act
        
        // Assert
    }
}