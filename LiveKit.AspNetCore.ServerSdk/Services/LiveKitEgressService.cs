using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LiveKit.Proto;
using LiveKit.Authentication;
using Microsoft.Extensions.Logging;

namespace LiveKit.Services;

/// <inheritdoc cref="ILiveKitEgressService" />
public sealed class LiveKitEgressService : TwirpClient, ILiveKitEgressService
{
    private const string ServiceName = "Egress";

    /// <summary>
    /// Initializes a new instance of the <see cref="LiveKitEgressService"/> class.
    /// </summary>
    public LiveKitEgressService(HttpClient httpClient, ILogger<LiveKitEgressService> logger, ILiveKitTokenService _tokenService)
        : base(httpClient, logger, ServiceName, _tokenService)
    {
    }

    /// <inheritdoc/>
    public async Task<EgressInfo> StartRoomCompositeEgressAsync(RoomCompositeEgressRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<EgressInfo>("StartRoomCompositeEgress", request.RoomName, request, null, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<EgressInfo> StartWebEgressAsync(WebEgressRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<EgressInfo>("StartWebEgress", null, request, null, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<EgressInfo> StartParticipantEgressAsync(ParticipantEgressRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<EgressInfo>("StartParticipantEgress", request.RoomName, request, null, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<EgressInfo> StartTrackCompositeEgressAsync(TrackCompositeEgressRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<EgressInfo>("StartTrackCompositeEgress", request.RoomName, request, null, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<EgressInfo> StartTrackEgressAsync(TrackEgressRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<EgressInfo>("StartTrackEgress", request.RoomName, request, null, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<EgressInfo> UpdateLayoutAsync(UpdateLayoutRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<EgressInfo>("UpdateLayout", null, request, null, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<EgressInfo> UpdateStreamAsync(UpdateStreamRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<EgressInfo>("UpdateStream", null, request, null, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<ListEgressResponse> ListEgressAsync(ListEgressRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ListEgressResponse>("ListEgress", request.RoomName, request, null, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<EgressInfo> StopEgressAsync(StopEgressRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<EgressInfo>("StopEgress", null, request, null, cancellationToken);
    }
}
