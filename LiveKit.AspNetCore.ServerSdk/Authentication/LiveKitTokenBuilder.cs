using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Google.Protobuf;
using LiveKit.Proto;
using Microsoft.IdentityModel.Tokens;

namespace LiveKit.Authentication;

/// <inheritdoc/>
public sealed class LiveKitTokenBuilder : ILiveKitTokenBuilder
{
    private string Identity { get; set; }
    private string? Name { get; set; }
    private string? Metadata { get; set; }
    private IDictionary<string, string>? Attributes { get; set; }
    private string? Kind { get; set; }
    private string? RoomPreset { get; set; }
    private VideoGrant? VideoGrant { get; set; }
    private SipGrant? SipGrant { get; set; }
    private RoomConfiguration? RoomConfiguration { get; set; }
    private TimeSpan Ttl { get; set; } = TimeSpan.FromMinutes(60);

    private readonly string _apiKey;
    private readonly string _apiSecret;
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="LiveKitTokenBuilder"/> class.
    /// </summary>
    /// <param name="apiKey">The LiveKit API key.</param>
    /// <param name="apiSecret">The LiveKit API secret.</param>
    /// <param name="identity">The unique identity for the participant.</param>
    public LiveKitTokenBuilder(string apiKey, string apiSecret, string identity)
    {
        _apiKey = apiKey;
        _apiSecret = apiSecret;
        Identity = identity;

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

    /// <inheritdoc/>
    public ILiveKitTokenBuilder WithParticipantName(string name)
    {
        Name = name;

        return this;
    }

    /// <inheritdoc/>
    public ILiveKitTokenBuilder WithMetadata(string metadata)
    {
        Metadata = metadata;

        return this;
    }

    /// <inheritdoc/>
    public ILiveKitTokenBuilder WithAttribute(string key, string value)
    {
        Attributes ??= new Dictionary<string, string>();
        Attributes[key] = value;

        return this;
    }

    /// <inheritdoc/>
    public ILiveKitTokenBuilder WithAttributeIf(bool cond, string key, string value)
    {
        if (!cond)
        {
            return this;
        }

        Attributes ??= new Dictionary<string, string>();
        Attributes[key] = value;

        return this;
    }

    /// <inheritdoc/>
    public ILiveKitTokenBuilder WithAttributes(IDictionary<string, string> attributes)
    {
        Attributes = attributes;

        return this;
    }

    /// <inheritdoc/>
    public ILiveKitTokenBuilder WithKind(string kind)
    {
        Kind = kind;

        return this;
    }

    /// <inheritdoc/>
    public ILiveKitTokenBuilder WithRoomPreset(string roomPreset)
    {
        RoomPreset = roomPreset;

        return this;
    }

    /// <inheritdoc/>
    public ILiveKitTokenBuilder WithVideoGrant(Action<VideoGrant> configureVideoGrant)
    {
        var videoGrant = new VideoGrant();
        configureVideoGrant.Invoke(videoGrant);
        VideoGrant = videoGrant;

        return this;
    }

    /// <inheritdoc/>
    public ILiveKitTokenBuilder WithVideoGrant(VideoGrant videoGrant)
    {
        VideoGrant = videoGrant;

        return this;
    }

    /// <inheritdoc/>
    public ILiveKitTokenBuilder WithSipGrant(Action<SipGrant> configureSipGrant)
    {
        var sipGrant = new SipGrant();
        configureSipGrant.Invoke(sipGrant);
        SipGrant = sipGrant;

        return this;
    }

    /// <inheritdoc/>
    public ILiveKitTokenBuilder WithSipGrant(SipGrant sipGrant)
    {
        SipGrant = sipGrant;

        return this;
    }

    /// <inheritdoc/>
    public ILiveKitTokenBuilder WithRoomConfiguration(Action<RoomConfiguration> configureRoomConfiguration)
    {
        var roomConfiguration = new RoomConfiguration();
        configureRoomConfiguration.Invoke(roomConfiguration);
        RoomConfiguration = roomConfiguration;

        return this;
    }

    /// <inheritdoc/>
    public ILiveKitTokenBuilder WithRoomConfiguration(RoomConfiguration roomConfiguration)
    {
        RoomConfiguration = roomConfiguration;

        return this;
    }

    /// <inheritdoc/>
    public ILiveKitTokenBuilder WithTimeToLive(TimeSpan ttl)
    {
        Ttl = ttl;

        return this;
    }

    /// <inheritdoc/>
    public string ToJwt()
    {
        var now = DateTime.UtcNow;
        List<Claim> claims =
        [
            new(LiveKitClaims.Identity, Identity, ClaimValueTypes.String),
        ];

        if (!string.IsNullOrWhiteSpace(Name))
        {
            claims.Add(new Claim(LiveKitClaims.Name, Name, ClaimValueTypes.String));
        }

        if (!string.IsNullOrWhiteSpace(Metadata))
        {
            claims.Add(new Claim(LiveKitClaims.Metadata, Metadata, ClaimValueTypes.String));
        }

        if (!string.IsNullOrWhiteSpace(Kind))
        {
            claims.Add(new Claim(LiveKitClaims.Kind, Kind, ClaimValueTypes.String));
        }

        if (!string.IsNullOrWhiteSpace(RoomPreset))
        {
            claims.Add(new Claim(LiveKitClaims.RoomPreset, RoomPreset, ClaimValueTypes.String));
        }

        if (Attributes is not null && Attributes.Count > 0)
        {
            var attributesJson = JsonSerializer.Serialize(Attributes, _jsonOptions);
            claims.Add(new Claim(LiveKitClaims.Attributes, attributesJson, JsonClaimValueTypes.Json));
        }

        if (VideoGrant is not null)
        {
            var videoGrantJson = JsonSerializer.Serialize(VideoGrant, _jsonOptions);
            claims.Add(new Claim(LiveKitClaims.Video, videoGrantJson, JsonClaimValueTypes.Json));
        }

        if (SipGrant is not null)
        {
            var sipGrantJson = JsonSerializer.Serialize(SipGrant, _jsonOptions);
            claims.Add(new Claim(LiveKitClaims.Sip, sipGrantJson, JsonClaimValueTypes.Json));
        }

        if (RoomConfiguration is not null)
        {
            var roomConfigJson = JsonFormatter.Default.Format(RoomConfiguration);
            claims.Add(new Claim(LiveKitClaims.RoomConfig, roomConfigJson, JsonClaimValueTypes.Json));
        }

        var secret = Encoding.UTF8.GetBytes(_apiSecret);
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);

        var header = new JwtHeader(signingCredentials);
        var payload = new JwtPayload(
            issuer: _apiKey,
            audience: null,
            claims: claims,
            notBefore: now,
            expires: now.Add(Ttl),
            issuedAt: now);

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = new JwtSecurityToken(header, payload);

        return tokenHandler.WriteToken(token);
    }
}
