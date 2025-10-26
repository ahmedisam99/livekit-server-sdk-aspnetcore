using System.Threading;
using System.Threading.Tasks;
using LiveKit.Proto;

namespace LiveKit.Services;

/// <summary>
/// Service for managing agent dispatches to rooms.
/// <para>
/// Provides methods for dispatching AI agents to rooms based on room configuration or on-demand.
/// See <see href="https://docs.livekit.io/home/get-started/authentication/#room-configuration">Agent Dispatch Documentation</see> for more information.
/// </para>
/// </summary>
public interface ILiveKitAgentDispatchService
{
    /// <summary>
    /// Creates a new agent dispatch for a room.
    /// </summary>
    Task<AgentDispatch> CreateDispatchAsync(CreateAgentDispatchRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an agent dispatch.
    /// </summary>
    Task<AgentDispatch> DeleteDispatchAsync(DeleteAgentDispatchRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists agent dispatches for a room.
    /// </summary>
    Task<ListAgentDispatchResponse> ListDispatchAsync(ListAgentDispatchRequest request, CancellationToken cancellationToken = default);
}

