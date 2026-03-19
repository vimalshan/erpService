using AutoMapper;
using IntegrationService.Application.DTOs;
using IntegrationService.Domain.Entities;
using IntegrationService.Domain.Exceptions;
using IntegrationService.Domain.Interfaces;
using IntegrationService.Domain.ValueObjects;
using MediatR;

namespace IntegrationService.Application.PurchaseOrders.Commands;

public class CreatePurchaseOrderHandler(
    IPurchaseOrderRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<CreatePurchaseOrderCommand, PurchaseOrderDto>
{
    public async Task<PurchaseOrderDto> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var paymentTerms = new PaymentTerms(request.DueDays, request.DueDayMonthOffset, request.MonthForward);
        var po = PurchaseOrder.Create(request.PoSeqId, request.OracleOrgId, request.OraclePoId,
            request.PoNumber, request.VendorSiteId, paymentTerms);

        await repository.AddAsync(po, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<PurchaseOrderDto>(po);
    }
}

public class UpdatePurchaseOrderHandler(
    IPurchaseOrderRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<UpdatePurchaseOrderCommand, PurchaseOrderDto>
{
    public async Task<PurchaseOrderDto> Handle(UpdatePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var po = await repository.GetByIdAsync(request.PoSeqId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(PurchaseOrder), request.PoSeqId);

        var paymentTerms = new PaymentTerms(request.DueDays, request.DueDayMonthOffset, request.MonthForward);
        po.UpdatePaymentTerms(paymentTerms);

        await repository.UpdateAsync(po, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<PurchaseOrderDto>(po);
    }
}

public class DeletePurchaseOrderHandler(
    IPurchaseOrderRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeletePurchaseOrderCommand, bool>
{
    public async Task<bool> Handle(DeletePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.PoSeqId, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class AddMaterialReceiptHandler(
    IPurchaseOrderRepository poRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<AddMaterialReceiptCommand, MaterialReceiptDto>
{
    public async Task<MaterialReceiptDto> Handle(AddMaterialReceiptCommand request, CancellationToken cancellationToken)
    {
        var po = await poRepository.GetWithMaterialReceiptsAsync(request.PurchaseOrderId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(PurchaseOrder), request.PurchaseOrderId);

        var mrc = MaterialReceiptCertificate.Create(request.MrcSeqId, request.PurchaseOrderId,
            request.MrcNumber, request.SequenceNumber, request.ReceiveDate,
            request.VendorId, request.VendorSiteId);

        po.AddMaterialReceipt(mrc);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<MaterialReceiptDto>(mrc);
    }
}
