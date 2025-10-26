using System;
using LiveKit.Proto;

namespace LiveKit.Services;

/// <summary>
/// Service for receiving and validating LiveKit webhook events.
/// </summary>
public interface ILiveKitWebhookReceiver
{
    /// <summary>
    /// Receives and validates a webhook event from LiveKit.
    /// </summary>
    /// <param name="body">The raw JSON body of the webhook request.</param>
    /// <param name="authorizationHeader">The Authorization header value containing the JWT token.</param>
    /// <param name="skipAuth">If true, skips authentication and signature validation. Use only for testing.</param>
    /// <returns>The parsed webhook event.</returns>
    /// <exception cref="ArgumentException">Thrown when validation fails or the body cannot be parsed.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the SHA256 checksum validation fails.</exception>
    WebhookEvent Receive(string body, string? authorizationHeader = null, bool skipAuth = false);
}

