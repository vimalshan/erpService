namespace VisitorServices.Domain.Enums;

/// <summary>
/// I = Inside (checked in), O = Outside (checked out), C = Cancelled
/// </summary>
public enum VisitorStatus
{
    Inside = 'I',
    Outside = 'O',
    Cancelled = 'C'
}
