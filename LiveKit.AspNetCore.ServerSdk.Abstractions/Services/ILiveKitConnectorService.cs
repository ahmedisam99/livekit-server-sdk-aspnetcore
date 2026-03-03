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
    Task<DialWhatsAppCallResponse> DialWhatsAppCallAsync(DialWhatsAppCallRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Disconnects an active WhatsApp call.
    /// </summary>
    Task<DisconnectWhatsAppCallResponse> DisconnectWhatsAppCallAsync(DisconnectWhatsAppCallRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Connects to a WhatsApp call.
    /// </summary>
    Task<ConnectWhatsAppCallResponse> ConnectWhatsAppCallAsync(ConnectWhatsAppCallRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Accepts an incoming WhatsApp call.
    /// </summary>
    Task<AcceptWhatsAppCallResponse> AcceptWhatsAppCallAsync(AcceptWhatsAppCallRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Connects to a Twilio call.
    /// </summary>
    Task<ConnectTwilioCallResponse> ConnectTwilioCallAsync(ConnectTwilioCallRequest request, CancellationToken cancellationToken = default);
}
