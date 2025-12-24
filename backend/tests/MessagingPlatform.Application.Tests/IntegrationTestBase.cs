using MessagingPlatform.Application;
using MessagingPlatform.Infrastructure;
using MessagingPlatform.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace MessagingPlatform.Application.Tests;

public abstract class IntegrationTestBase : IAsyncLifetime
{
    private readonly PostgreSqlTestContainerFixture _fixture;
    protected IServiceProvider ServiceProvider { get; private set; } = null!;

    protected IntegrationTestBase(PostgreSqlTestContainerFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = _fixture.ConnectionString,
                ["JwtOptions:SecretKey"] = "integration-test-secret-key-0123456789",
                ["JwtOptions:ExpiresHours"] = "1"
            })
            .Build();

        var services = new ServiceCollection();
        services.AddLogging(builder => builder.AddDebug());
        services.AddInfrastructure(configuration);
        services.AddApplication();

        ServiceProvider = services.BuildServiceProvider();

        using var scope = ServiceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.EnsureDeletedAsync();
        await db.Database.EnsureCreatedAsync();
    }

    public Task DisposeAsync()
    {
        if (ServiceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }

        return Task.CompletedTask;
    }

    protected IServiceScope CreateScope() => ServiceProvider.CreateScope();
}
