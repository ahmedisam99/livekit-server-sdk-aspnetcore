using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LiveKit.Proto;
using LiveKit.Authentication;
using Microsoft.Extensions.Logging;

namespace LiveKit.Services;

/// <inheritdoc cref="ILiveKitIngressService" />
public sealed class LiveKitIngressService : TwirpClient, ILiveKitIngressService
{
    private const string ServiceName = "Ingress";

    /// <summary>
    /// Initializes a new instance of the <see cref="LiveKitIngressService"/> class.
    /// </summary>
    public LiveKitIngressService(HttpClient httpClient, ILogger<LiveKitIngressService> logger, ILiveKitTokenService _tokenService)
        : base(httpClient, logger, ServiceName, _tokenService)
    {
    }

    /// <inheritdoc/>
    public async Task<IngressInfo> CreateIngressAsync(CreateIngressRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<IngressInfo>("CreateIngress", request.RoomName, request, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IngressInfo> UpdateIngressAsync(UpdateIngressRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<IngressInfo>("UpdateIngress", request.RoomName, request, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<ListIngressResponse> ListIngressAsync(ListIngressRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ListIngressResponse>("ListIngress", request.RoomName, request, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IngressInfo> DeleteIngressAsync(DeleteIngressRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<IngressInfo>("DeleteIngress", null, request, cancellationToken);
    }
}
