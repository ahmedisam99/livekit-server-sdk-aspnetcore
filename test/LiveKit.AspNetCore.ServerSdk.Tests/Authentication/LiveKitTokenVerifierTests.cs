using FluentAssertions;
using LiveKit.Authentication;
using Microsoft.Extensions.Options;

namespace LiveKit.Tests.Authentication;

public class LiveKitTokenVerifierTests
{
    private const string ApiKey = "test-api-key";
    private const string ApiSecret = "test-api-secret-that-is-long-enough";
    private const string Identity = "test-user";

    private static IOptions<LiveKitOptions> CreateOptions(string? apiKey = ApiKey, string? apiSecret = ApiSecret) =>
        Options.Create(new LiveKitOptions
        {
            ApiKey = apiKey ?? string.Empty,
            ApiSecret = apiSecret ?? string.Empty
        });

    private static LiveKitTokenVerifier CreateVerifier(string? apiKey = ApiKey, string? apiSecret = ApiSecret) =>
        new(CreateOptions(apiKey, apiSecret));

    private static string CreateToken(string identity = Identity, string apiKey = ApiKey, string apiSecret = ApiSecret,
        TimeSpan? ttl = null)
    {
        var builder = new LiveKitTokenBuilder(apiKey, apiSecret, identity);
        if (ttl.HasValue)
            builder.WithTimeToLive(ttl.Value);
        return builder.ToJwt();
    }

    [Fact]
    public void Constructor_WithMissingApiKey_ThrowsInvalidOperationException()
    {
        var act = () => CreateVerifier(apiKey: "");
        act.Should().Throw<InvalidOperationException>().Which.Message.Should().Contain("API key");
    }

    [Fact]
    public void Constructor_WithMissingApiSecret_ThrowsInvalidOperationException()
    {
        var act = () => CreateVerifier(apiSecret: "");
        act.Should().Throw<InvalidOperationException>().Which.Message.Should().Contain("API secret");
    }

    [Fact]
    public async Task VerifyAsync_WithNullToken_ThrowsArgumentException()
    {
        var verifier = CreateVerifier();
        var act = () => verifier.VerifyAsync(null!);
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task VerifyAsync_WithEmptyToken_ThrowsArgumentException()
    {
        var verifier = CreateVerifier();
        var act = () => verifier.VerifyAsync("");
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task VerifyAsync_WithValidToken_ReturnsClaimsDictionary()
    {
        var verifier = CreateVerifier();
        var token = CreateToken();

        var claims = await verifier.VerifyAsync(token);

        claims.Should().NotBeNull();
        claims.Should().NotBeEmpty();
    }

    [Fact]
    public async Task VerifyAsync_WithValidToken_ExtractsIdentityClaim()
    {
        var verifier = CreateVerifier();
        var token = CreateToken(identity: "my-user");

        var claims = await verifier.VerifyAsync(token);

        claims.Should().ContainKey("sub");
        claims["sub"].Should().Be("my-user");
    }

    [Fact]
    public async Task VerifyAsync_WithTokenContainingName_ExtractsNameClaim()
    {
        var verifier = CreateVerifier();
        var token = new LiveKitTokenBuilder(ApiKey, ApiSecret, Identity)
            .WithParticipantName("Test User")
            .ToJwt();

        var claims = await verifier.VerifyAsync(token);

        claims.Should().ContainKey("name");
        claims["name"].Should().Be("Test User");
    }

    [Fact]
    public async Task VerifyAsync_WithTokenContainingVideoGrant_ExtractsVideoClaim()
    {
        var verifier = CreateVerifier();
        var token = new LiveKitTokenBuilder(ApiKey, ApiSecret, Identity)
            .WithVideoGrant(g =>
            {
                g.RoomJoin = true;
                g.Room = "my-room";
            })
            .ToJwt();

        var claims = await verifier.VerifyAsync(token);

        claims.Should().ContainKey("video");
        claims["video"].Should().Contain("roomJoin");
    }

    [Fact]
    public async Task VerifyAsync_WithExpiredToken_ThrowsException()
    {
        var verifier = CreateVerifier();
        var token = CreateToken(ttl: TimeSpan.FromSeconds(-1));

        var act = () => verifier.VerifyAsync(token, clockTolerance: TimeSpan.Zero);

        await act.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task VerifyAsync_WithWrongApiSecret_ThrowsException()
    {
        var token = CreateToken(apiSecret: "correct-secret-that-is-long-enough");
        var verifier = CreateVerifier(apiSecret: "wrong-secret-that-is-also-long-enough");

        var act = () => verifier.VerifyAsync(token);

        await act.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task VerifyAsync_WithWrongIssuer_ThrowsException()
    {
        var token = CreateToken(apiKey: "correct-key");
        var verifier = CreateVerifier(apiKey: "wrong-key");

        var act = () => verifier.VerifyAsync(token);

        await act.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task VerifyAsync_WithMalformedToken_ThrowsException()
    {
        var verifier = CreateVerifier();

        var act = () => verifier.VerifyAsync("not.a.valid.jwt.token");

        await act.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task VerifyAsync_WithCustomClockTolerance_RespectsOverride()
    {
        var verifier = CreateVerifier();
        var token = CreateToken(ttl: TimeSpan.FromSeconds(1));

        // With a large tolerance, a recently-expired token should still validate
        await Task.Delay(TimeSpan.FromSeconds(2));
        var claims = await verifier.VerifyAsync(token, clockTolerance: TimeSpan.FromMinutes(5));

        claims.Should().NotBeNull();
    }
}
