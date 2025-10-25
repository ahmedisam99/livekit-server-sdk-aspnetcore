using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LiveKit.Proto;
using LiveKit.Authentication;
using Microsoft.Extensions.Logging;

namespace LiveKit.Services;

public sealed class LiveKitAgentDispatchService : TwirpClient, ILiveKitAgentDispatchService
{
    private const string ServiceName = "AgentDispatchService";

    public LiveKitAgentDispatchService(HttpClient httpClient, ILogger<LiveKitAgentDispatchService> logger, ILiveKitTokenService _tokenService)
        : base(httpClient, logger, ServiceName, _tokenService)
    {
    }

    public async Task<AgentDispatch> CreateDispatchAsync(CreateAgentDispatchRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<AgentDispatch>("CreateDispatch", request.Room, request, null, cancellationToken);
    }

    public async Task<AgentDispatch> DeleteDispatchAsync(DeleteAgentDispatchRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<AgentDispatch>("DeleteDispatch", request.Room, request, null, cancellationToken);
    }

    public async Task<ListAgentDispatchResponse> ListDispatchAsync(ListAgentDispatchRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ListAgentDispatchResponse>("ListDispatch", request.Room, request, null, cancellationToken);
    }
}
