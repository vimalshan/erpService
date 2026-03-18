namespace MemberService.Domain.Enums;

public enum MemberStatus
{
    Active = 'A',
    Inactive = 'I',
    Closed = 'C'
}

public enum EmployeeType
{
    New = 'N',       // N - New
    TransferInternal = 'S',  // S - Transfer within SRF
    TransferExternal = 'O'   // O - Transfer from Outside
}

public enum ContactType
{
    Personal = 'P',
    Official = 'O',
    Emergency = 'E'
}

public enum NomineeStatus
{
    Active = 'A',
    Inactive = 'I'
}

public enum PayrollStatus
{
    Active = 'A',
    Closed = 'C'
}
