using System.Text.Json;
using FluentAssertions;
using LiveKit.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;

namespace LiveKit.Tests.Authentication;

public class LiveKitTokenServiceTests
{
    private const string ApiKey = "test-api-key";
    private const string ApiSecret = "test-api-secret-that-is-long-enough";

    private static IOptions<LiveKitOptions> CreateOptions(string? apiKey = ApiKey, string? apiSecret = ApiSecret) =>
        Options.Create(new LiveKitOptions
        {
            ApiKey = apiKey ?? string.Empty,
            ApiSecret = apiSecret ?? string.Empty
        });

    private static LiveKitTokenService CreateService(string? apiKey = ApiKey, string? apiSecret = ApiSecret) =>
        new(CreateOptions(apiKey, apiSecret));

    private static JsonWebToken ReadToken(string jwt) => new JsonWebTokenHandler().ReadJsonWebToken(jwt);

    private static string? GetClaim(JsonWebToken token, string type) =>
        token.Claims.FirstOrDefault(c => c.Type == type)?.Value;

    [Fact]
    public void Constructor_WithMissingApiKey_ThrowsInvalidOperationException()
    {
        var act = () => CreateService(apiKey: "");
        act.Should().Throw<InvalidOperationException>().Which.Message.Should().Contain("API key");
    }

    [Fact]
    public void Constructor_WithMissingApiSecret_ThrowsInvalidOperationException()
    {
        var act = () => CreateService(apiSecret: "");
        act.Should().Throw<InvalidOperationException>().Which.Message.Should().Contain("API secret");
    }

    [Fact]
    public void CreateTokenBuilder_WithValidIdentity_ReturnsBuilder()
    {
        var service = CreateService();
        var builder = service.CreateTokenBuilder("user1");

        builder.Should().NotBeNull();
        builder.Should().BeAssignableTo<ILiveKitTokenBuilder>();
    }

    [Fact]
    public void CreateTokenBuilder_WithValidIdentity_BuilderProducesValidJwt()
    {
        var service = CreateService();
        var jwt = service.CreateTokenBuilder("user1").ToJwt();

        jwt.Split('.').Should().HaveCount(3);
        var token = ReadToken(jwt);
        token.Subject.Should().Be("user1");
    }

    [Fact]
    public void CreateTokenBuilder_WithNullIdentity_ThrowsArgumentException()
    {
        var service = CreateService();
        var act = () => service.CreateTokenBuilder(null!);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CreateServerToken_SetsIdentityToServer()
    {
        var service = CreateService();
        var jwt = service.CreateServerToken();
        var token = ReadToken(jwt);

        token.Subject.Should().Be("server");
    }

    [Fact]
    public void CreateServerToken_SetsNameToServer()
    {
        var service = CreateService();
        var jwt = service.CreateServerToken();
        var token = ReadToken(jwt);

        GetClaim(token, "name").Should().Be("server");
    }

    [Fact]
    public void CreateServerToken_IncludesVideoGrantWithAllPermissions()
    {
        var service = CreateService();
        var jwt = service.CreateServerToken();
        var token = ReadToken(jwt);

        var videoJson = GetClaim(token, "video");
        videoJson.Should().NotBeNull();

        var doc = JsonDocument.Parse(videoJson);
        var root = doc.RootElement;
        root.GetProperty("roomJoin").GetBoolean().Should().BeTrue();
        root.GetProperty("roomCreate").GetBoolean().Should().BeTrue();
        root.GetProperty("roomList").GetBoolean().Should().BeTrue();
        root.GetProperty("roomAdmin").GetBoolean().Should().BeTrue();
        root.GetProperty("roomRecord").GetBoolean().Should().BeTrue();
        root.GetProperty("ingressAdmin").GetBoolean().Should().BeTrue();
        root.GetProperty("canPublish").GetBoolean().Should().BeTrue();
        root.GetProperty("canSubscribe").GetBoolean().Should().BeTrue();
        root.GetProperty("canPublishData").GetBoolean().Should().BeTrue();
        root.GetProperty("canUpdateOwnMetadata").GetBoolean().Should().BeTrue();
    }

    [Fact]
    public void CreateServerToken_IncludesSipGrantWithAllPermissions()
    {
        var service = CreateService();
        var jwt = service.CreateServerToken();
        var token = ReadToken(jwt);

        var sipJson = GetClaim(token, "sip");
        sipJson.Should().NotBeNull();

        var doc = JsonDocument.Parse(sipJson);
        doc.RootElement.GetProperty("admin").GetBoolean().Should().BeTrue();
        doc.RootElement.GetProperty("call").GetBoolean().Should().BeTrue();
    }

    [Fact]
    public void CreateServerToken_WithRoomName_IncludesRoomInVideoGrant()
    {
        var service = CreateService();
        var jwt = service.CreateServerToken(roomName: "my-room");
        var token = ReadToken(jwt);

        var videoJson = GetClaim(token, "video");
        var doc = JsonDocument.Parse(videoJson!);
        doc.RootElement.GetProperty("room").GetString().Should().Be("my-room");
    }

    [Fact]
    public void CreateServerToken_DefaultTtlIs5Minutes()
    {
        var service = CreateService();
        var jwt = service.CreateServerToken();
        var token = ReadToken(jwt);

        var duration = token.ValidTo - token.ValidFrom;
        duration.Should().BeCloseTo(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void CreateServerToken_WithCustomTtl_SetsCorrectExpiration()
    {
        var service = CreateService();
        var jwt = service.CreateServerToken(ttl: TimeSpan.FromMinutes(15));
        var token = ReadToken(jwt);

        var duration = token.ValidTo - token.ValidFrom;
        duration.Should().BeCloseTo(TimeSpan.FromMinutes(15), TimeSpan.FromSeconds(5));
    }
}
