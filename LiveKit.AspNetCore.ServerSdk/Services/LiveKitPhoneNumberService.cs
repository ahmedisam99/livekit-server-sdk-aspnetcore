using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LiveKit.Proto;
using LiveKit.Authentication;
using Microsoft.Extensions.Logging;

namespace LiveKit.Services;

/// <inheritdoc cref="ILiveKitPhoneNumberService" />
public sealed class LiveKitPhoneNumberService : TwirpClient, ILiveKitPhoneNumberService
{
    private const string ServiceName = "PhoneNumberService";

    /// <summary>
    /// Initializes a new instance of the <see cref="LiveKitPhoneNumberService"/> class.
    /// </summary>
    public LiveKitPhoneNumberService(HttpClient httpClient, ILogger<LiveKitPhoneNumberService> logger, ILiveKitTokenService _tokenService)
        : base(httpClient, logger, ServiceName, _tokenService)
    {
    }

    /// <inheritdoc/>
    public async Task<SearchPhoneNumbersResponse> SearchPhoneNumbersAsync(SearchPhoneNumbersRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<SearchPhoneNumbersResponse>("SearchPhoneNumbers", null, request, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<PurchasePhoneNumberResponse> PurchasePhoneNumberAsync(PurchasePhoneNumberRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<PurchasePhoneNumberResponse>("PurchasePhoneNumber", null, request, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<ListPhoneNumbersResponse> ListPhoneNumbersAsync(ListPhoneNumbersRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ListPhoneNumbersResponse>("ListPhoneNumbers", null, request, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<GetPhoneNumberResponse> GetPhoneNumberAsync(GetPhoneNumberRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<GetPhoneNumberResponse>("GetPhoneNumber", null, request, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<UpdatePhoneNumberResponse> UpdatePhoneNumberAsync(UpdatePhoneNumberRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<UpdatePhoneNumberResponse>("UpdatePhoneNumber", null, request, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<ReleasePhoneNumbersResponse> ReleasePhoneNumbersAsync(ReleasePhoneNumbersRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ReleasePhoneNumbersResponse>("ReleasePhoneNumbers", null, request, cancellationToken);
    }
}
