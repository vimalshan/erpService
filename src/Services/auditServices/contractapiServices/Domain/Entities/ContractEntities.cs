using ContractService.Domain.Events;

namespace ContractService.Domain.Entities;

public class Contract
{
    public int ContractId { get; set; }
    public string ContractNumber { get; set; } = string.Empty;
    public string ContractName { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public string? ContractType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; } = "Active";
    public decimal? TotalValue { get; set; }
    public string? Currency { get; set; } = "USD";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? SignedDate { get; set; }
    public string? SignedByClient { get; set; }
    public string? SignedByDNV { get; set; }
    public string? ContractPath { get; set; }
    public string? Terms { get; set; }
    public string? Notes { get; set; }
    public DateTime? RenewalDate { get; set; }
    public bool AutoRenewal { get; set; }

    public ICollection<ContractServiceEntity> ContractServices { get; set; } = new List<ContractServiceEntity>();
    public ICollection<ContractSite> ContractSites { get; set; } = new List<ContractSite>();

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void ClearDomainEvents() => _domainEvents.Clear();

    public static Contract Create(string contractNumber, string contractName, int companyId, string? contractType,
        DateTime startDate, DateTime? endDate, decimal? totalValue, string? currency, int? createdBy)
    {
        var contract = new Contract
        {
            ContractNumber = contractNumber, ContractName = contractName, CompanyId = companyId,
            ContractType = contractType, StartDate = startDate, EndDate = endDate,
            TotalValue = totalValue, Currency = currency ?? "USD",
            Status = "Draft", IsActive = true, CreatedBy = createdBy, ModifiedBy = createdBy,
            CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow
        };
        contract._domainEvents.Add(new ContractCreatedEvent(contract.ContractId, contractNumber, contractName, companyId));
        return contract;
    }

    public void ChangeStatus(string newStatus, int? modifiedBy)
    {
        var oldStatus = Status;
        Status = newStatus;
        ModifiedDate = DateTime.UtcNow;
        ModifiedBy = modifiedBy;
        _domainEvents.Add(new ContractStatusChangedEvent(ContractId, oldStatus, newStatus));
    }

    public void Renew(DateTime? newEndDate, int? modifiedBy)
    {
        RenewalDate = DateTime.UtcNow;
        EndDate = newEndDate;
        Status = "Active";
        ModifiedDate = DateTime.UtcNow;
        ModifiedBy = modifiedBy;
        _domainEvents.Add(new ContractRenewedEvent(ContractId, ContractNumber, newEndDate));
    }
}

public class ContractServiceEntity
{
    public int ContractServiceId { get; set; }
    public int ContractId { get; set; }
    public int ServiceId { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal? UnitPrice { get; set; }
    public decimal? TotalPrice { get; set; }
    public string? Currency { get; set; } = "USD";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Status { get; set; } = "Active";
    public string? Notes { get; set; }

    public Contract Contract { get; set; } = null!;
}

public class ContractSite
{
    public int ContractSiteId { get; set; }
    public int ContractId { get; set; }
    public int SiteId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Status { get; set; } = "Active";
    public string? Notes { get; set; }

    public Contract Contract { get; set; } = null!;
}
