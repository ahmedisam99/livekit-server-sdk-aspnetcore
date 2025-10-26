using System;
using System.Collections.Generic;

namespace LiveKit.Authentication;

/// <summary>
/// Service for verifying LiveKit JWT tokens.
/// <para>
/// Validates JWT tokens by checking the signature, issuer (API key), and expiration time.
/// Used internally by the webhook receiver to verify incoming webhook requests.
/// </para>
/// </summary>
public interface ILiveKitTokenVerifier
{
    /// <summary>
    /// Verifies a LiveKit JWT token and extracts its claims.
    /// <para>
    /// Validation includes:
    /// - Signature verification using the API secret
    /// - Issuer (API key) validation
    /// - Expiration time check with configurable clock tolerance
    /// </para>
    /// </summary>
    /// <param name="token">The JWT token to verify.</param>
    /// <param name="clockTolerance">Optional clock tolerance for expiration validation. If not specified, uses the configured default (10 seconds).</param>
    /// <returns>A dictionary of claims extracted from the token, with claim names as keys.</returns>
    /// <exception cref="ArgumentException">Thrown when the token is invalid or verification fails.</exception>
    IDictionary<string, string> Verify(string token, TimeSpan? clockTolerance = null);
}

