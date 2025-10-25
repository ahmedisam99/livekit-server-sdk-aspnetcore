using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LiveKit.Proto;
using LiveKit.Authentication;
using Microsoft.Extensions.Logging;

namespace LiveKit.Services;

public sealed class LiveKitCloudAgentService : TwirpClient, ILiveKitCloudAgentService
{
    private const string ServiceName = "CloudAgent";

    public LiveKitCloudAgentService(HttpClient httpClient, ILogger<LiveKitCloudAgentService> logger, ILiveKitTokenService _tokenService)
        : base(httpClient, logger, ServiceName, _tokenService)
    {
    }

    public async Task<CreateAgentResponse> CreateAgentAsync(CreateAgentRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<CreateAgentResponse>("CreateAgent", null, request, null, cancellationToken);
    }

    public async Task<ListAgentsResponse> ListAgentsAsync(ListAgentsRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ListAgentsResponse>("ListAgents", null, request, null, cancellationToken);
    }

    public async Task<ListAgentVersionsResponse> ListAgentVersionsAsync(ListAgentVersionsRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ListAgentVersionsResponse>("ListAgentVersions", null, request, null, cancellationToken);
    }

    public async Task<ListAgentSecretsResponse> ListAgentSecretsAsync(ListAgentSecretsRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ListAgentSecretsResponse>("ListAgentSecrets", null, request, null, cancellationToken);
    }

    public async Task<UpdateAgentResponse> UpdateAgentAsync(UpdateAgentRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<UpdateAgentResponse>("UpdateAgent", null, request, null, cancellationToken);
    }

    public async Task<RestartAgentResponse> RestartAgentAsync(RestartAgentRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<RestartAgentResponse>("RestartAgent", null, request, null, cancellationToken);
    }

    public async Task<DeployAgentResponse> DeployAgentAsync(DeployAgentRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<DeployAgentResponse>("DeployAgent", null, request, null, cancellationToken);
    }

    public async Task<UpdateAgentSecretsResponse> UpdateAgentSecretsAsync(UpdateAgentSecretsRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<UpdateAgentSecretsResponse>("UpdateAgentSecrets", null, request, null, cancellationToken);
    }

    public async Task<RollbackAgentResponse> RollbackAgentAsync(RollbackAgentRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<RollbackAgentResponse>("RollbackAgent", null, request, null, cancellationToken);
    }

    public async Task<DeleteAgentResponse> DeleteAgentAsync(DeleteAgentRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<DeleteAgentResponse>("DeleteAgent", null, request, null, cancellationToken);
    }

    public async Task<ClientSettingsResponse> GetClientSettingsAsync(ClientSettingsRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ClientSettingsResponse>("GetClientSettings", null, request, null, cancellationToken);
    }
}
