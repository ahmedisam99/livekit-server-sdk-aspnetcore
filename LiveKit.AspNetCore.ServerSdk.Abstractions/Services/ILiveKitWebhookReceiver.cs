using System;
using LiveKit.Proto;

namespace LiveKit.Services;

/// <summary>
/// Service for receiving and validating LiveKit webhook events.
/// <para>
/// LiveKit sends webhook events to notify your server when room events occur, such as room started/finished,
/// participant joined/left, track published/unpublished, and egress/ingress events.
/// </para>
/// <para>
/// The webhook receiver validates that requests came from LiveKit by verifying the JWT token signature
/// and comparing the SHA256 hash of the request body.
/// </para>
/// <para>
/// See <see href="https://docs.livekit.io/home/server/webhooks/">LiveKit Webhooks Documentation</see> for more information.
/// </para>
/// </summary>
public interface ILiveKitWebhookReceiver
{
    /// <summary>
    /// Receives and validates a webhook event from LiveKit.
    /// <para>
    /// This method performs two-layer security validation:
    /// 1. JWT Token Verification - Validates the token signature and expiration
    /// 2. SHA256 Checksum - Verifies the request body hasn't been tampered with
    /// </para>
    /// </summary>
    /// <param name="body">The raw JSON body of the webhook request.</param>
    /// <param name="authorizationHeader">The Authorization header value containing the JWT token.</param>
    /// <param name="skipAuth">If true, skips authentication and signature validation. Use only for testing.</param>
    /// <returns>The parsed webhook event containing event type, room, participant, and other event-specific data.</returns>
    /// <exception cref="ArgumentException">Thrown when validation fails or the body cannot be parsed.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the SHA256 checksum validation fails.</exception>
    WebhookEvent Receive(string body, string? authorizationHeader = null, bool skipAuth = false);
}

