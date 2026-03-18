using MasterDataService.Domain.Common;

namespace MasterDataService.Domain.Entities;

public class StatusMaster : BaseEntity
{
    public string StatusType { get; set; } = string.Empty;
    public string StatusCodeValue { get; set; } = string.Empty;
    public string? StatusName { get; set; }
}
