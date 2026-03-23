using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LiveKit.Proto;
using LiveKit.Authentication;
using Microsoft.Extensions.Logging;

namespace LiveKit.Services;

/// <inheritdoc cref="ILiveKitAgentSimulationService" />
public sealed class LiveKitAgentSimulationService : TwirpClient, ILiveKitAgentSimulationService
{
    private const string ServiceName = "AgentSimulation";

    /// <summary>
    /// Initializes a new instance of the <see cref="LiveKitAgentSimulationService"/> class.
    /// </summary>
    public LiveKitAgentSimulationService(HttpClient httpClient, ILogger<LiveKitAgentSimulationService> logger, ILiveKitTokenService _tokenService)
        : base(httpClient, logger, ServiceName, _tokenService)
    {
    }

    /// <inheritdoc/>
    public async Task<SimulationRun.Types.Create.Types.Response> CreateSimulationRunAsync(SimulationRun.Types.Create.Types.Request request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<SimulationRun.Types.Create.Types.Response>("CreateSimulationRun", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<SimulationRun.Types.ConfirmSourceUpload.Types.Response> ConfirmSimulationSourceUploadAsync(
        SimulationRun.Types.ConfirmSourceUpload.Types.Request request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<SimulationRun.Types.ConfirmSourceUpload.Types.Response>("ConfirmSimulationSourceUpload", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<SimulationRun.Types.Get.Types.Response> GetSimulationRunAsync(SimulationRun.Types.Get.Types.Request request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<SimulationRun.Types.Get.Types.Response>("GetSimulationRun", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<SimulationRun.Types.List.Types.Response> ListSimulationRunsAsync(SimulationRun.Types.List.Types.Request request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<SimulationRun.Types.List.Types.Response>("ListSimulationRuns", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<SimulationRun.Types.Cancel.Types.Response> CancelSimulationRunAsync(SimulationRun.Types.Cancel.Types.Request request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<SimulationRun.Types.Cancel.Types.Response>("CancelSimulationRun", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<Scenario.Types.Create.Types.Response> CreateScenarioAsync(Scenario.Types.Create.Types.Request request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<Scenario.Types.Create.Types.Response>("CreateScenario", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<Scenario.Types.CreateFromSession.Types.Response> CreateScenarioFromSessionAsync(
        Scenario.Types.CreateFromSession.Types.Request request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<Scenario.Types.CreateFromSession.Types.Response>("CreateScenarioFromSession", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<Scenario.Types.Delete.Types.Response> DeleteScenarioAsync(Scenario.Types.Delete.Types.Request request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<Scenario.Types.Delete.Types.Response>("DeleteScenario", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<Scenario.Types.Update.Types.Response> UpdateScenarioAsync(Scenario.Types.Update.Types.Request request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<Scenario.Types.Update.Types.Response>("UpdateScenario", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<ScenarioGroup.Types.Create.Types.Response> CreateScenarioGroupAsync(ScenarioGroup.Types.Create.Types.Request request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ScenarioGroup.Types.Create.Types.Response>("CreateScenarioGroup", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<ScenarioGroup.Types.Delete.Types.Response> DeleteScenarioGroupAsync(ScenarioGroup.Types.Delete.Types.Request request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ScenarioGroup.Types.Delete.Types.Response>("DeleteScenarioGroup", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<ScenarioGroup.Types.List.Types.Response> ListScenarioGroupsAsync(ScenarioGroup.Types.List.Types.Request request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ScenarioGroup.Types.List.Types.Response>("ListScenarioGroups", null, request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<Scenario.Types.List.Types.Response> ListScenariosAsync(Scenario.Types.List.Types.Request request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<Scenario.Types.List.Types.Response>("ListScenarios", null, request, cancellationToken).ConfigureAwait(false);
    }
}
