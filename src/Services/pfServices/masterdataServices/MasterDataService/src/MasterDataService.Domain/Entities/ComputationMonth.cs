using MasterDataService.Domain.Common;

namespace MasterDataService.Domain.Entities;

public class ComputationMonth : BaseEntity
{
    public long SerialNumber { get; set; }
    public string? MonthName { get; set; }
}
