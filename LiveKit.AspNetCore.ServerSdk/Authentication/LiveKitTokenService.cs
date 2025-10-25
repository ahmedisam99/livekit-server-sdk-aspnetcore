using System;
using Microsoft.Extensions.Options;

namespace LiveKit.Authentication;

public sealed class LiveKitTokenService : ILiveKitTokenService
{
    private readonly LiveKitOptions _options;

    public LiveKitTokenService(IOptions<LiveKitOptions> options)
    {
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));

        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            throw new InvalidOperationException("LiveKit API key is required. Configure it using AddLiveKit().");
        }

        if (string.IsNullOrWhiteSpace(_options.ApiSecret))
        {
            throw new InvalidOperationException("LiveKit API secret is required. Configure it using AddLiveKit().");
        }
    }

    public ILiveKitTokenBuilder CreateTokenBuilder(string identity)
    {
        if (string.IsNullOrWhiteSpace(identity))
        {
            throw new ArgumentException("Identity cannot be null or empty.", nameof(identity));
        }

        return new LiveKitTokenBuilder(_options.ApiKey, _options.ApiSecret, identity);
    }

    public string CreateServerToken(string? roomName = null, TimeSpan? ttl = null)
    {
        return new LiveKitTokenBuilder(_options.ApiKey, _options.ApiSecret, "server")
            .WithParticipantName("server")
            .WithVideoGrant(grant =>
            {
                grant.Room = roomName;
                grant.RoomJoin = true;
                grant.RoomCreate = true;
                grant.RoomList = true;
                grant.RoomAdmin = true;
                grant.RoomRecord = true;
                grant.IngressAdmin = true;
                grant.CanPublish = true;
                grant.CanSubscribe = true;
                grant.CanPublishData = true;
                grant.CanUpdateOwnMetadata = true;
            })
            .WithTimeToLive(ttl ?? TimeSpan.FromHours(1))
            .ToJwt();
    }
}
