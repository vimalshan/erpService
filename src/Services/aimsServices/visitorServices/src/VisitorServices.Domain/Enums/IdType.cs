namespace VisitorServices.Domain.Enums;

/// <summary>
/// N = National ID, P = Passport, D = Driver's License, O = Other
/// </summary>
public enum IdType
{
    NationalId = 'N',
    Passport = 'P',
    DriverLicense = 'D',
    Other = 'O'
}
