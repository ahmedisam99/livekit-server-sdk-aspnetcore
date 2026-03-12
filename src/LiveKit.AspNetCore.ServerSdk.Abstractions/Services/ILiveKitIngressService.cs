using System.Threading;
using System.Threading.Tasks;
using LiveKit.Proto;

namespace LiveKit.Services;

/// <summary>
/// Service for managing media ingress (RTMP, WHIP, URL pull).
/// <para>
/// Provides methods for creating and managing ingress endpoints for bringing external media into LiveKit rooms.
/// See <see href="https://docs.livekit.io/home/ingress/overview/#api">LiveKit Ingress API Documentation</see> for more information.
/// </para>
/// </summary>
public interface ILiveKitIngressService
{
    /// <summary>
    /// Creates a new ingress endpoint.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<IngressInfo> CreateIngressAsync(CreateIngressRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing ingress. Ingress can only be updated when in ENDPOINT_WAITING state.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<IngressInfo> UpdateIngressAsync(UpdateIngressRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists ingress endpoints.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<ListIngressResponse> ListIngressAsync(ListIngressRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an ingress endpoint.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<IngressInfo> DeleteIngressAsync(DeleteIngressRequest request, CancellationToken cancellationToken = default);
}
