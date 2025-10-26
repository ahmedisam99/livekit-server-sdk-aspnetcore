using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Google.Protobuf;
using LiveKit.Authentication;
using LiveKit.Proto;

namespace LiveKit.Services;

/// <summary>
/// Service for receiving and validating LiveKit webhook events.
/// </summary>
public sealed class LiveKitWebhookReceiver : ILiveKitWebhookReceiver
{
    private readonly ILiveKitTokenVerifier _tokenVerifier;

    /// <summary>
    /// Initializes a new instance of the <see cref="LiveKitWebhookReceiver"/> class.
    /// </summary>
    /// <param name="tokenVerifier">The token verifier service.</param>
    /// <exception cref="ArgumentNullException">Thrown when tokenVerifier is null.</exception>
    public LiveKitWebhookReceiver(ILiveKitTokenVerifier tokenVerifier)
    {
        _tokenVerifier = tokenVerifier ?? throw new ArgumentNullException(nameof(tokenVerifier));
    }

    /// <inheritdoc/>
    public WebhookEvent Receive(string body, string? authorizationHeader = null, bool skipAuth = false)
    {
        if (string.IsNullOrWhiteSpace(body))
        {
            throw new ArgumentException("Request body cannot be null or empty.", nameof(body));
        }

        if (!skipAuth)
        {
            if (string.IsNullOrWhiteSpace(authorizationHeader))
            {
                throw new ArgumentException("Authorization header is required when skipAuth is false.", nameof(authorizationHeader));
            }

            IDictionary<string, string> claims;
            try
            {
                claims = _tokenVerifier.Verify(authorizationHeader!);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Authorization token verification failed.", nameof(authorizationHeader), ex);
            }

            if (claims.TryGetValue(LiveKitClaims.Sha256, out var sha256Claim) && !string.IsNullOrWhiteSpace(sha256Claim))
            {
                using var sha256 = SHA256.Create();
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(body));
                var hashBase64 = Convert.ToBase64String(hashBytes);

                if (sha256Claim != hashBase64)
                {
                    throw new InvalidOperationException(
                        "SHA256 checksum of body does not match the token claim. " +
                        "The webhook request may have been tampered with.");
                }
            }
        }

        try
        {
            var parser = new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));
            return parser.Parse<WebhookEvent>(body);
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Failed to parse webhook event: {ex.Message}", nameof(body), ex);
        }
    }
}
