using System;

namespace LiveKit.Authentication;

/// <summary>
/// Service for creating LiveKit authentication tokens.
/// <para>
/// Provides methods for generating JWT tokens for participant authentication and server API access.
/// See <see href="https://docs.livekit.io/home/get-started/authentication/">LiveKit Authentication Documentation</see> for more information.
/// </para>
/// </summary>
public interface ILiveKitTokenService
{
    /// <summary>
    /// Creates a token builder for the specified identity.
    /// </summary>
    ILiveKitTokenBuilder CreateTokenBuilder(string identity);

    /// <summary>
    /// Creates a server token for API calls.
    /// </summary>
    string CreateServerToken(string? roomName = null, TimeSpan? ttl = null);

    /// <summary>
    /// Creates a server token for API calls that forward or move a participant to another room.
    /// </summary>
    /// <param name="roomName">The room the token grants access to, if any.</param>
    /// <param name="ttl">The token time-to-live. Defaults to 5 minutes.</param>
    /// <param name="destinationRoom">The room a participant can be forwarded or moved to, if any.</param>
    string CreateServerToken(string? roomName, TimeSpan? ttl, string? destinationRoom);
}
