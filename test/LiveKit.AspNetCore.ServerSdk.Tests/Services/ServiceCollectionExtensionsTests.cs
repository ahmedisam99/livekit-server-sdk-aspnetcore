using FluentAssertions;
using LiveKit.Authentication;
using LiveKit.Services;
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
