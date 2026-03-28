namespace AuthorizationService.Domain.Entities;

/// <summary>
/// Right Entity - Maps to DD_RIGHTS table
/// </summary>
public class Right : BaseEntity
{
    public decimal RightCode { get; set; }
    public string? RightDescription { get; set; }

    public Right() { }

    public Right(decimal rightCode, string? rightDescription)
    {
        RightCode = rightCode;
        RightDescription = rightDescription;
    }
}
