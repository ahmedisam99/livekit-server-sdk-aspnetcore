using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using LiveKit.Authentication;
using LiveKit.Services;
using LiveKit.Tests.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using LiveKit.Proto;

namespace LiveKit.Tests.Services;

public class LiveKitAgentSimulationServiceTests
{
    private static LiveKitAgentSimulationService CreateService(MockHttpMessageHandler handler)
    {
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://livekit.example.com") };
        var tokenService = new LiveKitTokenService(Options.Create(new LiveKitOptions
        {
            ApiKey = "test-api-key",
            ApiSecret = "test-api-secret-that-is-long-enough"
        }));
        return new LiveKitAgentSimulationService(httpClient, NullLogger<LiveKitAgentSimulationService>.Instance, tokenService);
    }

    [Fact]
    public async Task CountSimulationRunsAsync_CallsRouteAndParsesResponse()
    {
        var handler = MockHttpMessageHandler.WithJsonResponse(
            "{\"buckets\": [{\"bucketStart\": \"2026-09-27T00:00:00Z\", \"count\": \"5\"}], \"interval\": \"3600s\"}");
        var service = CreateService(handler);

        var response = await service.CountSimulationRunsAsync(new SimulationRun.Types.Counts.Types.Request
        {
            ProjectId = "p_123",
            StartTime = Timestamp.FromDateTime(new DateTime(2026, 9, 27, 0, 0, 0, DateTimeKind.Utc)),
            EndTime = Timestamp.FromDateTime(new DateTime(2026, 9, 28, 0, 0, 0, DateTimeKind.Utc))
        });

        handler.LastRequest!.RequestUri!.PathAndQuery.Should().Be("/twirp/livekit.AgentSimulation/CountSimulationRuns");
        response.Buckets.Should().ContainSingle(b => b.Count == 5);
        response.Interval.ToTimeSpan().Should().Be(TimeSpan.FromHours(1));
    }
}
