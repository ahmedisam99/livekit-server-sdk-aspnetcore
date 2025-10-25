using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using LiveKit.Proto;
using LiveKit.Authentication;
using Microsoft.Extensions.Logging;

namespace LiveKit.Services;

public sealed class LiveKitSipService : TwirpClient, ILiveKitSipService
{
    private const string ServiceName = "SIP";

    public LiveKitSipService(HttpClient httpClient, ILogger<LiveKitSipService> logger, ILiveKitTokenService _tokenService)
        : base(httpClient, logger, ServiceName, _tokenService)
    {
    }

    [System.Obsolete("Use ListSIPInboundTrunk or ListSIPOutboundTrunk instead")]
    public async Task<ListSIPTrunkResponse> ListSIPTrunkAsync(ListSIPTrunkRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ListSIPTrunkResponse>("ListSIPTrunk", null, request, null, cancellationToken);
    }

    public async Task<SIPInboundTrunkInfo> CreateSIPInboundTrunkAsync(CreateSIPInboundTrunkRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<SIPInboundTrunkInfo>("CreateSIPInboundTrunk", null, request, null, cancellationToken);
    }

    public async Task<SIPOutboundTrunkInfo> CreateSIPOutboundTrunkAsync(CreateSIPOutboundTrunkRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<SIPOutboundTrunkInfo>("CreateSIPOutboundTrunk", null, request, null, cancellationToken);
    }

    public async Task<SIPInboundTrunkInfo> UpdateSIPInboundTrunkAsync(UpdateSIPInboundTrunkRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<SIPInboundTrunkInfo>("UpdateSIPInboundTrunk", null, request, null, cancellationToken);
    }

    public async Task<SIPOutboundTrunkInfo> UpdateSIPOutboundTrunkAsync(UpdateSIPOutboundTrunkRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<SIPOutboundTrunkInfo>("UpdateSIPOutboundTrunk", null, request, null, cancellationToken);
    }

    public async Task<GetSIPInboundTrunkResponse> GetSIPInboundTrunkAsync(GetSIPInboundTrunkRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<GetSIPInboundTrunkResponse>("GetSIPInboundTrunk", null, request, null, cancellationToken);
    }

    public async Task<GetSIPOutboundTrunkResponse> GetSIPOutboundTrunkAsync(GetSIPOutboundTrunkRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<GetSIPOutboundTrunkResponse>("GetSIPOutboundTrunk", null, request, null, cancellationToken);
    }

    public async Task<ListSIPInboundTrunkResponse> ListSIPInboundTrunkAsync(ListSIPInboundTrunkRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ListSIPInboundTrunkResponse>("ListSIPInboundTrunk", null, request, null, cancellationToken);
    }

    public async Task<ListSIPOutboundTrunkResponse> ListSIPOutboundTrunkAsync(ListSIPOutboundTrunkRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ListSIPOutboundTrunkResponse>("ListSIPOutboundTrunk", null, request, null, cancellationToken);
    }

    public async Task<SIPTrunkInfo> DeleteSIPTrunkAsync(DeleteSIPTrunkRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<SIPTrunkInfo>("DeleteSIPTrunk", null, request, null, cancellationToken);
    }

    public async Task<SIPDispatchRuleInfo> CreateSIPDispatchRuleAsync(CreateSIPDispatchRuleRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<SIPDispatchRuleInfo>("CreateSIPDispatchRule", null, request, null, cancellationToken);
    }

    public async Task<SIPDispatchRuleInfo> UpdateSIPDispatchRuleAsync(UpdateSIPDispatchRuleRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<SIPDispatchRuleInfo>("UpdateSIPDispatchRule", null, request, null, cancellationToken);
    }

    public async Task<ListSIPDispatchRuleResponse> ListSIPDispatchRuleAsync(ListSIPDispatchRuleRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ListSIPDispatchRuleResponse>("ListSIPDispatchRule", null, request, null, cancellationToken);
    }

    public async Task<SIPDispatchRuleInfo> DeleteSIPDispatchRuleAsync(DeleteSIPDispatchRuleRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<SIPDispatchRuleInfo>("DeleteSIPDispatchRule", null, request, null, cancellationToken);
    }

    public async Task<SIPParticipantInfo> CreateSIPParticipantAsync(CreateSIPParticipantRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<SIPParticipantInfo>("CreateSIPParticipant", request.RoomName, request, null, cancellationToken);
    }

    public async Task TransferSIPParticipantAsync(TransferSIPParticipantRequest request, CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<Empty>("TransferSIPParticipant", request.RoomName, request, null, cancellationToken);
    }
}
