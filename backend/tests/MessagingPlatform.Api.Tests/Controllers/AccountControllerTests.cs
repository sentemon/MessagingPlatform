using AutoMapper;
using MediatR;
using MessagingPlatform.Api.Controllers;
using MessagingPlatform.Application.CQRS.Users.Commands.AddUser;
using MessagingPlatform.Application.Common.Interfaces;
using MessagingPlatform.Application.Common.Models.UserDTOs;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace MessagingPlatform.Api.Tests.Controllers;

public class AccountControllerTests
{
    private readonly Mock<IMediator> _mediatrMock;
    private readonly Mock<ICookieService> _cookieServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AccountController _controller;

    public AccountControllerTests()
    {
        _mediatrMock = new Mock<IMediator>();
        _cookieServiceMock = new Mock<ICookieService>();
        _mapperMock = new Mock<IMapper>();
        
        _controller = new AccountController(_mediatrMock.Object, _cookieServiceMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task SignUp_ShouldReturnOk_WhenUserIsAddedSuccessfully()
    {
        // Arrange 
        var addUserDto = new AddUserDto
        {
            FirstName = "Ivan",
            LastName = "Sentemon",
            Username = "test",
            Email = "ivan.test@gmail.com",
            Password = "HelloItsFirstTest###123",
            ConfirmPassword = "HelloItsFirstTest###123"
        };
        
        var token = "valid-token";

        _mediatrMock
            .Setup(m => m.Send(It.IsAny<AddUserCommand>(), default))
            .ReturnsAsync(token);
        
        // Act 
        var result = await _controller.SignUp(addUserDto);
        
        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(new { message = "User signed up successfully." });
        
        _cookieServiceMock.Verify(c => c.Append("token", token), Times.Once);
    }

    [Fact]
    public async Task SignUp_ShouldReturnBadRequest_WhenDtoIsNull()
    {
        // Arrange 
        AddUserDto? nullAddUserDto = null;
        
        // Act
        var result = await _controller.SignUp(nullAddUserDto);
        
        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequestResult.StatusCode.Should().Be(400);
        badRequestResult.Value.Should().Be("Invalid user data.");
    }
}