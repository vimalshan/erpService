namespace ProxyModule.Application.DTOs;

public class CreateProxyRightDto
{
    public long ProxyUserId { get; set; }
    public long DelegatedUserId { get; set; }
    public DateTime ProxyStartDate { get; set; }
    public DateTime? ProxyEndDate { get; set; }
    public string ProxyType { get; set; } = default!;
    public string? Scope { get; set; }
    public string? Notes { get; set; }
    public long CreatedBy { get; set; }
}
