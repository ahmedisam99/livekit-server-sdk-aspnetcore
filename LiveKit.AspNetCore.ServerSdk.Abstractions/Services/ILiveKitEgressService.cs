using System.Threading;
using System.Threading.Tasks;
using LiveKit.Proto;

namespace LiveKit.Services;

/// <summary>
/// Egress service for recording and streaming rooms, participants, or tracks.
/// </summary>
public interface ILiveKitEgressService
{
    /// <summary>
    /// Starts a room composite egress (recording or streaming).
    /// </summary>
    Task<EgressInfo> StartRoomCompositeEgressAsync(RoomCompositeEgressRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Starts a web egress to record any website.
    /// </summary>
    Task<EgressInfo> StartWebEgressAsync(WebEgressRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Starts a participant egress to record a single participant.
    /// </summary>
    Task<EgressInfo> StartParticipantEgressAsync(ParticipantEgressRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Starts a track composite egress to record specific tracks.
    /// </summary>
    Task<EgressInfo> StartTrackCompositeEgressAsync(TrackCompositeEgressRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Starts a track egress to record a single track without transcoding.
    /// </summary>
    Task<EgressInfo> StartTrackEgressAsync(TrackEgressRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the layout of a web composite egress.
    /// </summary>
    Task<EgressInfo> UpdateLayoutAsync(UpdateLayoutRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds or removes stream endpoints for an active egress.
    /// </summary>
    Task<EgressInfo> UpdateStreamAsync(UpdateStreamRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists active and completed egresses.
    /// </summary>
    Task<ListEgressResponse> ListEgressAsync(ListEgressRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Stops an active egress.
    /// </summary>
    Task<EgressInfo> StopEgressAsync(StopEgressRequest request, CancellationToken cancellationToken = default);
}

