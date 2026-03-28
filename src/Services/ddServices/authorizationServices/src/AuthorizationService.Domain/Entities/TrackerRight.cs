namespace AuthorizationService.Domain.Entities;

/// <summary>
/// TrackerRight Entity - Maps to DD_TRACKERRIGHTS table
/// </summary>
public class TrackerRight : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public decimal? PinNumber { get; set; }
    public string? TrackerMode { get; set; }
    public string? BusinessCode { get; set; }
    public string? UnitCode { get; set; }
    public char? TrackerRights { get; set; }
    public char? VtcRights { get; set; }
    public char? RepresentingUnit { get; set; }
    public char? LetRight { get; set; }
    public char? CarRight { get; set; }

    public TrackerRight() { }

    public TrackerRight(string userId, decimal? pinNumber, string? businessCode)
    {
        UserId = userId;
        PinNumber = pinNumber;
        BusinessCode = businessCode;
    }

    public bool HasTrackerAccess => TrackerRights == 'Y' || TrackerRights == '1';
    public bool HasVtcAccess => VtcRights == 'Y' || VtcRights == '1';
}
