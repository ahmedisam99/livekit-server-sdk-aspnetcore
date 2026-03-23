using System.Threading;
using System.Threading.Tasks;
using LiveKit.Proto;

namespace LiveKit.Services;

/// <summary>
/// Service for managing agent simulations, scenarios, and scenario groups.
/// <para>
/// Provides methods for creating and managing simulation runs, scenarios, and scenario groups
/// to test and validate AI agent behavior.
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
    /// Creates a new scenario.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<Scenario.Types.Create.Types.Response> CreateScenarioAsync(Scenario.Types.Create.Types.Request request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a scenario from an existing session.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<Scenario.Types.CreateFromSession.Types.Response> CreateScenarioFromSessionAsync(Scenario.Types.CreateFromSession.Types.Request request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a scenario.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<Scenario.Types.Delete.Types.Response> DeleteScenarioAsync(Scenario.Types.Delete.Types.Request request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a scenario.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<Scenario.Types.Update.Types.Response> UpdateScenarioAsync(Scenario.Types.Update.Types.Request request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new scenario group.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<ScenarioGroup.Types.Create.Types.Response> CreateScenarioGroupAsync(ScenarioGroup.Types.Create.Types.Request request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a scenario group.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<ScenarioGroup.Types.Delete.Types.Response> DeleteScenarioGroupAsync(ScenarioGroup.Types.Delete.Types.Request request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists scenario groups.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<ScenarioGroup.Types.List.Types.Response> ListScenarioGroupsAsync(ScenarioGroup.Types.List.Types.Request request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists scenarios.
    /// </summary>
    /// <exception cref="LiveKitApiException">Thrown when the server returns a non-success HTTP status code.</exception>
    Task<Scenario.Types.List.Types.Response> ListScenariosAsync(Scenario.Types.List.Types.Request request, CancellationToken cancellationToken = default);
}
