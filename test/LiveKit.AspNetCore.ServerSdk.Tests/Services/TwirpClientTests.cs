using System.Net;
using System.Text.Json;
using FluentAssertions;
using LiveKit.Authentication;
using LiveKit.Services;
using LiveKit.Tests.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
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
    public async Task ForwardParticipant_TokenIncludesRoomAndDestinationRoom()
    {
        var handler = MockHttpMessageHandler.WithJsonResponse("{}");
        var service = CreateRoomService(handler);

        await service.ForwardParticipantAsync(new ForwardParticipantRequest
        {
            Room = "source-room",
            Identity = "user1",
            DestinationRoom = "destination-room"
        });

        var video = ReadVideoGrant(handler.LastRequest!);
        video.GetProperty("room").GetString().Should().Be("source-room");
        video.GetProperty("destinationRoom").GetString().Should().Be("destination-room");
    }

    [Fact]
    public async Task MoveParticipant_TokenIncludesRoomAndDestinationRoom()
    {
        var handler = MockHttpMessageHandler.WithJsonResponse("{}");
        var service = CreateRoomService(handler);

        await service.MoveParticipantAsync(new MoveParticipantRequest
        {
            Room = "source-room",
            Identity = "user1",
            DestinationRoom = "destination-room"
        });

        var video = ReadVideoGrant(handler.LastRequest!);
        video.GetProperty("room").GetString().Should().Be("source-room");
        video.GetProperty("destinationRoom").GetString().Should().Be("destination-room");
    }

    [Fact]
    public async Task MakeRequest_WithoutDestinationRoom_TokenOmitsDestinationRoom()
    {
        var handler = MockHttpMessageHandler.WithJsonResponse("{\"sid\": \"room-id\", \"name\": \"test-room\"}");
        var service = CreateRoomService(handler);

        await service.CreateRoomAsync(new CreateRoomRequest { Name = "test-room" });

        var video = ReadVideoGrant(handler.LastRequest!);
        video.TryGetProperty("destinationRoom", out _).Should().BeFalse();
    }

    private static JsonElement ReadVideoGrant(HttpRequestMessage request)
    {
        var token = new JsonWebTokenHandler().ReadJsonWebToken(request.Headers.Authorization!.Parameter);
        var videoJson = token.Claims.First(c => c.Type == "video").Value;
        return JsonDocument.Parse(videoJson).RootElement;
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

    [Fact]
    public async Task MakeRequest_WithTwirpErrorResponse_ThrowsLiveKitApiException()
    {
        var handler = MockHttpMessageHandler.WithJsonResponse(
            """{"code": "not_found", "msg": "room not found"}""",
            HttpStatusCode.NotFound);
        var service = CreateRoomService(handler);

        var act = () => service.CreateRoomAsync(new CreateRoomRequest { Name = "test-room" });

        await act.Should().ThrowAsync<LiveKitApiException>();
    }

    [Fact]
    public async Task MakeRequest_WithTwirpErrorResponse_ParsesErrorCodeAndMessage()
    {
        var handler = MockHttpMessageHandler.WithJsonResponse(
            """{"code": "not_found", "msg": "room not found"}""",
            HttpStatusCode.NotFound);
        var service = CreateRoomService(handler);

        var act = () => service.CreateRoomAsync(new CreateRoomRequest { Name = "test-room" });

        var ex = await act.Should().ThrowAsync<LiveKitApiException>();
        ex.Which.Code.Should().Be(TwirpErrorCode.NotFound);
        ex.Which.TwirpMessage.Should().Be("room not found");
        ex.Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task MakeRequest_WithTwirpErrorMeta_ExposesMeta()
    {
        var handler = MockHttpMessageHandler.WithJsonResponse(
            """{"code": "failed_precondition", "msg": "transfer failed", "meta": {"sip_transfer_reason": "STR_REFER_REJECTED", "sip_status_code": "603"}}""",
            HttpStatusCode.PreconditionFailed);
        var service = CreateRoomService(handler);

        var act = () => service.CreateRoomAsync(new CreateRoomRequest { Name = "test-room" });

        var ex = await act.Should().ThrowAsync<LiveKitApiException>();
        ex.Which.Meta.Should().Contain("sip_transfer_reason", "STR_REFER_REJECTED");
        ex.Which.Meta.Should().Contain("sip_status_code", "603");
    }

    [Fact]
    public async Task MakeRequest_WithoutTwirpErrorMeta_MetaIsEmpty()
    {
        var handler = MockHttpMessageHandler.WithJsonResponse(
            """{"code": "not_found", "msg": "room not found"}""",
            HttpStatusCode.NotFound);
        var service = CreateRoomService(handler);

        var act = () => service.CreateRoomAsync(new CreateRoomRequest { Name = "test-room" });

        var ex = await act.Should().ThrowAsync<LiveKitApiException>();
        ex.Which.Meta.Should().BeEmpty();
    }

    [Fact]
    public async Task MakeRequest_WithNonJsonErrorResponse_FallsBackToUnknownCode()
    {
        var handler = MockHttpMessageHandler.WithJsonResponse(
            "Internal Server Error",
            HttpStatusCode.InternalServerError);
        var service = CreateRoomService(handler);

        var act = () => service.CreateRoomAsync(new CreateRoomRequest { Name = "test-room" });

        var ex = await act.Should().ThrowAsync<LiveKitApiException>();
        ex.Which.Code.Should().Be(TwirpErrorCode.Unknown);
        ex.Which.TwirpMessage.Should().Contain("Internal Server Error");
    }

    [Fact]
    public async Task MakeRequest_WithPartialTwirpJson_FallsBackToUnknownCode()
    {
        var handler = MockHttpMessageHandler.WithJsonResponse(
            """{"error": "something went wrong"}""",
            HttpStatusCode.InternalServerError);
        var service = CreateRoomService(handler);

        var act = () => service.CreateRoomAsync(new CreateRoomRequest { Name = "test-room" });

        var ex = await act.Should().ThrowAsync<LiveKitApiException>();
        ex.Which.Code.Should().Be(TwirpErrorCode.Unknown);
    }
}
