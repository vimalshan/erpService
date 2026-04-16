namespace ContractService.Application.DTOs;

public record ContractDto(
    int ContractId, string ContractNumber, string ContractName, int CompanyId, string? ContractType,
    DateTime StartDate, DateTime? EndDate, string Status, decimal? TotalValue, string? Currency,
    bool IsActive, DateTime CreatedDate, DateTime ModifiedDate, int? CreatedBy, int? ModifiedBy,
    DateTime? SignedDate, string? SignedByClient, string? SignedByDNV, string? ContractPath,
    string? Terms, string? Notes, DateTime? RenewalDate, bool AutoRenewal,
    List<ContractServiceDto> ContractServices, List<ContractSiteDto> ContractSites);

public record ContractServiceDto(
    int ContractServiceId, int ContractId, int ServiceId, int Quantity,
    decimal? UnitPrice, decimal? TotalPrice, string? Currency, bool IsActive,
    DateTime? StartDate, DateTime? EndDate, string? Status, string? Notes);

public record ContractSiteDto(
    int ContractSiteId, int ContractId, int SiteId, bool IsActive,
    DateTime? StartDate, DateTime? EndDate, string? Status, string? Notes);

public record CreateContractDto(
    string ContractNumber, string ContractName, int CompanyId, string? ContractType,
    DateTime StartDate, DateTime? EndDate, decimal? TotalValue, string? Currency,
    string? SignedByClient, string? SignedByDNV, string? ContractPath, string? Terms, string? Notes,
    bool AutoRenewal, int? CreatedBy);

public record UpdateContractDto(
    int ContractId, string ContractNumber, string ContractName, int CompanyId, string? ContractType,
    DateTime StartDate, DateTime? EndDate, string Status, decimal? TotalValue, string? Currency,
    bool IsActive, string? SignedByClient, string? SignedByDNV, string? ContractPath,
    string? Terms, string? Notes, DateTime? RenewalDate, bool AutoRenewal, int? ModifiedBy);
