using System.Security.Claims;
using AutoMapper;
using MediatR;
using MessagingPlatform.Api.Controllers;
using MessagingPlatform.Application.CQRS.Users.Commands.AddUser;
using MessagingPlatform.Application.Common.Interfaces;
using MessagingPlatform.Application.Common.Models.UserDTOs;
using Moq;
using FluentAssertions;
using MessagingPlatform.Application.CQRS.Users.Commands.DeleteUser;
using MessagingPlatform.Application.CQRS.Users.Commands.SignIn;
using MessagingPlatform.Application.CQRS.Users.Commands.UpdateUser;
using MessagingPlatform.Application.CQRS.Users.Queries.GetUserById;
using MessagingPlatform.Application.CQRS.Users.Queries.GetUserByUsername;
using MessagingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Http;
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
    public async Task Get_ShouldReturnOk_WhenUserIsFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            FirstName = "Test",
            LastName = "GetUser",
            Username = "testuser",
            Email = "testuser@gmail.com",
            PasswordHash = "hashedPassword",
            AccountCreatedAt = DateTime.MinValue
        };

        _mediatrMock
            .Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), default))
            .ReturnsAsync(user);

        _mapperMock
            .Setup(m => m.Map<GetUserDto>(user))
            .Returns(new GetUserDto
            {
                FirstName = "Test",
                LastName = "GetUser",
                Username = "testuser",
                Email = "testuser@gmail.com",
                AccountCreatedAt = DateTime.MinValue
            });

        SetUserClaims(userId);

        // Act
        var result = await _controller.Get();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(new GetUserDto
        {
            FirstName = "Test",
            LastName = "GetUser",
            Username = "testuser",
            Email = "testuser@gmail.com",
            AccountCreatedAt = DateTime.MinValue
        });
    }

    [Fact]
    public async Task GetByUsername_ShouldReturnOk_WhenUserIsFound()
    {
        // Arrange
        var username = "testuser";
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "GetUser",
            Username = "testuser",
            Email = "testuser@gmail.com",
            PasswordHash = "hashedPassword",
            AccountCreatedAt = DateTime.MinValue
        };

        _mediatrMock
            .Setup(m => m.Send(It.IsAny<GetUserByUsernameQuery>(), default))
            .ReturnsAsync(user);

        _mapperMock
            .Setup(m => m.Map<GetUserDto>(user))
            .Returns(new GetUserDto
            {
                FirstName = "Test",
                LastName = "GetUser",
                Username = "testuser",
                Email = "testuser@gmail.com",
                Bio = "idk",
                IsOnline = true,
                AccountCreatedAt = DateTime.MaxValue
            });
        
        // Act
        var result = await _controller.GetByUsername(username);
        
        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(new GetUserDto
        {
            FirstName = "Test",
            LastName = "GetUser",
            Username = "testuser",
            Email = "testuser@gmail.com",
            Bio = "idk",
            IsOnline = true,
            AccountCreatedAt = DateTime.MaxValue
        });
    }
    
    [Fact]
    public async Task GetByUsername_ShouldReturnBadRequest_WhenUserNotFound()
    {
        // Arrange
        var username = "user_2039";
        User? user = null;

        _mediatrMock
            .Setup(m => m.Send(It.IsAny<GetUserByUsernameQuery>(), default))
            .ReturnsAsync(user);

        _mapperMock
            .Setup(m => m.Map<GetUserDto>(user))
            .Returns(new GetUserDto
            {
                FirstName = null,
                LastName = null,
                Username = null,
                Email = null,
                AccountCreatedAt = default
            });
        
        // Act
        var result = await _controller.GetByUsername(username);
        
        // Assert
        var okResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        okResult.StatusCode.Should().Be(404);
        okResult.Value.Should().Be("GetUser not found");
    }

    [Fact]
    public async Task SignUp_ShouldReturnOk_WhenUserIsAddedSuccessfully()
    {
        // Arrange 
        var addUserDto = new CreateUserDto
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
        okResult.Value.Should().BeEquivalentTo(new { message = "GetUser signed up successfully." });

        _cookieServiceMock.Verify(c => c.Append("token", token), Times.Once);
    }

    [Fact]
    public async Task SignUp_ShouldReturnBadRequest_WhenDtoIsNull()
    {
        // Arrange 
        CreateUserDto? nullAddUserDto = null;

        // Act
        var result = await _controller.SignUp(nullAddUserDto);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequestResult.StatusCode.Should().Be(400);
        badRequestResult.Value.Should().Be("Invalid user data.");
    }

    [Fact]
    public async Task SignIn_ShouldReturnOk_WhenUserIsSignedInSuccessfully()
    {
        // Arrange
        var signInDto = new SignInDto
        {
            Username = "qwerty",
            Password = "Qwerty"
        };

        var token = "valid-token";

        _mediatrMock
            .Setup(m => m.Send(It.IsAny<SignInCommand>(), default))
            .ReturnsAsync(token);

        // Act
        var result = await _controller.SignIn(signInDto);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(new { message = "You signed in successfully." });

        _cookieServiceMock.Verify(c => c.Append("token", token), Times.Once);
    }

    [Fact]
    public async Task SignIn_ShouldReturnBadRequest_WhenDtoIsNull()
    {
        // Arrange
        SignInDto? nullSignInDto = null;

        // Act
        var result = await _controller.SignIn(nullSignInDto);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequestResult.StatusCode.Should().Be(400);
        badRequestResult.Value.Should().Be("Invalid user data.");
    }

    [Fact]
    public async Task SignIn_ShouldReturnUnauthorized_WhenSignInFails()
    {
        // Arrange
        var wrongSignInDto = new SignInDto
        {
            Username = "wrong_user",
            Password = "wrong_password&*09@@@"
        };

        string? token = null;

        _mediatrMock
            .Setup(m => m.Send(It.IsAny<SignInCommand>(), default))
            .ReturnsAsync(token);

        // Act
        var result = await _controller.SignIn(wrongSignInDto);

        // Assert
        var unauthorizedResult = result.Should().BeOfType<UnauthorizedObjectResult>().Subject;
        unauthorizedResult.StatusCode.Should().Be(401);
        unauthorizedResult.Value.Should().Be("Invalid username or password.");
    }

    [Fact]
    public void SignOut_ShouldReturnOk_WhenUserSignsOutSuccessfully()
    {
        // Act
        var result = _controller.SignOut();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(new { message = "GetUser signed out successfully." });

        _cookieServiceMock.Verify(c => c.Delete("token"), Times.Once);
    }

    [Fact]
    public async Task Update_ShouldReturnOk_WhenUpdateIsSuccessful()
    {
        // Arrange
        var updateUserDto = new UpdateUserDto
        {
            FirstName = "Bob",
            LastName = "Smith",
            Email = "bob.smith2003@gmail.com",
            Bio = "I like it!"
        };
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = updateUserDto.FirstName,
            LastName = updateUserDto.LastName,
            Email = updateUserDto.Email,
            Bio = updateUserDto.Bio,
            Username = "bob.smith2003",
            PasswordHash = "hashedPassword",
            AccountCreatedAt = DateTime.MinValue
        };

        _mapperMock
            .Setup(m => m.Map<User>(updateUserDto))
            .Returns(user);
        
        _mediatrMock
            .Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), default))
            .ReturnsAsync(true);

        SetUserClaims(user.Id);
        
        // Act
        var result = await _controller.Update(updateUserDto);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(new { message = "GetUser data updated successfully." });
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenUserNotFound()
    {
        // Arrange
        var updateUserDto = new UpdateUserDto
        {
            FirstName = "Bob",
            LastName = "Smith",
            Email = "bob.smith2003@gmail.com",
            Bio = "I like it!"
        };
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = updateUserDto.FirstName,
            LastName = updateUserDto.LastName,
            Email = updateUserDto.Email,
            Bio = updateUserDto.Bio,
            Username = "bob.smith2003",
            PasswordHash = "hashedPassword",
            AccountCreatedAt = DateTime.MinValue
        };

        _mapperMock
            .Setup(m => m.Map<User>(updateUserDto))
            .Returns(user);
        
        _mediatrMock
            .Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), default))
            .ReturnsAsync(false);

        SetUserClaims(user.Id);
        
        // Act
        var result = await _controller.Update(updateUserDto);

        // Assert
        var okResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        okResult.StatusCode.Should().Be(404);
        okResult.Value.Should().Be("GetUser not found.");
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

    [Fact]
    public async Task Delete_ShouldReturnOk_WhenDeleteIsSuccessful()
    {
        // Arrange
        _mediatrMock
            .Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), default))
            .ReturnsAsync(true);
        
        // ToDo: maybe make it so that before testing this class the user id is initiated
        SetUserClaims(Guid.NewGuid());
        
        // Act
        var result = await _controller.Delete();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(new { message = "GetUser deleted successfully." });
        
        _cookieServiceMock.Verify(c => c.Delete("token"), Times.Once);
    }
    
    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenUserNotFound()
    {
        // Arrange
        _mediatrMock
            .Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), default))
            .ReturnsAsync(false);
        
        // ToDo: maybe make it so that before testing this class the user id is initiated
        SetUserClaims(Guid.NewGuid());
        
        // Act
        var result = await _controller.Delete();

        // Assert
        var okResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        okResult.StatusCode.Should().Be(404);
        okResult.Value.Should().BeEquivalentTo("GetUser not found.");
    }
}