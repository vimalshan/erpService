using MasterDataService.Domain.Common;

namespace MasterDataService.Domain.Entities;

public class RateType : BaseEntity
{
    public string RateTypeCode { get; set; } = string.Empty;
    public string? RateTypeName { get; set; }
}
