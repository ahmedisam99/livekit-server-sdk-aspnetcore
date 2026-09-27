using System.Threading;
using System.Threading.Tasks;
using LiveKit.Proto;

namespace LiveKit.Services;

/// <summary>
/// Service for managing agent databases.
/// <para>
/// Provides methods for creating, inspecting, deleting, and exporting the SQLite databases
/// available to agents. Queries run over a separate WebSocket data plane that this service does not cover.
/// </para>
/// </summary>
public interface ILiveKitAgentDBService
{
    /// <summary>
    /// Creates a new agent database.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<AgentDB.Types.CreateResponse> CreateDatabaseAsync(AgentDB.Types.CreateRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets details of an agent database.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<AgentDB.Types.AgentDatabase> GetDatabaseAsync(AgentDB.Types.GetRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists agent databases.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<AgentDB.Types.ListResponse> ListDatabasesAsync(AgentDB.Types.ListRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an agent database, including its stored data.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<AgentDB.Types.DeleteResponse> DeleteDatabaseAsync(AgentDB.Types.DeleteRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Exports a consistent SQLite file of an agent database as a time-limited download URL.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<AgentDB.Types.DumpResponse> DumpDatabaseAsync(AgentDB.Types.DumpRequest request, CancellationToken cancellationToken = default);
}
