using System.Threading;
using System.Threading.Tasks;
using LiveKit.Proto;

namespace LiveKit.Services;

/// <summary>
/// Service for managing WhatsApp and Twilio call connectors.
/// <para>
/// Provides methods for dialing, connecting, accepting, and disconnecting WhatsApp and Twilio calls.
/// </para>
/// </summary>
public interface ILiveKitConnectorService
{
    /// <summary>
    /// Dials an outbound WhatsApp call.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<DialWhatsAppCallResponse> DialWhatsAppCallAsync(DialWhatsAppCallRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Disconnects an active WhatsApp call.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<DisconnectWhatsAppCallResponse> DisconnectWhatsAppCallAsync(DisconnectWhatsAppCallRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Connects to a WhatsApp call.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<ConnectWhatsAppCallResponse> ConnectWhatsAppCallAsync(ConnectWhatsAppCallRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Accepts an incoming WhatsApp call.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<AcceptWhatsAppCallResponse> AcceptWhatsAppCallAsync(AcceptWhatsAppCallRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Connects to a Twilio call.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<ConnectTwilioCallResponse> ConnectTwilioCallAsync(ConnectTwilioCallRequest request, CancellationToken cancellationToken = default);
}
