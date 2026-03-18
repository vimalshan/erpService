using MasterDataService.Domain.Common;

namespace MasterDataService.Domain.Entities;

public class RoleMaster : AggregateRoot
{
    public long RoleCode { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public string? RoleDescription { get; set; }
    public string RoleStatus { get; set; } = "A";
}
