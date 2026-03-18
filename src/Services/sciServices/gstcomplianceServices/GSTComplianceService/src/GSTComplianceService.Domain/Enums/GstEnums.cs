namespace GSTComplianceService.Domain.Enums;

public enum GstType
{
    Regular = 'R',
    Composition = 'C',
    Unregistered = 'U',
    Consumer = 'N'
}

public enum GstStatus
{
    Pending = 'P',
    Active = 'A',
    Inactive = 'I',
    Suspended = 'S'
}

public enum RegistrationType
{
    Regular = 1,
    Composition = 2,
    CasualTaxable = 3,
    NonResidentTaxable = 4,
    InputServiceDistributor = 5,
    SEZDeveloper = 6,
    SEZUnit = 7,
    TDSDeductor = 8,
    TCSCollector = 9
}
