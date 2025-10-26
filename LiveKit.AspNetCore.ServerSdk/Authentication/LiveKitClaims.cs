using System.IdentityModel.Tokens.Jwt;

namespace LiveKit.Authentication;

/// <summary>
/// Claim names used in LiveKit authentication tokens.
/// </summary>
public static class LiveKitClaims
{
    /// <summary>
    /// The identity claim (JWT standard "sub" claim).
    /// </summary>
    public const string Identity = JwtRegisteredClaimNames.Sub;

    /// <summary>
    /// The participant name claim.
    /// </summary>
    public const string Name = "name";

    /// <summary>
    /// The metadata claim.
    /// </summary>
    public const string Metadata = "metadata";

    /// <summary>
    /// The participant kind claim.
    /// </summary>
    public const string Kind = "kind";

    /// <summary>
    /// The attributes claim.
    /// </summary>
    public const string Attributes = "attributes";

    /// <summary>
    /// The video grant claim.
    /// </summary>
    public const string Video = "video";

    /// <summary>
    /// The SIP grant claim.
    /// </summary>
    public const string Sip = "sip";

    /// <summary>
    /// The room configuration claim.
    /// </summary>
    public const string RoomConfig = "roomConfig";

    /// <summary>
    /// The room preset claim.
    /// </summary>
    public const string RoomPreset = "roomPreset";

    /// <summary>
    /// The SHA256 hash claim (used for webhook validation).
    /// </summary>
    public const string Sha256 = "sha256";
}
