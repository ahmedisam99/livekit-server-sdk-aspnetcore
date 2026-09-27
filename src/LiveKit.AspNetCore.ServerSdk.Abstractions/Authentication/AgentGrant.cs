namespace LiveKit.Authentication;

/// <summary>
/// Agent grant containing permissions for LiveKit Cloud agent APIs.
/// <para>
/// In order to interact with the Cloud Agent, Agent Simulation, and Agent Database services, or to manage the agent dispatch queue, permission must be granted in the agent field of the JWT.
/// See <see href="https://docs.livekit.io/home/get-started/authentication/">LiveKit Authentication Documentation</see> for more information.
/// </para>
/// </summary>
public sealed class AgentGrant
{
    /// <summary>
    /// Permission to create, update, and delete Cloud Agents.
    /// </summary>
    public bool? Admin { get; set; }

    /// <summary>
    /// Permission to manage simulations and scenarios for evaluating agents.
    /// </summary>
    public bool? SimulationAdmin { get; set; }

    /// <summary>
    /// Permission to manage a project's agent databases.
    /// </summary>
    public bool? DatabaseAdmin { get; set; }

    /// <summary>
    /// Permission to manage a project's agent dispatch queue.
    /// Not required by <see cref="LiveKit.Services.ILiveKitAgentDispatchService"/>, which uses the room admin video grant.
    /// </summary>
    public bool? DispatchAdmin { get; set; }
}
