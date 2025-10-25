using System.Threading;
using System.Threading.Tasks;
using LiveKit.Proto;

namespace LiveKit.Services;

/// <summary>
/// Ingress service for managing media ingress (RTMP, WHIP, URL pull).
/// </summary>
public interface ILiveKitIngressService
{
    /// <summary>
    /// Creates a new ingress endpoint.
    /// </summary>
    Task<IngressInfo> CreateIngressAsync(CreateIngressRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing ingress. Ingress can only be updated when in ENDPOINT_WAITING state.
    /// </summary>
    Task<IngressInfo> UpdateIngressAsync(UpdateIngressRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists ingress endpoints.
    /// </summary>
    Task<ListIngressResponse> ListIngressAsync(ListIngressRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an ingress endpoint.
    /// </summary>
    Task<IngressInfo> DeleteIngressAsync(DeleteIngressRequest request, CancellationToken cancellationToken = default);
}

