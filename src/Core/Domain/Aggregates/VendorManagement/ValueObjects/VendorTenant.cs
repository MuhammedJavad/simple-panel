using System.ComponentModel;

namespace Domain.Aggregates.VendorManagement.ValueObjects;

public enum VendorTenant
{
    [Description("Express")]
    SnappExpress,
    [Description("Food")]
    SnappFood
}