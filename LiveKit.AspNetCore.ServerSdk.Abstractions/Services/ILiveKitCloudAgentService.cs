using System.Threading;
using System.Threading.Tasks;
using LiveKit.Proto;

namespace LiveKit.Services;

/// <summary>
/// Cloud Agent service for managing LiveKit Cloud agents.
/// </summary>
public interface ILiveKitCloudAgentService
{
    /// <summary>
    /// Creates a new agent deployment.
    /// </summary>
    Task<CreateAgentResponse> CreateAgentAsync(CreateAgentRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists agents in the project.
    /// </summary>
    Task<ListAgentsResponse> ListAgentsAsync(ListAgentsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists versions of an agent.
    /// </summary>
    Task<ListAgentVersionsResponse> ListAgentVersionsAsync(ListAgentVersionsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists secrets for an agent.
    /// </summary>
    Task<ListAgentSecretsResponse> ListAgentSecretsAsync(ListAgentSecretsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing agent.
    /// </summary>
    Task<UpdateAgentResponse> UpdateAgentAsync(UpdateAgentRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Restarts an agent deployment.
    /// </summary>
    Task<RestartAgentResponse> RestartAgentAsync(RestartAgentRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deploys a new version of an agent.
    /// </summary>
    Task<DeployAgentResponse> DeployAgentAsync(DeployAgentRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates secrets for an agent.
    /// </summary>
    Task<UpdateAgentSecretsResponse> UpdateAgentSecretsAsync(UpdateAgentSecretsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back an agent to a previous version.
    /// </summary>
    Task<RollbackAgentResponse> RollbackAgentAsync(RollbackAgentRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an agent deployment.
    /// </summary>
    Task<DeleteAgentResponse> DeleteAgentAsync(DeleteAgentRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets client settings.
    /// </summary>
    Task<ClientSettingsResponse> GetClientSettingsAsync(ClientSettingsRequest request, CancellationToken cancellationToken = default);
}

