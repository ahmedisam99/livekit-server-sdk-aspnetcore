using System;

namespace LiveKit;

/// <summary>
/// Configuration options for LiveKit services.
/// <para>
/// Contains settings for API authentication, server URL, and webhook validation.
/// See <see href="https://docs.livekit.io/home/get-started/">LiveKit Documentation</see> for setup instructions.
/// </para>
/// </summary>
public sealed class LiveKitOptions
{
    /// <summary>
    /// Gets or sets the base URL for the LiveKit server.
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL for the Cloud Agent service.
    /// When not set, it is derived from <see cref="BaseUrl"/> by replacing its first subdomain with <c>agents</c>,
    /// e.g. <c>https://my-project.livekit.cloud</c> becomes <c>https://agents.livekit.cloud</c>.
    /// </summary>
    public string? AgentsUrl { get; set; }

    /// <summary>
    /// Gets or sets the API key for authentication.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the API secret for authentication.
    /// </summary>
    public string ApiSecret { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the clock tolerance for webhook JWT token validation.
    /// Defaults to 10 seconds to account for clock skew between servers.
    /// </summary>
    public TimeSpan WebhookClockTolerance { get; set; } = TimeSpan.FromSeconds(10);
}
