using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace LiveKit;

/// <summary>
/// Exception thrown when a LiveKit Twirp API request fails.
/// <para>
/// Provides structured access to the Twirp error code, message, and HTTP status code
/// returned by the server. Extends <see cref="HttpRequestException"/> so existing
/// catch blocks continue to work.
/// </para>
/// </summary>
public class LiveKitApiException : HttpRequestException
{
    /// <summary>
    /// The Twirp error code returned by the server.
    /// </summary>
    public TwirpErrorCode Code { get; }

    /// <summary>
    /// The error message from the Twirp response's <c>msg</c> field.
    /// </summary>
    public string TwirpMessage { get; }

    /// <summary>
    /// The HTTP status code of the response.
    /// </summary>
    public HttpStatusCode StatusCode { get; }

    /// <summary>
    /// The error metadata from the Twirp response's <c>meta</c> field, such as <c>sip_status_code</c>
    /// for SIP failures. Empty when the response has none.
    /// </summary>
    public IReadOnlyDictionary<string, string> Meta { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LiveKitApiException"/> class.
    /// </summary>
    /// <param name="message">The full diagnostic message for logging.</param>
    /// <param name="code">The parsed Twirp error code.</param>
    /// <param name="twirpMessage">The error message from the Twirp response.</param>
    /// <param name="statusCode">The HTTP status code of the response.</param>
    public LiveKitApiException(string message, TwirpErrorCode code, string twirpMessage, HttpStatusCode statusCode)
        : this(message, code, twirpMessage, statusCode, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LiveKitApiException"/> class.
    /// </summary>
    /// <param name="message">The full diagnostic message for logging.</param>
    /// <param name="code">The parsed Twirp error code.</param>
    /// <param name="twirpMessage">The error message from the Twirp response.</param>
    /// <param name="statusCode">The HTTP status code of the response.</param>
    /// <param name="meta">The error metadata from the Twirp response.</param>
    public LiveKitApiException(string message, TwirpErrorCode code, string twirpMessage, HttpStatusCode statusCode,
        IReadOnlyDictionary<string, string>? meta)
        : base(message)
    {
        Code = code;
        TwirpMessage = twirpMessage;
        StatusCode = statusCode;
        Meta = meta ?? new Dictionary<string, string>();
    }
}
