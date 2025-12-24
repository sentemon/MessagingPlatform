using MessagingPlatform.Application.Abstractions;

namespace MessagingPlatform.Application.CQRS.Users.Commands.SignIn;

public record SignInCommand(string Username, string Password) : ICommand;