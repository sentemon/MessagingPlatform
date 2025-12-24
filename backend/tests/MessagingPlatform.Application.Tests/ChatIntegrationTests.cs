using MessagingPlatform.Application.CQRS.Chats.Commands.CreateChat;
using MessagingPlatform.Application.CQRS.Chats.Commands.UpdateChat;
using MessagingPlatform.Application.CQRS.Chats.Queries.GetChatById;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Enums;
using MessagingPlatform.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MessagingPlatform.Application.Tests;

[CollectionDefinition(nameof(AppCollection))]
public class AppCollection : ICollectionFixture<PostgreSqlTestContainerFixture>;

[Collection(nameof(AppCollection))]
public class ChatIntegrationTests : IntegrationTestBase
{
    public ChatIntegrationTests(PostgreSqlTestContainerFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task CreateChat_Group_ShouldPersistWithMembers()
    {
        using var scope = CreateScope();
        var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var handler = scope.ServiceProvider.GetRequiredService<CreateChatCommandHandler>();
        var chatQuery = scope.ServiceProvider.GetRequiredService<GetChatByIdQueryHandler>();

        var owner = User.Create("Owner", "One", "owner", "owner@example.com", "hash", DateTime.UtcNow);
        var member = User.Create("Member", "Two", "member", "member@example.com", "hash", DateTime.UtcNow);
        await userRepo.AddAsync(owner);
        await userRepo.AddAsync(member);

        var command = new CreateChatCommand(ChatType.Group, new List<string> { member.Username }, owner.Id);
        var result = await handler.Handle(command);

        Assert.True(result.IsSuccess);

        var chatId = result.Response.Id;
        var chatResult = await chatQuery.Handle(new GetChatByIdQuery(chatId, owner.Id));
        var chat = chatResult.Response;

        Assert.NotNull(chat);
        Assert.Equal(ChatType.Group, chat.ChatType);
        Assert.Equal(2, chat.UserChats.Count);
    }

    [Fact]
    public async Task UpdateChat_Title_ShouldChange()
    {
        using var scope = CreateScope();
        var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var createHandler = scope.ServiceProvider.GetRequiredService<CreateChatCommandHandler>();
        var updateHandler = scope.ServiceProvider.GetRequiredService<UpdateChatCommandHandler>();
        var chatQuery = scope.ServiceProvider.GetRequiredService<GetChatByIdQueryHandler>();

        var owner = User.Create("Owner", "One", "owner2", "owner2@example.com", "hash", DateTime.UtcNow);
        await userRepo.AddAsync(owner);

        var createResult = await createHandler.Handle(new CreateChatCommand(ChatType.Group, new List<string>(), owner.Id));
        var chatId = createResult.Response.Id;

        var updateResult = await updateHandler.Handle(new UpdateChatCommand(owner.Id, chatId, "New Title"));
        Assert.True(updateResult.IsSuccess);

        var chat = (await chatQuery.Handle(new GetChatByIdQuery(chatId, owner.Id))).Response;
        Assert.Equal("New Title", chat?.Title);
    }
}
