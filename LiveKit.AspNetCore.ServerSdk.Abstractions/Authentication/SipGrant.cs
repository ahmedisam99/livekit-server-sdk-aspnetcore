namespace LiveKit.Authentication;

/// <summary>
/// SIP grant containing permissions for SIP service interactions.
/// In order to interact with the SIP service, permission must be granted in the sip field of the JWT.
/// </summary>
public sealed class SipGrant
{
    /// <summary>
    /// Permission to manage SIP trunks and dispatch rules.
    /// </summary>
    public bool? Admin { get; set; }

    /// <summary>
    /// Permission to make SIP calls via CreateSIPParticipant.
    /// </summary>
    public bool? Call { get; set; }
}
