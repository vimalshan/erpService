using MasterDataService.Domain.Common;

namespace MasterDataService.Domain.Entities;

public class FundType : BaseEntity
{
    public string FundTypeCode { get; set; } = string.Empty;
    public string FundTypeName { get; set; } = string.Empty;
}
