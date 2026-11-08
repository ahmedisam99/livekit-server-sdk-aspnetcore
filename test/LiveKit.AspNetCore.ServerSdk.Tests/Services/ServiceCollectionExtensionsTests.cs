using FluentAssertions;
using LiveKit.Authentication;
using LiveKit.Proto;
using LiveKit.Services;
using LiveKit.Tests.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace LiveKit.Tests.Services;

public class ServiceCollectionExtensionsTests
{
    private static ServiceCollection CreateServicesWithLiveKit()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddLiveKit(options =>
        {
            options.ApiKey = "test-key";
            options.ApiSecret = "test-secret-that-is-long-enough";
            options.BaseUrl = "https://livekit.example.com";
        });
        return services;
    }

    [Fact]
    public void AddLiveKit_WithNullServices_ThrowsArgumentNullException()
    {
        IServiceCollection services = null!;
        var act = () => services.AddLiveKit(_ => { });
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddLiveKit_WithNullConfigure_ThrowsArgumentNullException()
    {
        var services = new ServiceCollection();
        var act = () => services.AddLiveKit(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddLiveKit_RegistersTokenService()
    {
        var provider = CreateServicesWithLiveKit().BuildServiceProvider();
        provider.GetService<ILiveKitTokenService>().Should().NotBeNull();
    }

    [Fact]
    public void AddLiveKit_RegistersTokenVerifier()
    {
        var provider = CreateServicesWithLiveKit().BuildServiceProvider();
        provider.GetService<ILiveKitTokenVerifier>().Should().NotBeNull();
    }

    [Fact]
    public void AddLiveKit_RegistersWebhookReceiver()
    {
        var provider = CreateServicesWithLiveKit().BuildServiceProvider();
        provider.GetService<ILiveKitWebhookReceiver>().Should().NotBeNull();
    }

    [Fact]
    public void AddLiveKit_RegistersRoomService()
    {
        var provider = CreateServicesWithLiveKit().BuildServiceProvider();
        provider.GetService<ILiveKitRoomService>().Should().NotBeNull();
    }

    [Fact]
    public void AddLiveKit_RegistersEgressService()
    {
        var provider = CreateServicesWithLiveKit().BuildServiceProvider();
        provider.GetService<ILiveKitEgressService>().Should().NotBeNull();
    }

    [Fact]
    public void AddLiveKit_RegistersIngressService()
    {
        var provider = CreateServicesWithLiveKit().BuildServiceProvider();
        provider.GetService<ILiveKitIngressService>().Should().NotBeNull();
    }

    [Fact]
    public void AddLiveKit_RegistersSipService()
    {
        var provider = CreateServicesWithLiveKit().BuildServiceProvider();
        provider.GetService<ILiveKitSipService>().Should().NotBeNull();
    }

    [Fact]
    public void AddLiveKit_RegistersPhoneNumberService()
    {
        var provider = CreateServicesWithLiveKit().BuildServiceProvider();
        provider.GetService<ILiveKitPhoneNumberService>().Should().NotBeNull();
    }

    [Fact]
    public void AddLiveKit_RegistersAgentDispatchService()
    {
        var provider = CreateServicesWithLiveKit().BuildServiceProvider();
        provider.GetService<ILiveKitAgentDispatchService>().Should().NotBeNull();
    }

    [Fact]
    public void AddLiveKit_RegistersCloudAgentService()
    {
        var provider = CreateServicesWithLiveKit().BuildServiceProvider();
        provider.GetService<ILiveKitCloudAgentService>().Should().NotBeNull();
    }

    [Fact]
    public void AddLiveKit_RegistersConnectorService()
    {
        var provider = CreateServicesWithLiveKit().BuildServiceProvider();
        provider.GetService<ILiveKitConnectorService>().Should().NotBeNull();
    }

    [Fact]
    public void AddLiveKit_RegistersAgentSimulationService()
    {
        var provider = CreateServicesWithLiveKit().BuildServiceProvider();
        provider.GetService<ILiveKitAgentSimulationService>().Should().NotBeNull();
    }

    [Theory]
    [InlineData("https://my-project.livekit.cloud", "https://agents.livekit.cloud/")]
    [InlineData("wss://my-project.livekit.cloud", "https://agents.livekit.cloud/")]
    public void AddLiveKit_CloudAgentService_UsesAgentsHostDerivedFromBaseUrl(string baseUrl, string expected)
    {
        var provider = CreateServicesWithLiveKit(baseUrl).BuildServiceProvider();

        GetBaseAddress(provider, nameof(ILiveKitCloudAgentService)).Should().Be(expected);
    }

    [Theory]
    [InlineData("https://agents.example.com", "https://agents.example.com/")]
    [InlineData("wss://agents.example.com", "https://agents.example.com/")]
    public void AddLiveKit_CloudAgentService_UsesAgentsUrlWhenSet(string agentsUrl, string expected)
    {
        var provider = CreateServicesWithLiveKit("https://my-project.livekit.cloud", agentsUrl).BuildServiceProvider();

        GetBaseAddress(provider, nameof(ILiveKitCloudAgentService)).Should().Be(expected);
    }

    [Theory]
    [InlineData("https://my-project.livekit.cloud", "https://my-project.livekit.cloud/")]
    [InlineData("wss://my-project.livekit.cloud", "https://my-project.livekit.cloud/")]
    [InlineData("WSS://my-project.livekit.cloud", "https://my-project.livekit.cloud/")]
    [InlineData("ws://localhost:7880", "http://localhost:7880/")]
    [InlineData("http://localhost:7880", "http://localhost:7880/")]
    public void AddLiveKit_OtherServices_UseBaseUrlWithHttpScheme(string baseUrl, string expected)
    {
        var provider = CreateServicesWithLiveKit(baseUrl).BuildServiceProvider();

        foreach (var clientName in new[]
                 {
                     nameof(ILiveKitRoomService), nameof(ILiveKitEgressService), nameof(ILiveKitIngressService),
                     nameof(ILiveKitSipService), nameof(ILiveKitPhoneNumberService), nameof(ILiveKitAgentDispatchService),
                     nameof(ILiveKitConnectorService), nameof(ILiveKitAgentSimulationService)
                 })
        {
            GetBaseAddress(provider, clientName).Should().Be(expected, "{0} should use the base URL", clientName);
        }
    }

    [Fact]
    public async Task AddLiveKit_WithWssBaseUrl_HandlerReceivesHttpsRequest()
    {
        var handler = MockHttpMessageHandler.WithJsonResponse("{\"sid\": \"RM_123\", \"name\": \"test-room\"}");
        var services = CreateServicesWithLiveKit("wss://my-project.livekit.cloud");
        services.ConfigureHttpClientDefaults(builder => builder.ConfigurePrimaryHttpMessageHandler(() => handler));
        var roomService = services.BuildServiceProvider().GetRequiredService<ILiveKitRoomService>();

        await roomService.CreateRoomAsync(new CreateRoomRequest { Name = "test-room" });

        handler.LastRequest!.RequestUri.Should().Be(new Uri("https://my-project.livekit.cloud/twirp/livekit.RoomService/CreateRoom"));
    }

    private static ServiceCollection CreateServicesWithLiveKit(string baseUrl, string? agentsUrl = null)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddLiveKit(options =>
        {
            options.ApiKey = "test-key";
            options.ApiSecret = "test-api-secret-that-is-long-enough";
            options.BaseUrl = baseUrl;
            options.AgentsUrl = agentsUrl;
        });
        return services;
    }

    private static string? GetBaseAddress(IServiceProvider provider, string clientName) =>
        provider.GetRequiredService<IHttpClientFactory>().CreateClient(clientName).BaseAddress?.ToString();

    [Fact]
    public void AddLiveKit_ConfiguresOptions()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddLiveKit(options =>
        {
            options.ApiKey = "my-key";
            options.ApiSecret = "my-secret";
            options.BaseUrl = "https://my-livekit.com";
        });

        var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<Microsoft.Extensions.Options.IOptions<LiveKitOptions>>();

        options.Value.ApiKey.Should().Be("my-key");
        options.Value.ApiSecret.Should().Be("my-secret");
        options.Value.BaseUrl.Should().Be("https://my-livekit.com");
    }

    [Fact]
    public void AddLiveKit_ReturnsServiceCollectionForChaining()
    {
        var services = new ServiceCollection();
        var result = services.AddLiveKit(options =>
        {
            options.ApiKey = "key";
            options.ApiSecret = "secret";
            options.BaseUrl = "https://example.com";
        });

        result.Should().BeSameAs(services);
    }
}
