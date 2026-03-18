namespace ProxyModule.Application.DTOs;

public class ProxyRightDto
{
    public long ProxyId { get; set; }
    public long ProxyUserId { get; set; }
    public long DelegatedUserId { get; set; }
    public DateTime ProxyStartDate { get; set; }
    public DateTime? ProxyEndDate { get; set; }
    public string ProxyType { get; set; } = default!;
    public string ProxyStatus { get; set; } = default!;
    public string? Scope { get; set; }
    public string? Notes { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public long? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public bool IsCurrentlyActive { get; set; }
}
