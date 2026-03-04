using System.Threading;
using System.Threading.Tasks;
using LiveKit.Proto;

namespace LiveKit.Services;

/// <summary>
/// Service for managing phone numbers for SIP integration.
/// <para>
/// Provides methods for searching, purchasing, and managing phone numbers for SIP trunk configuration.
/// See <see href="https://docs.livekit.io/reference/server/server-apis/#phone-number-service">Phone Number Service Documentation</see> for more information.
/// </para>
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
