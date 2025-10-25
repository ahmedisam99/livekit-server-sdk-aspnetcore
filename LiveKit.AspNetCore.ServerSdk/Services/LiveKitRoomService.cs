using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LiveKit.Proto;
using LiveKit.Authentication;
using Microsoft.Extensions.Logging;

namespace LiveKit.Services;

public sealed class LiveKitRoomService : TwirpClient, ILiveKitRoomService
{
    private const string ServiceName = "RoomService";

    public LiveKitRoomService(HttpClient httpClient, ILogger<LiveKitRoomService> logger, ILiveKitTokenService _tokenService)
        : base(httpClient, logger, ServiceName, _tokenService)
    {
    }

    public async Task<Room> CreateRoomAsync(CreateRoomRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<Room>("CreateRoom", request.Name, request, null, cancellationToken);
    }

    public async Task<ListRoomsResponse> ListRoomsAsync(ListRoomsRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ListRoomsResponse>("ListRooms", null, request, null, cancellationToken);
    }

    public async Task<DeleteRoomResponse> DeleteRoomAsync(DeleteRoomRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<DeleteRoomResponse>("DeleteRoom", request.Room, request, null, cancellationToken);
    }

    public async Task<ListParticipantsResponse> ListParticipantsAsync(ListParticipantsRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ListParticipantsResponse>("ListParticipants", request.Room, request, null, cancellationToken);
    }

    public async Task<ParticipantInfo> GetParticipantAsync(RoomParticipantIdentity request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ParticipantInfo>("GetParticipant", request.Room, request, null, cancellationToken);
    }

    public async Task<RemoveParticipantResponse> RemoveParticipantAsync(RoomParticipantIdentity request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<RemoveParticipantResponse>("RemoveParticipant", request.Room, request, null, cancellationToken);
    }

    public async Task<MuteRoomTrackResponse> MutePublishedTrackAsync(MuteRoomTrackRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<MuteRoomTrackResponse>("MutePublishedTrack", request.Room, request, null, cancellationToken);
    }

    public async Task<ParticipantInfo> UpdateParticipantAsync(UpdateParticipantRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ParticipantInfo>("UpdateParticipant", request.Room, request, null, cancellationToken);
    }

    public async Task<UpdateSubscriptionsResponse> UpdateSubscriptionsAsync(UpdateSubscriptionsRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<UpdateSubscriptionsResponse>("UpdateSubscriptions", request.Room, request, null, cancellationToken);
    }

    public async Task<SendDataResponse> SendDataAsync(SendDataRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<SendDataResponse>("SendData", request.Room, request, null, cancellationToken);
    }

    public async Task<Room> UpdateRoomMetadataAsync(UpdateRoomMetadataRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<Room>("UpdateRoomMetadata", request.Room, request, null, cancellationToken);
    }

    public async Task<ForwardParticipantResponse> ForwardParticipantAsync(ForwardParticipantRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ForwardParticipantResponse>("ForwardParticipant", request.Room, request, null, cancellationToken);
    }

    public async Task<MoveParticipantResponse> MoveParticipantAsync(MoveParticipantRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<MoveParticipantResponse>("MoveParticipant", request.Room, request, null, cancellationToken);
    }

    public async Task<PerformRpcResponse> PerformRpcAsync(PerformRpcRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<PerformRpcResponse>("PerformRpc", request.Room, request, null, cancellationToken);
    }
}
