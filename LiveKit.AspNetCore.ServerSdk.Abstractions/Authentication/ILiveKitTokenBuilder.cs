using System;
using System.Collections.Generic;
using LiveKit.Proto;

namespace LiveKit.Authentication;

/// <summary>
/// Builder for creating LiveKit authentication tokens.
/// </summary>
public interface ILiveKitTokenBuilder
{
    /// <summary>
    /// Sets the participant name.
    /// </summary>
    ILiveKitTokenBuilder WithParticipantName(string name);

    /// <summary>
    /// Sets the metadata for the participant.
    /// </summary>
    ILiveKitTokenBuilder WithMetadata(string metadata);

    /// <summary>
    /// Adds a single attribute.
    /// </summary>
    ILiveKitTokenBuilder WithAttribute(string key, string value);

    /// <summary>
    /// Conditionally adds an attribute.
    /// </summary>
    ILiveKitTokenBuilder WithAttributeIf(bool cond, string key, string value);

    /// <summary>
    /// Sets all attributes.
    /// </summary>
    ILiveKitTokenBuilder WithAttributes(IDictionary<string, string> attributes);

    /// <summary>
    /// Sets the participant kind.
    /// </summary>
    ILiveKitTokenBuilder WithKind(string kind);

    /// <summary>
    /// Sets the room preset.
    /// </summary>
    ILiveKitTokenBuilder WithRoomPreset(string roomPreset);

    /// <summary>
    /// Configures video grant permissions.
    /// </summary>
    ILiveKitTokenBuilder WithVideoGrant(Action<VideoGrant> configureVideoGrant);

    /// <summary>
    /// Sets video grant permissions.
    /// </summary>
    ILiveKitTokenBuilder WithVideoGrant(VideoGrant videoGrant);

    /// <summary>
    /// Configures SIP grant permissions.
    /// </summary>
    ILiveKitTokenBuilder WithSipGrant(Action<SipGrant> configureSipGrant);

    /// <summary>
    /// Sets SIP grant permissions.
    /// </summary>
    ILiveKitTokenBuilder WithSipGrant(SipGrant sipGrant);

    /// <summary>
    /// Configures room configuration.
    /// </summary>
    ILiveKitTokenBuilder WithRoomConfiguration(Action<RoomConfiguration> configureRoomConfiguration);

    /// <summary>
    /// Sets room configuration.
    /// </summary>
    ILiveKitTokenBuilder WithRoomConfiguration(RoomConfiguration roomConfiguration);

    /// <summary>
    /// Sets the token time-to-live.
    /// </summary>
    ILiveKitTokenBuilder WithTimeToLive(TimeSpan ttl);

    /// <summary>
    /// Builds and returns the JWT token string.
    /// </summary>
    string ToJwt();
}

