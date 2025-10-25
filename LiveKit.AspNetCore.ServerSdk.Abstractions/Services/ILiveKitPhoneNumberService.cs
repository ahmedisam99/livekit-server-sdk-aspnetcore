using System.Threading;
using System.Threading.Tasks;
using Livekit;

namespace LiveKit.Services;

/// <summary>
/// Phone Number service for managing phone numbers for SIP integration.
/// </summary>
public interface ILiveKitPhoneNumberService
{
    /// <summary>
    /// Searches available phone numbers in inventory.
    /// </summary>
    Task<SearchPhoneNumbersResponse> SearchPhoneNumbersAsync(SearchPhoneNumbersRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Purchases phone numbers from inventory.
    /// </summary>
    Task<PurchasePhoneNumberResponse> PurchasePhoneNumberAsync(PurchasePhoneNumberRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists phone numbers for the project.
    /// </summary>
    Task<ListPhoneNumbersResponse> ListPhoneNumbersAsync(ListPhoneNumbersRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a specific phone number.
    /// </summary>
    Task<GetPhoneNumberResponse> GetPhoneNumberAsync(GetPhoneNumberRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a phone number configuration.
    /// </summary>
    Task<UpdatePhoneNumberResponse> UpdatePhoneNumberAsync(UpdatePhoneNumberRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Releases phone numbers back to inventory.
    /// </summary>
    Task<ReleasePhoneNumbersResponse> ReleasePhoneNumbersAsync(ReleasePhoneNumbersRequest request, CancellationToken cancellationToken = default);
}
