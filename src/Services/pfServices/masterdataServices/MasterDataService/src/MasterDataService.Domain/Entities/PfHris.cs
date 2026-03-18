using MasterDataService.Domain.Common;

namespace MasterDataService.Domain.Entities;

public class PfHris : BaseEntity
{
    public string CompanyCode { get; set; } = string.Empty;
    public decimal EmployeeNumber { get; set; }
    public decimal PinNumber { get; set; }
}
