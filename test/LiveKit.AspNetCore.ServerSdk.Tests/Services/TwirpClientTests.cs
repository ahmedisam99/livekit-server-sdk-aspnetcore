using System.Net;
using FluentAssertions;
using LiveKit.Authentication;
using LiveKit.Services;
using LiveKit.Tests.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using LiveKit.Proto;

namespace LiveKit.Tests.Services;

public class TwirpClientTests
{
    private const string ApiKey = "test-api-key";
    private const string ApiSecret = "test-api-secret-that-is-long-enough";
    private const string BaseUrl = "https://livekit.example.com";

    private static LiveKitTokenService CreateTokenService() =>
        new(Options.Create(new LiveKitOptions
        {
            ApiKey = ApiKey,
            ApiSecret = ApiSecret
        }));

    private static LiveKitRoomService CreateRoomService(MockHttpMessageHandler handler)
    {
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri(BaseUrl) };
        var logger = NullLogger<LiveKitRoomService>.Instance;
        var tokenService = CreateTokenService();
        var service = new LiveKitRoomService(httpClient, logger, tokenService);
        return service;
    }

    [Fact]
    public async Task MakeRequest_ConstructsCorrectTwirpUrl()
    {
        var handler = MockHttpMessageHandler.WithJsonResponse("{\"sid\": \"room-id\", \"name\": \"test-room\"}");
        var service = CreateRoomService(handler);

        await service.CreateRoomAsync(new CreateRoomRequest { Name = "test-room" });

        handler.LastRequest!.RequestUri!.PathAndQuery.Should().Be("/twirp/livekit.RoomService/CreateRoom");
    }

    [Fact]
    public async Task MakeRequest_UsesPostMethod()
    {
        var handler = MockHttpMessageHandler.WithJsonResponse("{\"sid\": \"room-id\", \"name\": \"test-room\"}");
        var service = CreateRoomService(handler);

        await service.CreateRoomAsync(new CreateRoomRequest { Name = "test-room" });

        handler.LastRequest!.Method.Should().Be(HttpMethod.Post);
    }

    [Fact]
    public async Task MakeRequest_SetsAuthorizationBearerHeader()
    {
        var handler = MockHttpMessageHandler.WithJsonResponse("{\"sid\": \"room-id\", \"name\": \"test-room\"}");
        var service = CreateRoomService(handler);

        await service.CreateRoomAsync(new CreateRoomRequest { Name = "test-room" });

        handler.LastRequest!.Headers.Authorization.Should().NotBeNull();
        handler.LastRequest.Headers.Authorization!.Scheme.Should().Be("Bearer");
        handler.LastRequest.Headers.Authorization.Parameter.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task MakeRequest_SerializesRequestBodyAsProtobufJson()
    {
        var handler = MockHttpMessageHandler.WithJsonResponse("{\"sid\": \"room-id\", \"name\": \"test-room\"}");
        var service = CreateRoomService(handler);

        await service.CreateRoomAsync(new CreateRoomRequest { Name = "test-room", MaxParticipants = 10 });

        handler.LastRequestBody.Should().NotBeNull();
        handler.LastRequestBody.Should().Contain("test-room");
        handler.LastRequestBody.Should().Contain("10");
    }

    [Fact]
    public async Task MakeRequest_SetsContentTypeToApplicationJson()
    {
        var handler = MockHttpMessageHandler.WithJsonResponse("{\"sid\": \"room-id\", \"name\": \"test-room\"}");
        var service = CreateRoomService(handler);

        await service.CreateRoomAsync(new CreateRoomRequest { Name = "test-room" });

        handler.LastRequest!.Content!.Headers.ContentType!.MediaType.Should().Be("application/json");
    }

    [Fact]
    public async Task MakeRequest_WithValidJsonResponse_DeserializesProtobufMessage()
    {
        var handler = MockHttpMessageHandler.WithJsonResponse("{\"sid\": \"RM_123\", \"name\": \"test-room\", \"maxParticipants\": 10}");
        var service = CreateRoomService(handler);

        var room = await service.CreateRoomAsync(new CreateRoomRequest { Name = "test-room" });

        room.Should().NotBeNull();
        room.Name.Should().Be("test-room");
        room.MaxParticipants.Should().Be(10);
    }

    [Fact]
    public async Task MakeRequest_WithEmptyJsonResponse_ReturnsDefaultInstance()
    {
        var handler = MockHttpMessageHandler.WithJsonResponse("{}");
        var service = CreateRoomService(handler);

        var room = await service.CreateRoomAsync(new CreateRoomRequest { Name = "test-room" });

        room.Should().NotBeNull();
        room.Name.Should().BeEmpty();
    }

    [Fact]
    public async Task MakeRequest_WithHttpErrorStatus_ThrowsHttpRequestException()
    {
        var handler = MockHttpMessageHandler.WithJsonResponse("{\"msg\": \"error\"}", HttpStatusCode.InternalServerError);
        var service = CreateRoomService(handler);

        var act = () => service.CreateRoomAsync(new CreateRoomRequest { Name = "test-room" });

        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Fact]
    public async Task MakeRequest_ErrorMessage_IncludesUrlAndStatusCode()
    {
        var handler = MockHttpMessageHandler.WithJsonResponse("{\"msg\": \"not found\"}", HttpStatusCode.NotFound);
        var service = CreateRoomService(handler);

        var act = () => service.CreateRoomAsync(new CreateRoomRequest { Name = "test-room" });

        var ex = await act.Should().ThrowAsync<HttpRequestException>();
        ex.Which.Message.Should().Contain("/twirp/livekit.RoomService/CreateRoom");
        ex.Which.Message.Should().Contain("NotFound");
    }
}
