using IntegrationService.Application.DTOs;
using MediatR;

namespace IntegrationService.Application.PurchaseOrders.Commands;

public record CreatePurchaseOrderCommand(
    long PoSeqId,
    long OracleOrgId,
    long OraclePoId,
    string PoNumber,
    long VendorSiteId,
    long DueDays,
    long DueDayMonthOffset,
    long MonthForward
) : IRequest<PurchaseOrderDto>;

public record UpdatePurchaseOrderCommand(
    long PoSeqId,
    long DueDays,
    long DueDayMonthOffset,
    long MonthForward
) : IRequest<PurchaseOrderDto>;

public record DeletePurchaseOrderCommand(long PoSeqId) : IRequest<bool>;

public record AddMaterialReceiptCommand(
    long MrcSeqId,
    long PurchaseOrderId,
    string MrcNumber,
    long? SequenceNumber,
    DateTime? ReceiveDate,
    long? VendorId,
    long? VendorSiteId
) : IRequest<MaterialReceiptDto>;
