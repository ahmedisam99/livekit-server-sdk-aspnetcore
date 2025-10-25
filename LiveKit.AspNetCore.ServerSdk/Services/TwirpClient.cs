using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using LiveKit.Authentication;
using Microsoft.Extensions.Logging;

namespace LiveKit.Services;

/// <summary>
/// Base class for Twirp protocol clients.
/// </summary>
public abstract class TwirpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;
    private readonly string _serviceName;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly ILiveKitTokenService _tokenService;

    protected TwirpClient(HttpClient httpClient, ILogger logger, string serviceName, ILiveKitTokenService tokenService)
    {
        _httpClient = httpClient;
        _logger = logger;
        _serviceName = serviceName;
        _tokenService = tokenService;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };
    }

    protected async Task<TResponse> MakeRequestAsync<TResponse>(
        string methodName,
        string? roomName,
        object? requestBody,
        string? token,
        CancellationToken cancellationToken = default)
    {
        var authToken = token ?? _tokenService.CreateServerToken();
        var url = $"/twirp/livekit.{_serviceName}/{methodName}";

        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

        if (requestBody != null)
        {
            var json = JsonSerializer.Serialize(requestBody, _jsonOptions);
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
            return default!;
        }

        return JsonSerializer.Deserialize<TResponse>(responseContent, _jsonOptions) ??
               throw new InvalidOperationException("Failed to deserialize response");
    }
}
