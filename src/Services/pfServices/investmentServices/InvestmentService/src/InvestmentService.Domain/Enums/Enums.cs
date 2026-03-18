namespace InvestmentService.Domain.Enums;

public enum InvestmentStatus
{
    Active = 'A',
    Matured = 'M',
    Redeemed = 'R',
    Cancelled = 'C'
}

public enum SaleType
{
    Full = 'F',
    Partial = 'P'
}

public enum InterestOption
{
    Simple,
    Compound
}

public enum InterestFrequency
{
    Monthly = 'M',
    Quarterly = 'Q',
    HalfYearly = 'H',
    Yearly = 'Y'
}

public enum CallPutOption
{
    Call = 'C',
    Put = 'P',
    None = 'N'
}

public enum ApprovalFlag
{
    Pending = 'P',
    Approved = 'A',
    Rejected = 'R'
}

public enum PaymentMode
{
    Cheque = 'C',
    Transfer = 'T',
    Cash = 'S'
}

public enum ScheduleType
{
    Interest,
    Principal,
    Both
}

public enum BankEntryType
{
    Purchase,
    Redemption,
    Interest
}

public enum BrokerStatus
{
    Active = 'A',
    Inactive = 'I'
}
