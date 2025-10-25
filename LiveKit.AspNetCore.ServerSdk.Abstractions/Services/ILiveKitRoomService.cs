using System.Threading;
using System.Threading.Tasks;
using LiveKit.Proto;

namespace LiveKit.Services;

/// <summary>
/// Room service for managing LiveKit rooms and participants.
/// </summary>
public interface ILiveKitRoomService
{
    /// <summary>
    /// Creates a room with settings. Requires `roomCreate` permission.
    /// This method is optional; rooms are automatically created when clients connect to them for the first time.
    /// </summary>
    Task<Room> CreateRoomAsync(CreateRoomRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists rooms that are active on the server. Requires `roomList` permission.
    /// </summary>
    Task<ListRoomsResponse> ListRoomsAsync(ListRoomsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an existing room by name or id. Requires `roomCreate` permission.
    /// DeleteRoom will disconnect all participants that are currently in the room.
    /// </summary>
    Task<DeleteRoomResponse> DeleteRoomAsync(DeleteRoomRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists participants in a room. Requires `roomAdmin` permission.
    /// </summary>
    Task<ListParticipantsResponse> ListParticipantsAsync(ListParticipantsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets information on a specific participant. Requires `roomAdmin` permission.
    /// </summary>
    Task<ParticipantInfo> GetParticipantAsync(RoomParticipantIdentity request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a participant from room. Requires `roomAdmin` permission.
    /// </summary>
    Task<RemoveParticipantResponse> RemoveParticipantAsync(RoomParticipantIdentity request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Mutes or unmutes a participant's track. Requires `roomAdmin` permission.
    /// </summary>
    Task<MuteRoomTrackResponse> MutePublishedTrackAsync(MuteRoomTrackRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates participant metadata. Will cause updates to be broadcasted to everyone in the room. Requires `roomAdmin` permission.
    /// </summary>
    Task<ParticipantInfo> UpdateParticipantAsync(UpdateParticipantRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Subscribes or unsubscribes a participant from tracks. Requires `roomAdmin` permission.
    /// </summary>
    Task<UpdateSubscriptionsResponse> UpdateSubscriptionsAsync(UpdateSubscriptionsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends data over data channel to participants in a room. Requires `roomAdmin` permission.
    /// </summary>
    Task<SendDataResponse> SendDataAsync(SendDataRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates room metadata. Will cause updates to be broadcasted to everyone in the room. Requires `roomAdmin` permission.
    /// </summary>
    Task<Room> UpdateRoomMetadataAsync(UpdateRoomMetadataRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cloud-only: Forwards a connected participant's track(s) to another room. Requires `roomAdmin` and `destinationRoom` permissions.
    /// The forwarding will stop when the participant leaves the room or RemoveParticipant has been called in the destination room.
    /// </summary>
    Task<ForwardParticipantResponse> ForwardParticipantAsync(ForwardParticipantRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cloud-only: Moves a connected participant to a different room. Requires `roomAdmin` and `destinationRoom` permissions.
    /// The participant will be removed from the current room and added to the destination room.
    /// </summary>
    Task<MoveParticipantResponse> MoveParticipantAsync(MoveParticipantRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Performs an RPC call to a participant.
    /// </summary>
    Task<PerformRpcResponse> PerformRpcAsync(PerformRpcRequest request, CancellationToken cancellationToken = default);
}
