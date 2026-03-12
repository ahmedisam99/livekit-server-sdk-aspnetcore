namespace LiveKit;

/// <summary>
/// Represents the error codes defined by the Twirp protocol.
/// </summary>
public enum TwirpErrorCode
{
    /// <summary>The operation was canceled.</summary>
    Canceled,

    /// <summary>An unknown error occurred.</summary>
    Unknown,

    /// <summary>The client specified an invalid argument.</summary>
    InvalidArgument,

    /// <summary>The request was malformed.</summary>
    Malformed,

    /// <summary>The operation deadline was exceeded.</summary>
    DeadlineExceeded,

    /// <summary>The requested resource was not found.</summary>
    NotFound,

    /// <summary>The request was routed to an invalid endpoint.</summary>
    BadRoute,

    /// <summary>The resource already exists.</summary>
    AlreadyExists,

    /// <summary>The caller does not have permission to execute the operation.</summary>
    PermissionDenied,

    /// <summary>The request does not have valid authentication credentials.</summary>
    Unauthenticated,

    /// <summary>Some resource has been exhausted.</summary>
    ResourceExhausted,

    /// <summary>The operation was rejected because the system is not in a required state.</summary>
    FailedPrecondition,

    /// <summary>The operation was aborted.</summary>
    Aborted,

    /// <summary>The operation was attempted past the valid range.</summary>
    OutOfRange,

    /// <summary>The operation is not implemented or not supported.</summary>
    Unimplemented,

    /// <summary>An internal error occurred.</summary>
    Internal,

    /// <summary>The service is currently unavailable.</summary>
    Unavailable,

    /// <summary>Unrecoverable data loss or corruption.</summary>
    DataLoss
}
