namespace ArchiveService.Application.DTOs;

public record ServiceOrderDto
{
    public string SernoDell { get; init; } = string.Empty;
    public string? Branch { get; init; }
    public string? SapLogin { get; init; }
    public DateTime? PostingDate { get; init; }
    public string? SapId { get; init; }
    public string? Sla { get; init; }
    public string? ProductId { get; init; }
    public string? ServiceTag { get; init; }
    public string? RelatedCase { get; init; }
    public string? Lob { get; init; }
    public string? CallStatus { get; init; }
    public string? CurrentRc { get; init; }
    public string? EngineerId { get; init; }
    public string? EngineerName { get; init; }
    public string? EngMobNo { get; init; }
    public string? OrgName { get; init; }
    public string? CustomerName { get; init; }
    public string? ContactNo { get; init; }
    public string? Address { get; init; }
    public string? AltContactNo { get; init; }
    public DateTime? DispatchDate { get; init; }
    public DateTime? CustEtaDate { get; init; }
    public DateTime? PartEtaDate { get; init; }
    public string? TechSupName { get; init; }
    public string? Dsp { get; init; }
    public string? ProblemDescription { get; init; }
    public string? LongDescription { get; init; }
    public string? ReasonCode { get; init; }
    public string? Activity { get; init; }
    public DateTime? OnsiteDate { get; init; }
    public DateTime? CompletedDate { get; init; }
    public string? Flag { get; init; }
    public DateTime? EnteredOn { get; init; }
    public string? EnteredBy { get; init; }
    public DateTime? ChangedOn { get; init; }
    public string? ChangedBy { get; init; }
    public List<ServiceOrderDetailDto> Details { get; init; } = [];
}

public record ServiceOrderDetailDto
{
    public long Id { get; init; }
    public string? SernoDell { get; init; }
    public string? PartNo { get; init; }
    public string? Quantity { get; init; }
    public string? UniqueId { get; init; }
    public string? PartStatus { get; init; }
    public DateTime? EnteredOn { get; init; }
    public string? EnteredBy { get; init; }
}

public record ToolKitDto
{
    public long Id { get; init; }
    public string? KitCode { get; init; }
    public string? ImeiNo { get; init; }
    public string? EngineerId { get; init; }
    public string? Flag { get; init; }
    public DateTime? EnteredOn { get; init; }
    public string? EnteredBy { get; init; }
    public List<ToolKitTransactionDto> Transactions { get; init; } = [];
}

public record ToolKitTransactionDto
{
    public long Id { get; init; }
    public long? ToolkitId { get; init; }
    public int? ToolkitNameId { get; init; }
    public string? EngineerId { get; init; }
    public string? IssuerId { get; init; }
    public int? Quantity { get; init; }
    public string? Status { get; init; }
    public string? Remarks { get; init; }
    public string? AdditionalRemarks { get; init; }
    public DateTime? EnteredOn { get; init; }
    public string? EnteredBy { get; init; }
}

public record PagedResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
}
