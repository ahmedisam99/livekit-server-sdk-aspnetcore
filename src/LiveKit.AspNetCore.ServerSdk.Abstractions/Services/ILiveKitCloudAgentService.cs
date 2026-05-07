using System.Threading;
using System.Threading.Tasks;
using LiveKit.Proto;

namespace LiveKit.Services;

/// <summary>
/// Service for managing LiveKit Cloud agents.
/// <para>
/// Provides methods for deploying, managing, and monitoring AI agent deployments on LiveKit Cloud.
/// See <see href="https://docs.livekit.io/reference/server/server-apis/#cloud-agent-service">Cloud Agent Service Documentation</see> for more information.
/// </para>
/// </summary>
public interface ILiveKitCloudAgentService
{
    /// <summary>
    /// Creates a new agent deployment.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<CreateAgentResponse> CreateAgentAsync(CreateAgentRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new agent deployment (V2 API).
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<CreateAgentV2Response> CreateAgentV2Async(CreateAgentV2Request request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists agents in the project.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<ListAgentsResponse> ListAgentsAsync(ListAgentsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists versions of an agent.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<ListAgentVersionsResponse> ListAgentVersionsAsync(ListAgentVersionsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists secrets for an agent.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<ListAgentSecretsResponse> ListAgentSecretsAsync(ListAgentSecretsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing agent.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<UpdateAgentResponse> UpdateAgentAsync(UpdateAgentRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Restarts an agent deployment.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<RestartAgentResponse> RestartAgentAsync(RestartAgentRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deploys a new version of an agent.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<DeployAgentResponse> DeployAgentAsync(DeployAgentRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deploys a new version of an agent (V2 API).
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<DeployAgentV2Response> DeployAgentV2Async(DeployAgentV2Request request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates secrets for an agent.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<UpdateAgentSecretsResponse> UpdateAgentSecretsAsync(UpdateAgentSecretsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back an agent to a previous version.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<RollbackAgentResponse> RollbackAgentAsync(RollbackAgentRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an agent deployment.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<DeleteAgentResponse> DeleteAgentAsync(DeleteAgentRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets client settings.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<ClientSettingsResponse> GetClientSettingsAsync(ClientSettingsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a private link for an agent.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<CreatePrivateLinkResponse> CreatePrivateLinkAsync(CreatePrivateLinkRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Destroys a private link.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<DestroyPrivateLinkResponse> DestroyPrivateLinkAsync(DestroyPrivateLinkRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists private links.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<ListPrivateLinksResponse> ListPrivateLinksAsync(ListPrivateLinksRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the status of a private link.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<GetPrivateLinkStatusResponse> GetPrivateLinkStatusAsync(GetPrivateLinkStatusRequest request, CancellationToken cancellationToken = default);
}
