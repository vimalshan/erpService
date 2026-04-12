using MediatR;
using travelTransactionService.Application.DTOs;

namespace travelTransactionService.Application.Commands;

public record CreateVendorCommand : IRequest<VendorMasterDto>
{
    public long VendorId { get; init; }
    public string Name { get; init; } = null!;
    public string CategoryType { get; init; } = "V";
    public string? AddressLine1 { get; init; }
    public string? PhoneNumber { get; init; }
    public string? ItPanNumber { get; init; }
}

public record UpdateVendorCommand : IRequest<bool>
{
    public long VendorId { get; init; }
    public string Name { get; init; } = null!;
    public string? AddressLine1 { get; init; }
    public string? PhoneNumber { get; init; }
    public string? ItPanNumber { get; init; }
    public string? BankName { get; init; }
    public string? AccountNumber { get; init; }
}

public record DeleteVendorCommand(long VendorId) : IRequest<bool>;

public record CreateTaxMasterCommand : IRequest<TaxMasterDto>
{
    public long VendorId { get; init; }
    public string TaxType { get; init; } = null!;
    public decimal? TaxRate { get; init; }
    public DateTime EffectiveDate { get; init; }
}

public record UpdateTaxRateCommand : IRequest<bool>
{
    public string TaxType { get; init; } = null!;
    public decimal NewRate { get; init; }
    public long ModifiedBy { get; init; }
}

public record CreateJaiInterfaceLineCommand : IRequest<JaiInterfaceLineDto>
{
    public decimal OrgId { get; init; }
    public decimal PartyId { get; init; }
    public decimal PartySiteId { get; init; }
    public string ImportModule { get; init; } = null!;
    public string TransactionNum { get; init; } = null!;
    public decimal TransactionLineNum { get; init; }
    public decimal CreatedBy { get; init; }
    public List<CreateJaiTaxLineItem> TaxLines { get; init; } = [];
}

public record CreateJaiTaxLineItem
{
    public long TaxLineNo { get; init; }
    public string? ExternalTaxCode { get; init; }
    public decimal? TaxRate { get; init; }
    public decimal? TaxAmount { get; init; }
}

public record UpdateGstAmountsCommand : IRequest<bool>
{
    public decimal InterfaceLineId { get; init; }
    public decimal SgstAmount { get; init; }
    public decimal CgstAmount { get; init; }
    public decimal IgstAmount { get; init; }
}

public record CreateTravelApParamsCommand : IRequest<TravelApParamsDto>
{
    public long ApUnitId { get; init; }
    public string AccountStatus { get; init; } = null!;
    public string AccountCode { get; init; } = null!;
    public long? ControlCombId { get; init; }
}
