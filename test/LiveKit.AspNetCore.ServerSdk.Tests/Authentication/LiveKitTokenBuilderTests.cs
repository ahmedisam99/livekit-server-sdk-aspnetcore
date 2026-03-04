using System.Text.Json;
using FluentAssertions;
using LiveKit.Authentication;
using LiveKit.Proto;
using Microsoft.IdentityModel.JsonWebTokens;

namespace LiveKit.Tests.Authentication;

public class LiveKitTokenBuilderTests
{
    private const string ApiKey = "test-api-key";
    private const string ApiSecret = "test-api-secret-that-is-long-enough";
    private const string Identity = "test-user";

    private static LiveKitTokenBuilder CreateBuilder() => new(ApiKey, ApiSecret, Identity);

    private static JsonWebToken ReadToken(string jwt) => new JsonWebTokenHandler().ReadJsonWebToken(jwt);

    private static string? GetClaim(JsonWebToken token, string type) =>
        token.Claims.FirstOrDefault(c => c.Type == type)?.Value;

    [Fact]
    public void ToJwt_WithIdentityOnly_ReturnsValidJwtString()
    {
        var jwt = CreateBuilder().ToJwt();

        jwt.Split('.').Should().HaveCount(3);
    }

    [Fact]
    public void ToJwt_WithIdentityOnly_SetsIssuerToApiKey()
    {
        var jwt = CreateBuilder().ToJwt();
        var token = ReadToken(jwt);

        token.Issuer.Should().Be(ApiKey);
    }

    [Fact]
    public void ToJwt_WithIdentityOnly_SetsSubClaimToIdentity()
    {
        var jwt = CreateBuilder().ToJwt();
        var token = ReadToken(jwt);

        token.Subject.Should().Be(Identity);
    }

    [Fact]
    public void ToJwt_WithIdentityOnly_SetsExpirationInFuture()
    {
        var jwt = CreateBuilder().ToJwt();
        var token = ReadToken(jwt);

        token.ValidTo.Should().BeAfter(DateTime.UtcNow);
    }

