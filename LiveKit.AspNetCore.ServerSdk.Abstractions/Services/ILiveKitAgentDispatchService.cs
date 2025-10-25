using System.Threading;
using System.Threading.Tasks;
using LiveKit.Proto;

namespace LiveKit.Services;

/// <summary>
/// Service for managing agent dispatches to rooms.
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

