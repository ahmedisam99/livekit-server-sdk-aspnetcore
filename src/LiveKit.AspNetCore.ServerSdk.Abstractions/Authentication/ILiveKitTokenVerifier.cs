using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
    /// <param name="clockTolerance">Optional clock tolerance for expiration validation. If not specified, uses the configured webhook clock tolerance.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A dictionary of claims extracted from the token, with claim names as keys.</returns>
    /// <exception cref="ArgumentException">Thrown when the token is null or empty.</exception>
    /// <exception cref="Exception">Thrown when token validation fails (e.g. SecurityTokenExpiredException, SecurityTokenInvalidSignatureException).</exception>
    Task<IDictionary<string, string>> VerifyAsync(string token, TimeSpan? clockTolerance = null, CancellationToken cancellationToken = default);
}

