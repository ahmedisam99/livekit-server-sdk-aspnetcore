using System;
using System.Threading;
using System.Threading.Tasks;
using LiveKit.Proto;

namespace LiveKit.Services;

/// <summary>
/// Service for managing SIP trunks, dispatch rules, and participants.
/// <para>
/// Provides methods for SIP integration, including managing trunks, dispatch rules, and making SIP calls.
/// See <see href="https://docs.livekit.io/reference/server/server-apis/#sip-service">LiveKit SIP Service Documentation</see> for more information.
/// </para>
/// </summary>
public interface ILiveKitSipService
{
    /// <summary>
    /// Lists SIP trunks (deprecated, use ListSIPInboundTrunk or ListSIPOutboundTrunk).
    /// </summary>
    [Obsolete("Use ListSIPInboundTrunk or ListSIPOutboundTrunk instead")]
    Task<ListSIPTrunkResponse> ListSIPTrunkAsync(ListSIPTrunkRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new SIP inbound trunk.
    /// </summary>
    Task<SIPInboundTrunkInfo> CreateSIPInboundTrunkAsync(CreateSIPInboundTrunkRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new SIP outbound trunk.
    /// </summary>
    Task<SIPOutboundTrunkInfo> CreateSIPOutboundTrunkAsync(CreateSIPOutboundTrunkRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing SIP inbound trunk.
    /// </summary>
    Task<SIPInboundTrunkInfo> UpdateSIPInboundTrunkAsync(UpdateSIPInboundTrunkRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing SIP outbound trunk.
    /// </summary>
    Task<SIPOutboundTrunkInfo> UpdateSIPOutboundTrunkAsync(UpdateSIPOutboundTrunkRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a SIP inbound trunk by ID.
    /// </summary>
    Task<GetSIPInboundTrunkResponse> GetSIPInboundTrunkAsync(GetSIPInboundTrunkRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a SIP outbound trunk by ID.
    /// </summary>
    Task<GetSIPOutboundTrunkResponse> GetSIPOutboundTrunkAsync(GetSIPOutboundTrunkRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists SIP inbound trunks.
    /// </summary>
    Task<ListSIPInboundTrunkResponse> ListSIPInboundTrunkAsync(ListSIPInboundTrunkRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists SIP outbound trunks.
    /// </summary>
    Task<ListSIPOutboundTrunkResponse> ListSIPOutboundTrunkAsync(ListSIPOutboundTrunkRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a SIP trunk by ID.
    /// </summary>
#pragma warning disable CS0612 // Type or member is obsolete
    Task<SIPTrunkInfo> DeleteSIPTrunkAsync(DeleteSIPTrunkRequest request, CancellationToken cancellationToken = default);
#pragma warning restore CS0612 // Type or member is obsolete

    /// <summary>
    /// Creates a new SIP dispatch rule.
    /// </summary>
    Task<SIPDispatchRuleInfo> CreateSIPDispatchRuleAsync(CreateSIPDispatchRuleRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing SIP dispatch rule.
    /// </summary>
    Task<SIPDispatchRuleInfo> UpdateSIPDispatchRuleAsync(UpdateSIPDispatchRuleRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists SIP dispatch rules.
    /// </summary>
    Task<ListSIPDispatchRuleResponse> ListSIPDispatchRuleAsync(ListSIPDispatchRuleRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a SIP dispatch rule by ID.
    /// </summary>
    Task<SIPDispatchRuleInfo> DeleteSIPDispatchRuleAsync(DeleteSIPDispatchRuleRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a SIP participant to make an outbound call.
    /// </summary>
    Task<SIPParticipantInfo> CreateSIPParticipantAsync(CreateSIPParticipantRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Transfers an active SIP participant to another number.
    /// </summary>
    Task TransferSIPParticipantAsync(TransferSIPParticipantRequest request, CancellationToken cancellationToken = default);
}
