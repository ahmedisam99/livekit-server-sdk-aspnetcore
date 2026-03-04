using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using LiveKit.Authentication;
using LiveKit.Services;
using Moq;

namespace LiveKit.Tests.Services;

public class LiveKitWebhookReceiverTests
{
    private const string ValidWebhookBody = "{\"event\": \"room_started\"}";

    private static string ComputeSha256(string body)
    {
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(body));
        return Convert.ToBase64String(hash);
    }

    [Fact]
    public void Constructor_WithNullVerifier_ThrowsArgumentNullException()
    {
        var act = () => new LiveKitWebhookReceiver(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task ReceiveAsync_WithSkipAuth_ParsesBodyWithoutValidation()
    {
        var mockVerifier = new Mock<ILiveKitTokenVerifier>();
        var receiver = new LiveKitWebhookReceiver(mockVerifier.Object);

        var result = await receiver.ReceiveAsync(ValidWebhookBody, skipAuth: true);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task ReceiveAsync_WithSkipAuth_DoesNotCallTokenVerifier()
    {
        var mockVerifier = new Mock<ILiveKitTokenVerifier>();
        var receiver = new LiveKitWebhookReceiver(mockVerifier.Object);

        await receiver.ReceiveAsync(ValidWebhookBody, skipAuth: true);

        mockVerifier.Verify(
            v => v.VerifyAsync(It.IsAny<string>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task ReceiveAsync_WithNullBody_ThrowsArgumentException()
    {
        var mockVerifier = new Mock<ILiveKitTokenVerifier>();
        var receiver = new LiveKitWebhookReceiver(mockVerifier.Object);

        var act = () => receiver.ReceiveAsync(null!, skipAuth: true);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task ReceiveAsync_WithEmptyBody_ThrowsArgumentException()
    {
        var mockVerifier = new Mock<ILiveKitTokenVerifier>();
        var receiver = new LiveKitWebhookReceiver(mockVerifier.Object);

        var act = () => receiver.ReceiveAsync("", skipAuth: true);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task ReceiveAsync_WithMissingAuthHeader_ThrowsArgumentException()
    {
        var mockVerifier = new Mock<ILiveKitTokenVerifier>();
        var receiver = new LiveKitWebhookReceiver(mockVerifier.Object);

        var act = () => receiver.ReceiveAsync(ValidWebhookBody, authorizationHeader: null, skipAuth: false);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task ReceiveAsync_WithValidSha256_ReturnsWebhookEvent()
    {
        var sha256 = ComputeSha256(ValidWebhookBody);
        var claims = new Dictionary<string, string> { { "sha256", sha256 } };

        var mockVerifier = new Mock<ILiveKitTokenVerifier>();
        mockVerifier
            .Setup(v => v.VerifyAsync(It.IsAny<string>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(claims);

        var receiver = new LiveKitWebhookReceiver(mockVerifier.Object);
        var result = await receiver.ReceiveAsync(ValidWebhookBody, authorizationHeader: "test-token", skipAuth: false);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task ReceiveAsync_WithMismatchedSha256_ThrowsInvalidOperationException()
    {
        var claims = new Dictionary<string, string> { { "sha256", "wrong-hash" } };

        var mockVerifier = new Mock<ILiveKitTokenVerifier>();
        mockVerifier
            .Setup(v => v.VerifyAsync(It.IsAny<string>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(claims);

        var receiver = new LiveKitWebhookReceiver(mockVerifier.Object);
        var act = () => receiver.ReceiveAsync(ValidWebhookBody, authorizationHeader: "test-token", skipAuth: false);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*tampered*");
    }

    [Fact]
    public async Task ReceiveAsync_WithNoSha256Claim_SkipsSha256Check()
    {
        var claims = new Dictionary<string, string> { { "sub", "server" } };

        var mockVerifier = new Mock<ILiveKitTokenVerifier>();
        mockVerifier
            .Setup(v => v.VerifyAsync(It.IsAny<string>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(claims);

        var receiver = new LiveKitWebhookReceiver(mockVerifier.Object);
        var result = await receiver.ReceiveAsync(ValidWebhookBody, authorizationHeader: "test-token", skipAuth: false);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task ReceiveAsync_WhenTokenVerificationFails_ThrowsArgumentException()
    {
        var mockVerifier = new Mock<ILiveKitTokenVerifier>();
        mockVerifier
            .Setup(v => v.VerifyAsync(It.IsAny<string>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Token invalid"));

        var receiver = new LiveKitWebhookReceiver(mockVerifier.Object);
        var act = () => receiver.ReceiveAsync(ValidWebhookBody, authorizationHeader: "bad-token", skipAuth: false);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("*verification failed*");
    }

    [Fact]
    public async Task ReceiveAsync_WithInvalidJson_ThrowsArgumentException()
    {
        var mockVerifier = new Mock<ILiveKitTokenVerifier>();
        var receiver = new LiveKitWebhookReceiver(mockVerifier.Object);

        var act = () => receiver.ReceiveAsync("not valid json {{{", skipAuth: true);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("*parse*");
    }

    [Fact]
    public async Task ReceiveAsync_WithUnknownFields_IgnoresThemGracefully()
    {
        var bodyWithUnknownFields = "{\"event\": \"room_started\", \"unknownField\": \"value\", \"anotherUnknown\": 42}";

        var mockVerifier = new Mock<ILiveKitTokenVerifier>();
        var receiver = new LiveKitWebhookReceiver(mockVerifier.Object);

        var result = await receiver.ReceiveAsync(bodyWithUnknownFields, skipAuth: true);

        result.Should().NotBeNull();
    }
}
