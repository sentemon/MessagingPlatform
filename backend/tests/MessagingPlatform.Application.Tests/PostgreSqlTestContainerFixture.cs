using DotNet.Testcontainers.Builders;
using Testcontainers.PostgreSql;
using Xunit;

namespace MessagingPlatform.Application.Tests;

public class PostgreSqlTestContainerFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;

    public PostgreSqlTestContainerFixture()
    {
        _container = new PostgreSqlBuilder()
            .WithDatabase("messaging_app_tests")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithImage("postgres:16-alpine")
            .WithCleanUp(true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
            .Build();
    }

    public string ConnectionString => _container.GetConnectionString();

    public Task InitializeAsync() => _container.StartAsync();

    public Task DisposeAsync() => _container.DisposeAsync().AsTask();
}
