using FluentAssertions;
using LiveKit.Authentication;
using LiveKit.Services;
using LiveKit.Tests.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using LiveKit.Proto;

namespace LiveKit.Tests.Services;

public class LiveKitAgentDBServiceTests
{
    private static LiveKitAgentDBService CreateService(MockHttpMessageHandler handler)
    {
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://livekit.example.com") };
        var tokenService = new LiveKitTokenService(Options.Create(new LiveKitOptions
        {
            ApiKey = "test-api-key",
            ApiSecret = "test-api-secret-that-is-long-enough"
        }));
        return new LiveKitAgentDBService(httpClient, NullLogger<LiveKitAgentDBService>.Instance, tokenService);
    }

    [Fact]
    public async Task CreateDatabaseAsync_CallsRouteAndParsesResponse()
    {
        var handler = MockHttpMessageHandler.WithJsonResponse("{\"databaseId\": \"DB_123\", \"expiresAt\": \"1700000000\"}");
        var service = CreateService(handler);

        var response = await service.CreateDatabaseAsync(new AgentDB.Types.CreateRequest { Region = "us-east" });

        handler.LastRequest!.RequestUri!.PathAndQuery.Should().Be("/twirp/livekit.AgentDBService/CreateDatabase");
        response.DatabaseId.Should().Be("DB_123");
        response.ExpiresAt.Should().Be(1700000000);
    }

    [Fact]
    public async Task GetDatabaseAsync_CallsRouteAndParsesResponse()
    {
        var handler = MockHttpMessageHandler.WithJsonResponse("{\"databaseId\": \"DB_123\", \"region\": \"us-east\", \"tip\": \"42\"}");
        var service = CreateService(handler);

        var response = await service.GetDatabaseAsync(new AgentDB.Types.GetRequest { DatabaseId = "DB_123" });

        handler.LastRequest!.RequestUri!.PathAndQuery.Should().Be("/twirp/livekit.AgentDBService/GetDatabase");
        response.Region.Should().Be("us-east");
        response.Tip.Should().Be(42);
    }

    [Fact]
    public async Task ListDatabasesAsync_CallsRouteAndParsesResponse()
    {
        var handler = MockHttpMessageHandler.WithJsonResponse("{\"databases\": [{\"databaseId\": \"DB_123\"}], \"nextPageToken\": \"next\"}");
        var service = CreateService(handler);

        var response = await service.ListDatabasesAsync(new AgentDB.Types.ListRequest { PageSize = 10 });

        handler.LastRequest!.RequestUri!.PathAndQuery.Should().Be("/twirp/livekit.AgentDBService/ListDatabases");
        response.Databases.Should().ContainSingle(d => d.DatabaseId == "DB_123");
        response.NextPageToken.Should().Be("next");
    }

    [Fact]
    public async Task DeleteDatabaseAsync_CallsRoute()
    {
        var handler = MockHttpMessageHandler.WithEmptyResponse();
        var service = CreateService(handler);

        await service.DeleteDatabaseAsync(new AgentDB.Types.DeleteRequest { DatabaseId = "DB_123" });

        handler.LastRequest!.RequestUri!.PathAndQuery.Should().Be("/twirp/livekit.AgentDBService/DeleteDatabase");
    }

    [Fact]
    public async Task DumpDatabaseAsync_CallsRouteAndParsesResponse()
    {
        var handler = MockHttpMessageHandler.WithJsonResponse("{\"downloadUrl\": \"https://storage.example.com/dump\", \"tip\": \"42\"}");
        var service = CreateService(handler);

        var response = await service.DumpDatabaseAsync(new AgentDB.Types.DumpRequest { DatabaseId = "DB_123" });

        handler.LastRequest!.RequestUri!.PathAndQuery.Should().Be("/twirp/livekit.AgentDBService/DumpDatabase");
        response.DownloadUrl.Should().Be("https://storage.example.com/dump");
    }
}
