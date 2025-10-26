using Google.Protobuf;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LiveKit.Authentication;
using Microsoft.Extensions.Logging;

namespace LiveKit.Services;

public abstract class TwirpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;
    private readonly string _serviceName;
    private readonly ILiveKitTokenService _tokenService;

    private readonly JsonFormatter _jsonFormatter;
    private readonly JsonParser _jsonParser;

    protected TwirpClient(HttpClient httpClient, ILogger logger, string serviceName, ILiveKitTokenService tokenService)
    {
        _httpClient = httpClient;
        _logger = logger;
        _serviceName = serviceName;
        _tokenService = tokenService;

        _jsonFormatter = new JsonFormatter(JsonFormatter.Settings.Default);
        _jsonParser = new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));
    }

    protected async Task<TResponse> MakeRequestAsync<TResponse>(
        string methodName,
        string? roomName,
        IMessage? requestBody,
        string? token,
        CancellationToken cancellationToken = default)
        where TResponse : IMessage<TResponse>, new()
    {
        var authToken = token ?? _tokenService.CreateServerToken();
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

        var response = await _httpClient.SendAsync(request, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Request to '{Url}' failed with status {StatusCode}: {Content}", url, response.StatusCode, responseContent);

            throw new HttpRequestException($"Request to '{url}' failed with status {response.StatusCode}: {responseContent}");
        }

        _logger.LogDebug("Response received: {Content}", responseContent);

        if (string.IsNullOrEmpty(responseContent) || responseContent == "{}")
        {
            return new TResponse();
        }

        return _jsonParser.Parse<TResponse>(responseContent);
    }
}
