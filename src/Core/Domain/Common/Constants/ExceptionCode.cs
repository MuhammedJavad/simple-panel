using System.ComponentModel;

namespace Domain.Common.Constants;

public enum ExceptionCode
{
    [Description("An unexpected error has occurred. Please check the error log for more details")]
    Failed,
    InvalidPassword,
    UserNotFound,
    UserHasBeenBlocked,
    InvalidDomain,
    AccessDenied,
    [Description("A vendor with the same information already exists.")]
    VendorAlreadyExists,
    [Description("The vendor could not be found.")]
    VendorNotFound,
    [Description("Core services failed to provide a response")]
    CoreServicesNotAvailable,
    [Description("Cache server failed to provide a response")]
    CacheServerNotAvailable,
    VendorIdCannotBeNull,
    PasswordCannotBeNull,
    ClientIdCannotBeNull,
    ClientSecretCannotBeNull,
    
}