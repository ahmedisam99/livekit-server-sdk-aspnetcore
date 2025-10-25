using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Livekit;
using LiveKit.Authentication;
using Microsoft.Extensions.Logging;

namespace LiveKit.Services;

public sealed class LiveKitPhoneNumberService : TwirpClient, ILiveKitPhoneNumberService
{
    private const string ServiceName = "PhoneNumberService";

    public LiveKitPhoneNumberService(HttpClient httpClient, ILogger<LiveKitPhoneNumberService> logger, ILiveKitTokenService _tokenService)
        : base(httpClient, logger, ServiceName, _tokenService)
    {
    }

    public async Task<SearchPhoneNumbersResponse> SearchPhoneNumbersAsync(SearchPhoneNumbersRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<SearchPhoneNumbersResponse>("SearchPhoneNumbers", null, request, null, cancellationToken);
    }

    public async Task<PurchasePhoneNumberResponse> PurchasePhoneNumberAsync(PurchasePhoneNumberRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<PurchasePhoneNumberResponse>("PurchasePhoneNumber", null, request, null, cancellationToken);
    }

    public async Task<ListPhoneNumbersResponse> ListPhoneNumbersAsync(ListPhoneNumbersRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ListPhoneNumbersResponse>("ListPhoneNumbers", null, request, null, cancellationToken);
    }

    public async Task<GetPhoneNumberResponse> GetPhoneNumberAsync(GetPhoneNumberRequest request, CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<GetPhoneNumberResponse>("GetPhoneNumber", null, request, null, cancellationToken);
    }

    public async Task<UpdatePhoneNumberResponse> UpdatePhoneNumberAsync(UpdatePhoneNumberRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<UpdatePhoneNumberResponse>("UpdatePhoneNumber", null, request, null, cancellationToken);
    }

    public async Task<ReleasePhoneNumbersResponse> ReleasePhoneNumbersAsync(ReleasePhoneNumbersRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ReleasePhoneNumbersResponse>("ReleasePhoneNumbers", null, request, null, cancellationToken);
    }
}