    [Fact]
    public void ToJwt_WithIdentityOnly_DefaultTtlIs60Minutes()
    {
        var jwt = CreateBuilder().ToJwt();
        var token = ReadToken(jwt);

        var duration = token.ValidTo - token.ValidFrom;
        duration.Should().BeCloseTo(TimeSpan.FromMinutes(60), TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void ToJwt_WithIdentityOnly_UsesHmacSha256Algorithm()
    {
        var jwt = CreateBuilder().ToJwt();
        var token = ReadToken(jwt);

        token.Alg.Should().Be("HS256");
    }

    [Fact]
    public void ToJwt_WithIdentityOnly_DoesNotIncludeOptionalClaims()
    {
        var jwt = CreateBuilder().ToJwt();
        var token = ReadToken(jwt);

        GetClaim(token, "name").Should().BeNull();
        GetClaim(token, "metadata").Should().BeNull();
        GetClaim(token, "kind").Should().BeNull();
        GetClaim(token, "roomPreset").Should().BeNull();
        GetClaim(token, "attributes").Should().BeNull();
        GetClaim(token, "video").Should().BeNull();
        GetClaim(token, "sip").Should().BeNull();
        GetClaim(token, "roomConfig").Should().BeNull();
    }

    [Fact]
    public void ToJwt_WithParticipantName_IncludesNameClaim()
    {
        var jwt = CreateBuilder().WithParticipantName("Test User").ToJwt();
        var token = ReadToken(jwt);

        GetClaim(token, "name").Should().Be("Test User");
    }

    [Fact]
    public void ToJwt_WithMetadata_IncludesMetadataClaim()
    {
        var jwt = CreateBuilder().WithMetadata("some-metadata").ToJwt();
        var token = ReadToken(jwt);

        GetClaim(token, "metadata").Should().Be("some-metadata");
    }

    [Fact]
    public void ToJwt_WithKind_IncludesKindClaim()
    {
        var jwt = CreateBuilder().WithKind("agent").ToJwt();
        var token = ReadToken(jwt);

        GetClaim(token, "kind").Should().Be("agent");
    }

    [Fact]
    public void ToJwt_WithRoomPreset_IncludesRoomPresetClaim()
    {
        var jwt = CreateBuilder().WithRoomPreset("my-preset").ToJwt();
        var token = ReadToken(jwt);

        GetClaim(token, "roomPreset").Should().Be("my-preset");
    }

    [Fact]
    public void ToJwt_WithCustomTtl_SetsCorrectExpiration()
    {
        var jwt = CreateBuilder().WithTimeToLive(TimeSpan.FromMinutes(30)).ToJwt();
        var token = ReadToken(jwt);

        var duration = token.ValidTo - token.ValidFrom;
        duration.Should().BeCloseTo(TimeSpan.FromMinutes(30), TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void ToJwt_WithSingleAttribute_SerializesAsJsonObject()
    {
        var jwt = CreateBuilder().WithAttribute("role", "admin").ToJwt();
        var token = ReadToken(jwt);

        var attributesJson = GetClaim(token, "attributes");
        attributesJson.Should().NotBeNull();

        var doc = JsonDocument.Parse(attributesJson);
        doc.RootElement.ValueKind.Should().Be(JsonValueKind.Object);
        doc.RootElement.GetProperty("role").GetString().Should().Be("admin");
    }

    [Fact]
    public void ToJwt_WithMultipleAttributes_SerializesAllAttributes()
    {
        var jwt = CreateBuilder()
            .WithAttribute("role", "admin")
            .WithAttribute("team", "engineering")
            .ToJwt();
        var token = ReadToken(jwt);

        var attributesJson = GetClaim(token, "attributes");
        var doc = JsonDocument.Parse(attributesJson!);
        doc.RootElement.GetProperty("role").GetString().Should().Be("admin");
        doc.RootElement.GetProperty("team").GetString().Should().Be("engineering");
    }

    [Fact]
    public void ToJwt_WithAttributeIf_True_IncludesAttribute()
    {
        var jwt = CreateBuilder().WithAttributeIf(true, "role", "admin").ToJwt();
        var token = ReadToken(jwt);

        var attributesJson = GetClaim(token, "attributes");
        attributesJson.Should().NotBeNull();
        var doc = JsonDocument.Parse(attributesJson);
        doc.RootElement.GetProperty("role").GetString().Should().Be("admin");
    }

    [Fact]
    public void ToJwt_WithAttributeIf_False_ExcludesAttribute()
    {
        var jwt = CreateBuilder().WithAttributeIf(false, "role", "admin").ToJwt();
        var token = ReadToken(jwt);

        GetClaim(token, "attributes").Should().BeNull();
    }

    [Fact]
    public void ToJwt_WithVideoGrant_ActionOverload_SerializesToJson()
    {
        var jwt = CreateBuilder()
            .WithVideoGrant(g =>
            {
                g.RoomJoin = true;
                g.Room = "my-room";
            })
            .ToJwt();
        var token = ReadToken(jwt);

        var videoJson = GetClaim(token, "video");
        videoJson.Should().NotBeNull();

        var doc = JsonDocument.Parse(videoJson);
        doc.RootElement.GetProperty("roomJoin").GetBoolean().Should().BeTrue();
        doc.RootElement.GetProperty("room").GetString().Should().Be("my-room");
    }

    [Fact]
    public void ToJwt_WithVideoGrant_NullPropertiesOmitted()
    {
        var jwt = CreateBuilder()
            .WithVideoGrant(g => { g.RoomJoin = true; })
            .ToJwt();
        var token = ReadToken(jwt);

        var videoJson = GetClaim(token, "video");
        var doc = JsonDocument.Parse(videoJson!);

        doc.RootElement.TryGetProperty("roomCreate", out _).Should().BeFalse();
        doc.RootElement.TryGetProperty("roomList", out _).Should().BeFalse();
        doc.RootElement.TryGetProperty("room", out _).Should().BeFalse();
    }

    [Fact]
    public void ToJwt_WithVideoGrant_AllProperties_SerializesCorrectly()
    {
        var jwt = CreateBuilder()
            .WithVideoGrant(g =>
            {
                g.RoomCreate = true;
                g.RoomJoin = true;
                g.RoomList = true;
                g.RoomRecord = true;
                g.RoomAdmin = true;
                g.Room = "test-room";
                g.IngressAdmin = true;
                g.CanPublish = true;
                g.CanSubscribe = true;
                g.CanPublishData = true;
                g.CanUpdateOwnMetadata = true;
                g.Hidden = false;
                g.Recorder = true;
                g.Agent = true;
                g.CanSubscribeMetrics = true;
                g.DestinationRoom = "dest-room";
                g.CanPublishSources = new List<string> { "camera", "microphone" };
            })
            .ToJwt();
        var token = ReadToken(jwt);

        var videoJson = GetClaim(token, "video");
        var doc = JsonDocument.Parse(videoJson!);
        var root = doc.RootElement;

        root.GetProperty("roomCreate").GetBoolean().Should().BeTrue();
        root.GetProperty("roomJoin").GetBoolean().Should().BeTrue();
        root.GetProperty("roomList").GetBoolean().Should().BeTrue();
        root.GetProperty("roomRecord").GetBoolean().Should().BeTrue();
        root.GetProperty("roomAdmin").GetBoolean().Should().BeTrue();
        root.GetProperty("room").GetString().Should().Be("test-room");
        root.GetProperty("ingressAdmin").GetBoolean().Should().BeTrue();
        root.GetProperty("canPublish").GetBoolean().Should().BeTrue();
        root.GetProperty("canSubscribe").GetBoolean().Should().BeTrue();
        root.GetProperty("canPublishData").GetBoolean().Should().BeTrue();
        root.GetProperty("canUpdateOwnMetadata").GetBoolean().Should().BeTrue();
        root.GetProperty("hidden").GetBoolean().Should().BeFalse();
        root.GetProperty("recorder").GetBoolean().Should().BeTrue();
        root.GetProperty("agent").GetBoolean().Should().BeTrue();
        root.GetProperty("canSubscribeMetrics").GetBoolean().Should().BeTrue();
        root.GetProperty("destinationRoom").GetString().Should().Be("dest-room");
    }

    [Fact]
    public void ToJwt_WithVideoGrant_CanPublishSources_SerializesAsList()
    {
        var jwt = CreateBuilder()
            .WithVideoGrant(g => { g.CanPublishSources = new List<string> { "camera", "microphone" }; })
            .ToJwt();
        var token = ReadToken(jwt);

        var videoJson = GetClaim(token, "video");
        var doc = JsonDocument.Parse(videoJson!);
        var sources = doc.RootElement.GetProperty("canPublishSources");
        sources.ValueKind.Should().Be(JsonValueKind.Array);
        sources.GetArrayLength().Should().Be(2);
        sources[0].GetString().Should().Be("camera");
        sources[1].GetString().Should().Be("microphone");
    }

    [Fact]
    public void ToJwt_WithSipGrant_SerializesToJson()
    {
        var jwt = CreateBuilder()
            .WithSipGrant(s =>
            {
                s.Admin = true;
                s.Call = true;
            })
            .ToJwt();
        var token = ReadToken(jwt);

        var sipJson = GetClaim(token, "sip");
        sipJson.Should().NotBeNull();

        var doc = JsonDocument.Parse(sipJson);
        doc.RootElement.GetProperty("admin").GetBoolean().Should().BeTrue();
        doc.RootElement.GetProperty("call").GetBoolean().Should().BeTrue();
    }

    [Fact]
    public void ToJwt_WithRoomConfiguration_SerializesToProtobufJson()
    {
        var roomConfig = new RoomConfiguration
        {
            Name = "config-room",
            MaxParticipants = 10
        };

        var jwt = CreateBuilder().WithRoomConfiguration(roomConfig).ToJwt();
        var token = ReadToken(jwt);

        var roomConfigJson = GetClaim(token, "roomConfig");
        roomConfigJson.Should().NotBeNull();
        roomConfigJson.Should().Contain("config-room");
    }

    [Fact]
    public void ToJwt_WithAllPropertiesSet_IncludesAllClaims()
    {
        var jwt = CreateBuilder()
            .WithParticipantName("Full User")
            .WithMetadata("full-metadata")
            .WithKind("standard")
            .WithRoomPreset("full-preset")
            .WithAttribute("role", "admin")
            .WithVideoGrant(g => { g.RoomJoin = true; })
            .WithSipGrant(s => { s.Admin = true; })
            .WithRoomConfiguration(new RoomConfiguration { Name = "full-room" })
            .WithTimeToLive(TimeSpan.FromMinutes(15))
            .ToJwt();
        var token = ReadToken(jwt);

        token.Subject.Should().Be(Identity);
        token.Issuer.Should().Be(ApiKey);
        GetClaim(token, "name").Should().Be("Full User");
        GetClaim(token, "metadata").Should().Be("full-metadata");
        GetClaim(token, "kind").Should().Be("standard");
        GetClaim(token, "roomPreset").Should().Be("full-preset");
        GetClaim(token, "attributes").Should().NotBeNull();
        GetClaim(token, "video").Should().NotBeNull();
        GetClaim(token, "sip").Should().NotBeNull();
        GetClaim(token, "roomConfig").Should().NotBeNull();

        var duration = token.ValidTo - token.ValidFrom;
        duration.Should().BeCloseTo(TimeSpan.FromMinutes(15), TimeSpan.FromSeconds(5));
    }
}
