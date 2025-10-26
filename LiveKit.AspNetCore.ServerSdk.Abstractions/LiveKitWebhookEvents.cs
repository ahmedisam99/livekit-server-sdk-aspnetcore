namespace LiveKit;

/// <summary>
/// Constants for LiveKit webhook event types.
/// <para>
/// These constants represent the different event types that LiveKit can send via webhooks.
/// Use these values to check the <c>Event</c> property of a <c>WebhookEvent</c>.
/// </para>
/// <para>
/// See <see href="https://docs.livekit.io/home/server/webhooks/">LiveKit Webhooks Documentation</see> for more information.
/// </para>
/// </summary>
public static class LiveKitWebhookEvents
{
    /// <summary>
    /// Event fired when a room is created and becomes active.
    /// </summary>
    public const string ROOM_STARTED = "room_started";

    /// <summary>
    /// Event fired when a room is closed and all participants have left.
    /// </summary>
    public const string ROOM_FINISHED = "room_finished";

    /// <summary>
    /// Event fired when a participant joins a room.
    /// </summary>
    public const string PARTICIPANT_JOINED = "participant_joined";

    /// <summary>
    /// Event fired when a participant leaves a room.
    /// </summary>
    public const string PARTICIPANT_LEFT = "participant_left";

    /// <summary>
    /// Event fired when a participant's connection is aborted unexpectedly.
    /// </summary>
    public const string PARTICIPANT_CONNECTION_ABORTED = "participant_connection_aborted";

    /// <summary>
    /// Event fired when a participant publishes a new track to the room.
    /// </summary>
    public const string TRACK_PUBLISHED = "track_published";

    /// <summary>
    /// Event fired when a participant unpublishes a track from the room.
    /// </summary>
    public const string TRACK_UNPUBLISHED = "track_unpublished";

    /// <summary>
    /// Event fired when an egress (recording or streaming) session starts.
    /// </summary>
    public const string EGRESS_STARTED = "egress_started";

    /// <summary>
    /// Event fired when an egress session is updated (e.g., layout changes).
    /// </summary>
    public const string EGRESS_UPDATED = "egress_updated";

    /// <summary>
    /// Event fired when an egress session completes.
    /// </summary>
    public const string EGRESS_ENDED = "egress_ended";

    /// <summary>
    /// Event fired when an ingress (RTMP, WHIP, or URL pull) session starts.
    /// </summary>
    public const string INGRESS_STARTED = "ingress_started";

    /// <summary>
    /// Event fired when an ingress session ends.
    /// </summary>
    public const string INGRESS_ENDED = "ingress_ended";
}
