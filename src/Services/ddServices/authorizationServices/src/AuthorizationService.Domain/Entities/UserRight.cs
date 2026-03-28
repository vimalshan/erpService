namespace AuthorizationService.Domain.Entities;

/// <summary>
/// UserRight Entity - Maps to DD_USERRIGHTS table
/// </summary>
public class UserRight : BaseEntity
{
    public string? UserId { get; set; }
    public decimal? PinNumber { get; set; }
    public decimal? RightCode { get; set; }
    public string? BusinessCode { get; set; }
    public string? UnitCode { get; set; }
    public decimal? RightMode { get; set; }

    public UserRight() { }

    public UserRight(string? userId, decimal? pinNumber, decimal? rightCode)
    {
        UserId = userId;
        PinNumber = pinNumber;
        RightCode = rightCode;
    }
}
