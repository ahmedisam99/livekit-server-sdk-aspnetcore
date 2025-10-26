using System;
using System.Collections.Generic;

namespace LiveKit.Authentication;

/// <summary>
/// Service for verifying LiveKit JWT tokens.
/// </summary>
public interface ILiveKitTokenVerifier
{
    /// <summary>
    /// Verifies a LiveKit JWT token and extracts its claims.
    /// </summary>
    /// <param name="token">The JWT token to verify.</param>
    /// <param name="clockTolerance">Optional clock tolerance for expiration validation. If not specified, uses the configured default.</param>
    /// <returns>A dictionary of claims extracted from the token, with claim names as keys.</returns>
    /// <exception cref="ArgumentException">Thrown when the token is invalid or verification fails.</exception>
    IDictionary<string, string> Verify(string token, TimeSpan? clockTolerance = null);
}

