using Google.Protobuf;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using LiveKit.Authentication;
using Microsoft.Extensions.Logging;

namespace LiveKit.Services;

/// <summary>
/// Base class for LiveKit Twirp client services.
/// <para>
/// Provides a common implementation for making authenticated HTTP requests to LiveKit's Twirp-based services.
/// This class handles JSON serialization of protobuf messages and authentication token management.
/// </para>
/// </summary>
public abstract class TwirpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;
    private readonly string _serviceName;
    private readonly ILiveKitTokenService _tokenService;

    private readonly JsonFormatter _jsonFormatter;
    private readonly JsonParser _jsonParser;

    /// <summary>
    /// Initializes a new instance of the <see cref="TwirpClient"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for making requests.</param>
    /// <param name="logger">The logger for diagnostic messages.</param>
    /// <param name="serviceName">The name of the Twirp service.</param>
    /// <param name="tokenService">The LiveKit token service for authentication.</param>
    protected TwirpClient(HttpClient httpClient, ILogger logger, string serviceName, ILiveKitTokenService tokenService)
    {
        _httpClient = httpClient;
        _logger = logger;
        _serviceName = serviceName;
        _tokenService = tokenService;

        _jsonFormatter = new JsonFormatter(JsonFormatter.Settings.Default);
        _jsonParser = new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));
    }

    /// <summary>
    /// Makes an authenticated HTTP request to a LiveKit Twirp service.
    /// </summary>
    /// <typeparam name="TResponse">The protobuf message type for the response.</typeparam>
    /// <param name="methodName">The name of the Twirp method to call.</param>
    /// <param name="roomName">The name of the room (optional, used to create server token).</param>
    /// <param name="requestBody">The request body as a protobuf message.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The response as a protobuf message.</returns>
    /// <exception cref="LiveKitApiException">
    /// Thrown when the server returns a non-success HTTP status code.
    /// Contains the parsed Twirp error code, message, and HTTP status code.
    /// </exception>
    protected async Task<TResponse> MakeRequestAsync<TResponse>(
        string methodName,
        string? roomName,
        IMessage? requestBody,
        CancellationToken cancellationToken = default)
        where TResponse : IMessage<TResponse>, new()
    {
        var authToken = _tokenService.CreateServerToken(roomName);
        var url = $"/twirp/livekit.{_serviceName}/{methodName}";

        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        if (requestBody != null)
        {
            var json = _jsonFormatter.Format(requestBody);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }
        else
        {
            request.Content = new StringContent("{}", Encoding.UTF8, "application/json");
        }

        _logger.LogDebug("Making request to {Url}", url);

        var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Request to '{Url}' failed with status {StatusCode}: {Content}", url, response.StatusCode, responseContent);

            var (twirpCode, twirpMessage) = ParseTwirpError(responseContent);
            var msg = $"Request to '{url}' failed with status {response.StatusCode}: {twirpMessage}";

            throw new LiveKitApiException(msg, twirpCode, twirpMessage, response.StatusCode);
        }

        _logger.LogDebug("Response received: {Content}", responseContent);

        if (string.IsNullOrWhiteSpace(responseContent) || responseContent == "{}")
        {
            return new TResponse();
        }

        return _jsonParser.Parse<TResponse>(responseContent);
    }

    private static (TwirpErrorCode code, string message) ParseTwirpError(string responseContent)
    {
        try
        {
            using var doc = JsonDocument.Parse(responseContent);
            var root = doc.RootElement;

            if (root.TryGetProperty("code", out var codeElement) && root.TryGetProperty("msg", out var msgElement))
            {
                var code = ParseTwirpErrorCode(codeElement.GetString());
                var msg = msgElement.GetString() ?? string.Empty;
                return (code, msg);
            }
        }
        catch (JsonException)
        {
        }

        return (TwirpErrorCode.Unknown, responseContent);
    }

    private static TwirpErrorCode ParseTwirpErrorCode(string? code) => code switch
    {
        "canceled" => TwirpErrorCode.Canceled,
        "unknown" => TwirpErrorCode.Unknown,
        "invalid_argument" => TwirpErrorCode.InvalidArgument,
        "malformed" => TwirpErrorCode.Malformed,
        "deadline_exceeded" => TwirpErrorCode.DeadlineExceeded,
        "not_found" => TwirpErrorCode.NotFound,
        "bad_route" => TwirpErrorCode.BadRoute,
        "already_exists" => TwirpErrorCode.AlreadyExists,
        "permission_denied" => TwirpErrorCode.PermissionDenied,
        "unauthenticated" => TwirpErrorCode.Unauthenticated,
        "resource_exhausted" => TwirpErrorCode.ResourceExhausted,
        "failed_precondition" => TwirpErrorCode.FailedPrecondition,
        "aborted" => TwirpErrorCode.Aborted,
        "out_of_range" => TwirpErrorCode.OutOfRange,
        "unimplemented" => TwirpErrorCode.Unimplemented,
        "internal" => TwirpErrorCode.Internal,
        "unavailable" => TwirpErrorCode.Unavailable,
        "dataloss" => TwirpErrorCode.DataLoss,
        _ => TwirpErrorCode.Unknown
    };
}
