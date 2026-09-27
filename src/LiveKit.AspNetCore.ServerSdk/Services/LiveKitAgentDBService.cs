using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LiveKit.Proto;
using LiveKit.Authentication;
using Microsoft.Extensions.Logging;

namespace LiveKit.Services;

/// <inheritdoc cref="ILiveKitAgentDBService" />
public sealed class LiveKitAgentDBService : TwirpClient, ILiveKitAgentDBService
{
    private const string ServiceName = "AgentDBService";

    /// <summary>
    /// Initializes a new instance of the <see cref="LiveKitAgentDBService"/> class.
    /// </summary>
    public LiveKitAgentDBService(HttpClient httpClient, ILogger<LiveKitAgentDBService> logger, ILiveKitTokenService _tokenService)
        : base(httpClient, logger, ServiceName, _tokenService)
    {
    }

    /// <inheritdoc/>
    public async Task<AgentDB.Types.CreateResponse> CreateDatabaseAsync(AgentDB.Types.CreateRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<AgentDB.Types.CreateResponse>("CreateDatabase", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<AgentDB.Types.AgentDatabase> GetDatabaseAsync(AgentDB.Types.GetRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<AgentDB.Types.AgentDatabase>("GetDatabase", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<AgentDB.Types.ListResponse> ListDatabasesAsync(AgentDB.Types.ListRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<AgentDB.Types.ListResponse>("ListDatabases", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<AgentDB.Types.DeleteResponse> DeleteDatabaseAsync(AgentDB.Types.DeleteRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<AgentDB.Types.DeleteResponse>("DeleteDatabase", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<AgentDB.Types.DumpResponse> DumpDatabaseAsync(AgentDB.Types.DumpRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<AgentDB.Types.DumpResponse>("DumpDatabase", null, request, cancellationToken).ConfigureAwait(false);
    }
}
