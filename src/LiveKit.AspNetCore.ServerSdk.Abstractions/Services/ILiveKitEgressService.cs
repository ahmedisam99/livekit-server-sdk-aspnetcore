using System.Threading;
using System.Threading.Tasks;
using LiveKit.Proto;

namespace LiveKit.Services;

/// <summary>
/// Service for recording and streaming rooms, participants, or tracks.
/// <para>
/// Provides methods for starting and managing egress (recording/streaming) sessions.
/// See <see href="https://docs.livekit.io/home/egress/api/">LiveKit Egress API Documentation</see> for more information.
/// </para>
/// </summary>
public interface ILiveKitEgressService
{
    /// <summary>
    /// Starts a room composite egress (recording or streaming).
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<EgressInfo> StartRoomCompositeEgressAsync(RoomCompositeEgressRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Starts a web egress to record any website.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<EgressInfo> StartWebEgressAsync(WebEgressRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Starts a participant egress to record a single participant.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<EgressInfo> StartParticipantEgressAsync(ParticipantEgressRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Starts a track composite egress to record specific tracks.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<EgressInfo> StartTrackCompositeEgressAsync(TrackCompositeEgressRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Starts a track egress to record a single track without transcoding.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<EgressInfo> StartTrackEgressAsync(TrackEgressRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the layout of a web composite egress.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<EgressInfo> UpdateLayoutAsync(UpdateLayoutRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds or removes stream endpoints for an active egress.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<EgressInfo> UpdateStreamAsync(UpdateStreamRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists active and completed egresses.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<ListEgressResponse> ListEgressAsync(ListEgressRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Stops an active egress.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<EgressInfo> StopEgressAsync(StopEgressRequest request, CancellationToken cancellationToken = default);
}
