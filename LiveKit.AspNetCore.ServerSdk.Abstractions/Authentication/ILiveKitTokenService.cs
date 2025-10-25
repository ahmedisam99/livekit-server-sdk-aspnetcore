using System;

namespace LiveKit.Authentication;

/// <summary>
/// Service for creating LiveKit authentication tokens.
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
}
