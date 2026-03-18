namespace ProxyModule.Application.DTOs;

public class UpdateProxyRightDto
{
    public long ProxyId { get; set; }
    public DateTime? ProxyStartDate { get; set; }
    public DateTime? ProxyEndDate { get; set; }
    public string? ProxyType { get; set; }
    public string? Scope { get; set; }
    public string? Notes { get; set; }
    public long UpdatedBy { get; set; }
}
