using FluentAssertions;
using LiveKit.Authentication;
using LiveKit.Services;
using LiveKit.Tests.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using LiveKit.Proto;

namespace LiveKit.Tests.Services;

public class LiveKitSipServiceTests
{
    private static LiveKitSipService CreateService(MockHttpMessageHandler handler)
    {
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://livekit.example.com") };
        var tokenService = new LiveKitTokenService(Options.Create(new LiveKitOptions
        {
            ApiKey = "test-api-key",
            ApiSecret = "test-api-secret-that-is-long-enough"
        }));
        return new LiveKitSipService(httpClient, NullLogger<LiveKitSipService>.Instance, tokenService);
    }

    // The server returns TransferSIPParticipantResponse, which upstream plans to empty.
    // The SDK keeps returning Task, so a non-empty body must still be accepted.
    [Fact]
    public async Task TransferSIPParticipantAsync_WithNonEmptyResponse_Succeeds()
    {
        var handler = MockHttpMessageHandler.WithJsonResponse(
            "{\"transferId\": \"TR_123\", \"status\": \"STS_TRANSFER_SUCCESSFUL\", \"sipStatus\": {\"code\": 200, \"status\": \"OK\"}}");
        var service = CreateService(handler);

        var act = () => service.TransferSIPParticipantAsync(new TransferSIPParticipantRequest
        {
            RoomName = "test-room",
            ParticipantIdentity = "caller",
            TransferTo = "tel:+15550100"
        });

        await act.Should().NotThrowAsync();
        handler.LastRequest!.RequestUri!.PathAndQuery.Should().Be("/twirp/livekit.SIP/TransferSIPParticipant");
    }
}
