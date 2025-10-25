namespace LiveKit;

public static class LiveKitWebhookEvents
{
    public const string ROOM_STARTED = "room_started";
    public const string ROOM_FINISHED = "room_finished";
    public const string PARTICIPANT_JOINED = "participant_joined";
    public const string PARTICIPANT_LEFT = "participant_left";
    public const string PARTICIPANT_CONNECTION_ABORTED = "participant_connection_aborted";
    public const string TRACK_PUBLISHED = "track_published";
    public const string TRACK_UNPUBLISHED = "track_unpublished";
    public const string EGRESS_STARTED = "egress_started";
    public const string EGRESS_UPDATED = "egress_updated";
    public const string EGRESS_ENDED = "egress_ended";
    public const string INGRESS_STARTED = "ingress_started";
    public const string INGRESS_ENDED = "ingress_ended";
}
