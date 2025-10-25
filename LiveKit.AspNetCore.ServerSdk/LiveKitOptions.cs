namespace LiveKit;

/// <summary>
/// Configuration options for LiveKit services.
/// </summary>
public sealed class LiveKitOptions
{
    /// <summary>
    /// Gets or sets the base URL for the LiveKit server.
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the API key for authentication.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the API secret for authentication.
    /// </summary>
    public string ApiSecret { get; set; } = string.Empty;
}
