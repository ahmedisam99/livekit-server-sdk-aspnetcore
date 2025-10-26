using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LiveKit.Proto;
using LiveKit.Authentication;
using Microsoft.Extensions.Logging;

namespace LiveKit.Services;

/// <inheritdoc cref="ILiveKitAgentDispatchService" />
public sealed class LiveKitAgentDispatchService : TwirpClient, ILiveKitAgentDispatchService
{
    private const string ServiceName = "AgentDispatchService";

    /// <summary>
    /// Initializes a new instance of the <see cref="LiveKitAgentDispatchService"/> class.
    /// </summary>
    public LiveKitAgentDispatchService(HttpClient httpClient, ILogger<LiveKitAgentDispatchService> logger, ILiveKitTokenService _tokenService)
        : base(httpClient, logger, ServiceName, _tokenService)
    {
    }

    /// <inheritdoc/>
    public async Task<AgentDispatch> CreateDispatchAsync(CreateAgentDispatchRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<AgentDispatch>("CreateDispatch", request.Room, request, null, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<AgentDispatch> DeleteDispatchAsync(DeleteAgentDispatchRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<AgentDispatch>("DeleteDispatch", request.Room, request, null, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<ListAgentDispatchResponse> ListDispatchAsync(ListAgentDispatchRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ListAgentDispatchResponse>("ListDispatch", request.Room, request, null, cancellationToken);
    }
}
