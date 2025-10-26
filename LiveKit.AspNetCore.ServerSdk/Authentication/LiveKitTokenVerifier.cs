using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LiveKit.Authentication;

/// <summary>
/// Service for verifying LiveKit JWT tokens.
/// </summary>
public sealed class LiveKitTokenVerifier : ILiveKitTokenVerifier
{
    private readonly LiveKitOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="LiveKitTokenVerifier"/> class.
    /// </summary>
    /// <param name="options">The LiveKit configuration options.</param>
    /// <exception cref="ArgumentNullException">Thrown when options is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when API key or secret is not configured.</exception>
    public LiveKitTokenVerifier(IOptions<LiveKitOptions> options)
    {
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));

        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            throw new InvalidOperationException("LiveKit API key is required. Configure it using AddLiveKit().");
        }

        if (string.IsNullOrWhiteSpace(_options.ApiSecret))
        {
            throw new InvalidOperationException("LiveKit API secret is required. Configure it using AddLiveKit().");
        }
    }

    /// <inheritdoc/>
    public IDictionary<string, string> Verify(string token, TimeSpan? clockTolerance = null)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException("Token cannot be null or empty.", nameof(token));
        }

        var tolerance = clockTolerance ?? _options.WebhookClockTolerance;

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _options.ApiKey,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.ApiSecret)),
            ValidateLifetime = true,
            ClockSkew = tolerance
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal.Claims.ToDictionary(c => c.Type, c => c.Value);
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Token verification failed: {ex.Message}", nameof(token), ex);
        }
    }
}
