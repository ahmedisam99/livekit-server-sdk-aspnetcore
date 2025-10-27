---
_layout: landing
---

# LiveKit ASP.NET Core Server SDK

[API Reference](/api/LiveKit.html)
[LiveKit Protocol v1.42.2](https://github.com/livekit/protocol/tree/v1.42.2)

ASP.NET Core SDK for LiveKit Server API. Provides HTTP clients and services for managing rooms, participants, recordings, ingress, SIP, and more.

## Features

- ✅ **Room Management** - Create, list, and delete rooms
- ✅ **Participant Management** - Manage participants, tracks, and subscriptions
- ✅ **Egress** - Record and stream rooms, participants, or tracks
- ✅ **Ingress** - Bring external media (RTMP, WHIP, URL pull) into LiveKit
- ✅ **SIP Integration** - Manage SIP trunks, dispatch rules, and make calls
- ✅ **Token Generation** - JWT token builder with fluent API
- ✅ **Webhook Validation** - Two-layer security (JWT + SHA256)
- ✅ **Agent Dispatch** - Deploy AI agents to rooms
- ✅ **Phone Numbers** - Search, purchase, and manage phone numbers

## Installation

Install the packages from NuGet:

```bash
dotnet add package LiveKit.AspNetCore.ServerSdk
dotnet add package LiveKit.AspNetCore.ServerSdk.Abstractions
```

Or via Package Manager:

```ps1
Install-Package LiveKit.AspNetCore.ServerSdk
Install-Package LiveKit.AspNetCore.ServerSdk.Abstractions
```

## Quick Start

### 1. Configure Services

In your `Program.cs` or `Startup.cs`:

```csharp
builder.Services.AddLiveKit(options =>
{
    options.BaseUrl = "wss://your-livekit-server.com";
    options.ApiKey = "your-api-key";
    options.ApiSecret = "your-api-secret";
});
```

### 2. Generate Access Tokens

```csharp
public class TokenController : ControllerBase
{
    private readonly ILiveKitTokenService _tokenService;

    public TokenController(ILiveKitTokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpGet]
    public IActionResult GetToken(string identity, string roomName)
    {
        var token = _tokenService
            .CreateTokenBuilder(identity)
            .WithKind(ParticipantInfo.Types.Kind.Standard)
            .WithVideoGrant(video =>
            {
                video.RoomJoin = true;
                video.Room = roomName;
                video.CanPublish = true;
                video.CanSubscribe = true;
            })
            .ToJwt();

        return Ok(new { token });
    }
}
```

### 3. Manage Rooms

```csharp
public class RoomsController : ControllerBase
{
    private readonly ILiveKitRoomService _roomService;

    public RoomsController(ILiveKitRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoom(string name)
    {
        var request = new CreateRoomRequest { Name = name };
        var room = await _roomService.CreateRoomAsync(request);
        return Ok(room);
    }

    [HttpGet]
    public async Task<IActionResult> ListRooms()
    {
        var request = new ListRoomsRequest();
        var response = await _roomService.ListRoomsAsync(request);
        return Ok(response);
    }

    [HttpDelete("{roomName}")]
    public async Task<IActionResult> DeleteRoom(string roomName)
    {
        var request = new DeleteRoomRequest { Room = roomName };
        var response = await _roomService.DeleteRoomAsync(request);
        return NoContent();
    }
}
```

### 4. Handle Webhooks

Create a controller for receiving webhook events:

```csharp
public class WebhookController : ControllerBase
{
    private readonly ILiveKitWebhookReceiver _webhookReceiver;

    public WebhookController(ILiveKitWebhookReceiver webhookReceiver)
    {
        _webhookReceiver = webhookReceiver;
    }

    [HttpPost("/webhook")]
    public async Task<IActionResult> ReceiveWebhook()
    {
        var body = await new StreamReader(Request.Body).ReadToEndAsync();
        var authHeader = Request.Headers["Authorization"].ToString();

        var webhookEvent = _webhookReceiver.Receive(body, authHeader);

        // Handle webhook event
        switch (webhookEvent.Event)
        {
            case LiveKitWebhookEvents.ROOM_STARTED:
                // Handle room started
                break;
            case LiveKitWebhookEvents.PARTICIPANT_JOINED:
                // Handle participant joined
                break;
            case LiveKitWebhookEvents.PARTICIPANT_LEFT:
                // Handle participant left
                break;
            case LiveKitWebhookEvents.TRACK_PUBLISHED:
                // Handle track published
                break;
            // Add more cases as needed
        }

        return Ok();
    }
}
```

## Available Services

### Room Service
[`ILiveKitRoomService`](/api/LiveKit.Services.ILiveKitRoomService.html) - Manage rooms and participants

### Egress Service
[`ILiveKitEgressService`](/api/LiveKit.Services.ILiveKitEgressService.html) - Record and stream content

### Ingress Service
[`ILiveKitIngressService`](/api/LiveKit.Services.ILiveKitIngressService.html) - Create ingress endpoints for external media

### SIP Service
[`ILiveKitSipService`](/api/LiveKit.Services.ILiveKitSipService.html) - Manage SIP trunks and dispatch rules

### Token Service
[`ILiveKitTokenService`](/api/LiveKit.Services.ILiveKitTokenService.html) - Generate JWT tokens for client authentication

### Webhook Receiver
[`ILiveKitWebhookReceiver`](/api/LiveKit.Services.ILiveKitWebhookReceiver.html) - Validate and parse webhook events

### Additional Services
- [`ILiveKitAgentDispatchService`](/api/LiveKit.Services.ILiveKitAgentDispatchService.html) - Deploy agents to rooms
- [`ILiveKitPhoneNumberService`](/api/LiveKit.Services.ILiveKitPhoneNumberService.html) - Manage phone numbers
- [`ILiveKitCloudAgentService`](/api/LiveKit.Services.ILiveKitCloudAgentService.html) - Manage LiveKit Cloud agents

## Documentation

- [LiveKit Documentation](https://docs.livekit.io/)
- [Authentication Guide](https://docs.livekit.io/home/get-started/authentication/)
- [Server APIs](https://docs.livekit.io/reference/server/server-apis)
- [RoomService APIs](https://docs.livekit.io/reference/server/server-apis/#room-service)
- [Egress APIs](https://docs.livekit.io/home/egress/api/)
- [Ingress APIs](https://docs.livekit.io/home/ingress/overview/#api)
- [SIP APIs](https://docs.livekit.io/sip/api/)
- [Webhooks](https://docs.livekit.io/home/server/webhooks/)

