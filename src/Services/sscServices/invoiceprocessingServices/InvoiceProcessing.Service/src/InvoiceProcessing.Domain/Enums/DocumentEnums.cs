namespace InvoiceProcessing.Domain.Enums;

public enum DocumentType
{
    POBased,
    NonPOBased,
    ReceiptBased,
    TravelVendor
}

public enum DocumentStatus
{
    Draft,
    Submitted,
    InProcess,
    Approved,
    Rejected,
    Cancelled,
    OnHold,
    Completed
}

public enum InvoiceStatus
{
    Pending,
    Validated,
    Processed,
    Paid,
    Cancelled
}

public enum ApprovalStatus
{
    Pending,
    Approved,
    Rejected
}

public enum HoldStatus
{
    None,
    OnHold,
    Released
}

public enum CorrespondenceStatus
{
    Open,
    Closed
}

public enum ScanLocation
{
    Local,
    SSC
}

public enum RescanStatus
{
    Pending,
    Completed
}

public enum AllocationAction
{
    Allocated,
    Pulled,
    Returned
}
