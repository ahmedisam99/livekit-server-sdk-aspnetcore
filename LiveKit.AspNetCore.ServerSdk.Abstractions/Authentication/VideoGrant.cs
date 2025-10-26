using System.Collections.Generic;

namespace LiveKit.Authentication;

/// <summary>
/// Video grant containing room permissions and participant capabilities.
/// Room permissions are specified in the video field of a decoded join token.
/// </summary>
public sealed class VideoGrant
{
    /// <summary>
    /// Permission to create or delete rooms.
    /// </summary>
    public bool? RoomCreate { get; set; }

    /// <summary>
    /// Permission to join a room as a participant. Room must be set.
    /// </summary>
    public bool? RoomJoin { get; set; }

    /// <summary>
    /// Permission to list available rooms.
    /// </summary>
    public bool? RoomList { get; set; }

    /// <summary>
    /// Permission to start a recording. Permissions to use Egress service.
    /// </summary>
    public bool? RoomRecord { get; set; }

    /// <summary>
    /// Permission to control a specific room. Room must be set.
    /// </summary>
    public bool? RoomAdmin { get; set; }

    /// <summary>
    /// Name of the room, must be set for admin or join permissions.
    /// </summary>
    public string? Room { get; set; }

    /// <summary>
    /// Permissions to control ingress, not specific to any room or ingress.
    /// </summary>
    public bool? IngressAdmin { get; set; }

    /// <summary>
    /// Allow participant to publish. If neither canPublish or canSubscribe is set,
    /// both publish and subscribe are enabled.
    /// </summary>
    public bool? CanPublish { get; set; }

    /// <summary>
    /// TrackSource types that the participant is allowed to publish.
    /// When set, it supersedes CanPublish. Only sources explicitly set here can be published.
    /// <see cref="LiveKit.Proto.TrackSource"/>
    /// </summary>
    public List<string>? CanPublishSources { get; set; }

    /// <summary>
    /// Allow participant to subscribe to other tracks.
    /// </summary>
    public bool? CanSubscribe { get; set; }

    /// <summary>
    /// Allow participants to publish data, defaults to true if not set.
    /// </summary>
    public bool? CanPublishData { get; set; }

    /// <summary>
    /// By default, a participant is not allowed to update its own metadata.
    /// </summary>
    public bool? CanUpdateOwnMetadata { get; set; }

    /// <summary>
    /// Hide participant from others in the room.
    /// </summary>
    public bool? Hidden { get; set; }

    /// <summary>
    /// Participant is recording the room, when set, allows room to indicate it's being recorded.
    /// </summary>
    public bool? Recorder { get; set; }

    /// <summary>
    /// Participant allowed to connect to LiveKit as Agent Framework worker.
    /// </summary>
    public bool? Agent { get; set; }

    /// <summary>
    /// Allow participant to subscribe to metrics.
    /// </summary>
    public bool? CanSubscribeMetrics { get; set; }

    /// <summary>
    /// Destination room which this participant can forward to.
    /// </summary>
    public string? DestinationRoom { get; set; }
}
