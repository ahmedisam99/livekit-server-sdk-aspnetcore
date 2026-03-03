using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LiveKit.Proto;
using LiveKit.Authentication;
using Microsoft.Extensions.Logging;

namespace LiveKit.Services;

/// <inheritdoc cref="ILiveKitConnectorService" />
public sealed class LiveKitConnectorService : TwirpClient, ILiveKitConnectorService
{
    private const string ServiceName = "Connector";

    /// <summary>
    /// Initializes a new instance of the <see cref="LiveKitConnectorService"/> class.
    /// </summary>
    public LiveKitConnectorService(HttpClient httpClient, ILogger<LiveKitConnectorService> logger, ILiveKitTokenService _tokenService)
        : base(httpClient, logger, ServiceName, _tokenService)
    {
    }

    /// <inheritdoc/>
    public async Task<DialWhatsAppCallResponse> DialWhatsAppCallAsync(DialWhatsAppCallRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<DialWhatsAppCallResponse>("DialWhatsAppCall", request.RoomName, request, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<DisconnectWhatsAppCallResponse> DisconnectWhatsAppCallAsync(DisconnectWhatsAppCallRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<DisconnectWhatsAppCallResponse>("DisconnectWhatsAppCall", null, request, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<ConnectWhatsAppCallResponse> ConnectWhatsAppCallAsync(ConnectWhatsAppCallRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ConnectWhatsAppCallResponse>("ConnectWhatsAppCall", null, request, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<AcceptWhatsAppCallResponse> AcceptWhatsAppCallAsync(AcceptWhatsAppCallRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<AcceptWhatsAppCallResponse>("AcceptWhatsAppCall", request.RoomName, request, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<ConnectTwilioCallResponse> ConnectTwilioCallAsync(ConnectTwilioCallRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ConnectTwilioCallResponse>("ConnectTwilioCall", request.RoomName, request, cancellationToken);
    }
}
