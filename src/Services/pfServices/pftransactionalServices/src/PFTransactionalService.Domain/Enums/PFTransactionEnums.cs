namespace PFTransactionalService.Domain.Enums;

public enum TransactionStatus
{
    Pending = 'P',
    Posted = 'O',
    Cancelled = 'C',
    Reversed = 'R'
}

public enum AccumulationStatus
{
    Active = 'A',
    Closed = 'C',
    Frozen = 'F'
}

public enum FinancialYearStatus
{
    Open = 'O',
    Closed = 'C'
}

public enum CertificateStatus
{
    Generated = 'G',
    Issued = 'I',
    Cancelled = 'C'
}
