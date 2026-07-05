using System.Threading;
using System.Threading.Tasks;
using LiveKit.Proto;

namespace LiveKit.Services;

/// <summary>
/// Service for managing agent simulations.
/// <para>
/// Provides methods for creating and managing simulation runs and creating scenarios from
/// sessions to test and validate AI agent behavior.
/// </para>
/// </summary>
public interface ILiveKitAgentSimulationService
{
    /// <summary>
    /// Creates a new simulation run.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<SimulationRun.Types.Create.Types.Response> CreateSimulationRunAsync(SimulationRun.Types.Create.Types.Request request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Confirms that the simulation source has been uploaded.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<SimulationRun.Types.ConfirmSourceUpload.Types.Response> ConfirmSimulationSourceUploadAsync(SimulationRun.Types.ConfirmSourceUpload.Types.Request request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets details of a simulation run.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<SimulationRun.Types.Get.Types.Response> GetSimulationRunAsync(SimulationRun.Types.Get.Types.Request request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists simulation runs.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<SimulationRun.Types.List.Types.Response> ListSimulationRunsAsync(SimulationRun.Types.List.Types.Request request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels a simulation run.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<SimulationRun.Types.Cancel.Types.Response> CancelSimulationRunAsync(SimulationRun.Types.Cancel.Types.Request request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a scenario from an existing session.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<Scenario.Types.CreateFromSession.Types.Response> CreateScenarioFromSessionAsync(Scenario.Types.CreateFromSession.Types.Request request, CancellationToken cancellationToken = default);
}
