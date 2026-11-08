using System;
using System.Text.RegularExpressions;
using LiveKit.Services;
using LiveKit.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LiveKit;

/// <summary>
/// Extension methods for configuring LiveKit services in an <see cref="IServiceCollection"/>.
/// <para>
/// Registers LiveKit Room, Egress, Ingress, SIP, and other services for ASP.NET Core dependency injection.
/// See <see href="https://docs.livekit.io/home/get-started/">LiveKit Documentation</see> for integration details.
/// </para>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds LiveKit services to the specified <see cref="IServiceCollection"/>.
    /// <para>
    /// Configures HTTP clients for all LiveKit services including Room, Egress, Ingress, SIP, Phone Number, Agent Dispatch, Cloud Agent, Connector, and Agent Simulation services.
    /// </para>
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configure">An action to configure LiveKit options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddLiveKit(this IServiceCollection services, Action<LiveKitOptions> configure)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        services.Configure(configure);
        services.AddTransient<ILiveKitTokenService, LiveKitTokenService>();
        services.AddTransient<ILiveKitTokenVerifier, LiveKitTokenVerifier>();
        services.AddTransient<ILiveKitWebhookReceiver, LiveKitWebhookReceiver>();

        RegisterHttpClient<ILiveKitRoomService, LiveKitRoomService>(services);
        RegisterHttpClient<ILiveKitEgressService, LiveKitEgressService>(services);
        RegisterHttpClient<ILiveKitIngressService, LiveKitIngressService>(services);
        RegisterHttpClient<ILiveKitSipService, LiveKitSipService>(services);
        RegisterHttpClient<ILiveKitPhoneNumberService, LiveKitPhoneNumberService>(services);
        RegisterHttpClient<ILiveKitAgentDispatchService, LiveKitAgentDispatchService>(services);
        RegisterHttpClient<ILiveKitCloudAgentService, LiveKitCloudAgentService>(services, useAgentsUrl: true);
        RegisterHttpClient<ILiveKitConnectorService, LiveKitConnectorService>(services);
        RegisterHttpClient<ILiveKitAgentSimulationService, LiveKitAgentSimulationService>(services);

        return services;
    }

    private static void RegisterHttpClient<TService, TImpl>(IServiceCollection services, bool useAgentsUrl = false)
        where TService : class
        where TImpl : class, TService
    {
        services.AddHttpClient<TService, TImpl>((sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<LiveKitOptions>>().Value;
            client.BaseAddress = new Uri(ToHttpUrl(useAgentsUrl ? GetAgentsUrl(options) : options.BaseUrl));
        });
    }

    // Twirp runs over HTTP, and only some HTTP handlers accept ws/wss URLs, so convert them to http/https.
    private static string ToHttpUrl(string url)
    {
        if (url.StartsWith("wss://", StringComparison.OrdinalIgnoreCase))
        {
            return "https://" + url.Substring("wss://".Length);
        }

        if (url.StartsWith("ws://", StringComparison.OrdinalIgnoreCase))
        {
            return "http://" + url.Substring("ws://".Length);
        }

        return url;
    }

    // Cloud Agent is served from its own host. Derived the same way as the official LiveKit SDKs.
    private static string GetAgentsUrl(LiveKitOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.AgentsUrl))
        {
            return options.AgentsUrl!;
        }

        return Regex.Replace(ToHttpUrl(options.BaseUrl), @"^https?://[^.]+\.", "https://agents.", RegexOptions.IgnoreCase);
    }
}
