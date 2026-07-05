using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LiveKit.Proto;
using LiveKit.Authentication;
using Microsoft.Extensions.Logging;

namespace LiveKit.Services;

/// <inheritdoc cref="ILiveKitCloudAgentService" />
public sealed class LiveKitCloudAgentService : TwirpClient, ILiveKitCloudAgentService
{
    private const string ServiceName = "CloudAgent";

    /// <summary>
    /// Initializes a new instance of the <see cref="LiveKitCloudAgentService"/> class.
    /// </summary>
    public LiveKitCloudAgentService(HttpClient httpClient, ILogger<LiveKitCloudAgentService> logger, ILiveKitTokenService _tokenService)
        : base(httpClient, logger, ServiceName, _tokenService)
    {
    }

    /// <inheritdoc/>
    public async Task<CreateAgentResponse> CreateAgentAsync(CreateAgentRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<CreateAgentResponse>("CreateAgent", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<CreateAgentV2Response> CreateAgentV2Async(CreateAgentV2Request request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<CreateAgentV2Response>("CreateAgentV2", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<ListAgentsResponse> ListAgentsAsync(ListAgentsRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ListAgentsResponse>("ListAgents", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<ListAgentVersionsResponse> ListAgentVersionsAsync(ListAgentVersionsRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ListAgentVersionsResponse>("ListAgentVersions", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<ListAgentSecretsResponse> ListAgentSecretsAsync(ListAgentSecretsRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ListAgentSecretsResponse>("ListAgentSecrets", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<UpdateAgentResponse> UpdateAgentAsync(UpdateAgentRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<UpdateAgentResponse>("UpdateAgent", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<RestartAgentResponse> RestartAgentAsync(RestartAgentRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<RestartAgentResponse>("RestartAgent", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<PromoteAgentResponse> PromoteAgentAsync(PromoteAgentRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<PromoteAgentResponse>("PromoteAgent", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<DeployAgentResponse> DeployAgentAsync(DeployAgentRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<DeployAgentResponse>("DeployAgent", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<DeployAgentV2Response> DeployAgentV2Async(DeployAgentV2Request request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<DeployAgentV2Response>("DeployAgentV2", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<UpdateAgentSecretsResponse> UpdateAgentSecretsAsync(UpdateAgentSecretsRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<UpdateAgentSecretsResponse>("UpdateAgentSecrets", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<RollbackAgentResponse> RollbackAgentAsync(RollbackAgentRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<RollbackAgentResponse>("RollbackAgent", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<DeleteAgentResponse> DeleteAgentAsync(DeleteAgentRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<DeleteAgentResponse>("DeleteAgent", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<ClientSettingsResponse> GetClientSettingsAsync(ClientSettingsRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ClientSettingsResponse>("GetClientSettings", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<CreatePrivateLinkResponse> CreatePrivateLinkAsync(CreatePrivateLinkRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<CreatePrivateLinkResponse>("CreatePrivateLink", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<DestroyPrivateLinkResponse> DestroyPrivateLinkAsync(DestroyPrivateLinkRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<DestroyPrivateLinkResponse>("DestroyPrivateLink", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<ListPrivateLinksResponse> ListPrivateLinksAsync(ListPrivateLinksRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ListPrivateLinksResponse>("ListPrivateLinks", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<GetPrivateLinkStatusResponse> GetPrivateLinkStatusAsync(GetPrivateLinkStatusRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<GetPrivateLinkStatusResponse>("GetPrivateLinkStatus", null, request, cancellationToken).ConfigureAwait(false);
    }
}
