using MessagingPlatform.Application.CQRS.Chats.Commands.CreateChat;
using MessagingPlatform.Application.CQRS.Messages.Commands.AddMessage;
using MessagingPlatform.Application.CQRS.Messages.Commands.DeleteMessage;
using MessagingPlatform.Application.CQRS.Messages.Commands.UpdateMessage;
using MessagingPlatform.Application.CQRS.Messages.Queries.GetAllMessages;
using MessagingPlatform.Application.Common.Models.MessageDTOs;
using MessagingPlatform.Domain.Entities;
using MessagingPlatform.Domain.Enums;
using MessagingPlatform.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MessagingPlatform.Application.Tests;

[Collection(nameof(AppCollection))]
public class MessageIntegrationTests : IntegrationTestBase
{
    public MessageIntegrationTests(PostgreSqlTestContainerFixture fixture) : base(fixture)
    {
    }

    private async Task<(Guid chatId, Guid senderId)> SeedChatWithUsers(IServiceProvider sp)
    {
        var userRepo = sp.GetRequiredService<IUserRepository>();
        var createChatHandler = sp.GetRequiredService<CreateChatCommandHandler>();

        var owner = User.Create("Msg", "Owner", "msg.owner", "msg.owner@example.com", "hash", DateTime.UtcNow);
        var member = User.Create("Msg", "Member", "msg.member", "msg.member@example.com", "hash", DateTime.UtcNow);
        await userRepo.AddAsync(owner);
        await userRepo.AddAsync(member);

        var chatResult = await createChatHandler.Handle(new CreateChatCommand(ChatType.Group, new List<string> { member.Username }, owner.Id));
        return (chatResult.Response.Id, owner.Id);
    }

    [Fact]
    public async Task AddMessage_ShouldPersistAndReturnMessage()
    {
        using var scope = CreateScope();
        var (chatId, senderId) = await SeedChatWithUsers(scope.ServiceProvider);
        var addHandler = scope.ServiceProvider.GetRequiredService<AddMessageCommandHandler>();
        var query = scope.ServiceProvider.GetRequiredService<GetAllMessagesQueryHandler>();

        var addResult = await addHandler.Handle(new AddMessageCommand(new CreateMessageDto { ChatId = chatId, Content = "hello" }, senderId));
        Assert.True(addResult.IsSuccess);

        var messages = (await query.Handle(new GetAllMessagesQuery(chatId))).Response;
        Assert.Contains(messages, m => m.Content == "hello" && m.SenderId == senderId);
    }

    [Fact]
    public async Task UpdateMessage_ShouldChangeContent()
    {
        using var scope = CreateScope();
        var (chatId, senderId) = await SeedChatWithUsers(scope.ServiceProvider);
        var addHandler = scope.ServiceProvider.GetRequiredService<AddMessageCommandHandler>();
        var updateHandler = scope.ServiceProvider.GetRequiredService<UpdateMessageCommandHandler>();

        var addResult = await addHandler.Handle(new AddMessageCommand(new CreateMessageDto { ChatId = chatId, Content = "old" }, senderId));
        var messageId = addResult.Response.Id;

        var updateResult = await updateHandler.Handle(new UpdateMessageCommand(new UpdateMessageDto { MessageId = messageId, Content = "new" }, senderId));
        Assert.True(updateResult.IsSuccess);
        Assert.Equal("new", updateResult.Response.Content);
    }

    [Fact]
    public async Task DeleteMessage_ShouldRemove()
    {
        using var scope = CreateScope();
        var (chatId, senderId) = await SeedChatWithUsers(scope.ServiceProvider);
        var addHandler = scope.ServiceProvider.GetRequiredService<AddMessageCommandHandler>();
        var deleteHandler = scope.ServiceProvider.GetRequiredService<DeleteMessageCommandHandler>();
        var query = scope.ServiceProvider.GetRequiredService<GetAllMessagesQueryHandler>();

        var addResult = await addHandler.Handle(new AddMessageCommand(new CreateMessageDto { ChatId = chatId, Content = "to-delete" }, senderId));
        var messageId = addResult.Response.Id;

        var deleteResult = await deleteHandler.Handle(new DeleteMessageCommand(new DeleteMessageDto { MessageId = messageId, SenderId = senderId }));
        Assert.True(deleteResult.IsSuccess);

        var messages = (await query.Handle(new GetAllMessagesQuery(chatId))).Response.ToList();
        Assert.DoesNotContain(messages, m => m.Id == messageId);
    }
}
