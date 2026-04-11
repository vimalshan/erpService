using MediatR;
using TransactionService.Application.DTOs;
using TransactionService.Application.EmployeeJournalVouchers.Commands.CreateEmployeeJV;
using TransactionService.Application.EmployeeJournalVouchers.Commands.PostEmployeeJV;
using TransactionService.Application.EmployeeJournalVouchers.Commands.ReverseEmployeeJV;
using TransactionService.Application.SupplierJournalVouchers.Commands.CreateSupplierJV;
using TransactionService.Application.SupplierJournalVouchers.Commands.PostSupplierJV;
using TransactionService.Application.TravelBatches.Commands.ApproveTravelBatch;
using TransactionService.Application.TravelBatches.Commands.CreateTravelBatch;
using TransactionService.Application.EmployeePayments;
using TransactionService.Application.AirlineInvoices;

namespace TransactionService.API.GraphQL;

public sealed class Mutation
{
    // ── Employee JV ──────────────────────────────────────────────────────────
    public async Task<EmployeeJVDto> CreateEmployeeJVAsync(
        [Service] IMediator mediator,
        CreateEmployeeJVInput input,
        CancellationToken ct)
        => await mediator.Send(new CreateEmployeeJVCommand
        {
            JvBatchId = input.JvBatchId,
            JvTpId = input.JvTpId,
            JvType = input.JvType,
            JvDate = input.JvDate,
            JvEmpSysId = input.JvEmpSysId,
            JvTrnType = input.JvTrnType,
            JvNetAmt = input.JvNetAmt,
            JvPayUnitId = input.JvPayUnitId,
            CreatedBy = input.CreatedBy
        }, ct);

    public async Task<bool> PostEmployeeJVAsync(
        [Service] IMediator mediator, long jvBatchId, string? oracleRefNo, long postedBy, CancellationToken ct)
    {
        await mediator.Send(new PostEmployeeJVCommand(jvBatchId, oracleRefNo, postedBy), ct);
        return true;
    }

    public async Task<bool> ReverseEmployeeJVAsync(
        [Service] IMediator mediator, long jvBatchId, long reversedBy, CancellationToken ct)
    {
        await mediator.Send(new ReverseEmployeeJVCommand(jvBatchId, reversedBy), ct);
        return true;
    }

    // ── Supplier JV ──────────────────────────────────────────────────────────
    public async Task<SupplierJVDto> CreateSupplierJVAsync(
        [Service] IMediator mediator,
        CreateSupplierJVInput input,
        CancellationToken ct)
        => await mediator.Send(new CreateSupplierJVCommand
        {
            JvId = input.JvId,
            JvType = input.JvType,
            JvDate = input.JvDate,
            JvVendorId = input.JvVendorId,
            JvPayUnitId = input.JvPayUnitId,
            JvRefInvNo = input.JvRefInvNo,
            JvNetAmt = input.JvNetAmt,
            JvTrnType = input.JvTrnType,
            JvOraVendorId = input.JvOraVendorId,
            JvAdminId = input.JvAdminId,
            JvInvBatchId = input.JvInvBatchId,
            JvOraSiteId = input.JvOraSiteId,
            JvCenvatApplicable = input.JvCenvatApplicable,
            JvDocKeyNo = input.JvDocKeyNo,
            CreatedBy = input.CreatedBy
        }, ct);

    public async Task<bool> PostSupplierJVAsync(
        [Service] IMediator mediator, long jvId, string? oracleRefNo, long postedBy, CancellationToken ct)
    {
        await mediator.Send(new PostSupplierJVCommand(jvId, oracleRefNo, postedBy), ct);
        return true;
    }

    // ── Travel Batch ─────────────────────────────────────────────────────────
    public async Task<TravelBatchDto> CreateTravelBatchAsync(
        [Service] IMediator mediator,
        CreateTravelBatchInput input,
        CancellationToken ct)
        => await mediator.Send(new CreateTravelBatchCommand
        {
            BatchId = input.BatchId,
            AdminId = input.AdminId,
            PayUnitId = input.PayUnitId,
            VendorId = input.VendorId,
            InvNum = input.InvNum,
            InvAmount = input.InvAmount,
            BatchType = input.BatchType,
            CreatedBy = input.CreatedBy
        }, ct);

    public async Task<bool> AdminApproveTravelBatchAsync(
        [Service] IMediator mediator,
        string batchId, string approvedBy, string? approvedAmount, string? remarks,
        CancellationToken ct)
    {
        await mediator.Send(new AdminApproveTravelBatchCommand(batchId, approvedBy, approvedAmount, remarks), ct);
        return true;
    }

    public async Task<bool> FinanceApproveTravelBatchAsync(
        [Service] IMediator mediator, string batchId, string approvedBy, string? remarks, CancellationToken ct)
    {
        await mediator.Send(new FinanceApproveTravelBatchCommand(batchId, approvedBy, remarks), ct);
        return true;
    }

    public async Task<bool> RejectTravelBatchAsync(
        [Service] IMediator mediator, string batchId, string rejectedBy, string? remarks, CancellationToken ct)
    {
        await mediator.Send(new RejectTravelBatchCommand(batchId, rejectedBy, remarks), ct);
        return true;
    }

    // ── Employee Payment ─────────────────────────────────────────────────────
    public async Task<EmployeePaymentDto> CreateEmployeePaymentAsync(
        [Service] IMediator mediator,
        CreateEmployeePaymentInput input,
        CancellationToken ct)
        => await mediator.Send(new CreateEmployeePaymentCommand
        {
            PayId = input.PayId,
            PayTpId = input.PayTpId,
            PayTrnType = input.PayTrnType,
            PayEmpSysId = input.PayEmpSysId,
            PayUnitId = input.PayUnitId,
            PayMode = input.PayMode,
            PayType = input.PayType,
            PayAmount = input.PayAmount,
            PayRefId = input.PayRefId,
            PayBatchId = input.PayBatchId,
            PayJvId = input.PayJvId,
            CreatedBy = input.CreatedBy
        }, ct);

    // ── Airline Invoice ──────────────────────────────────────────────────────
    public async Task<AirlineInvoiceDto> CreateAirlineInvoiceAsync(
        [Service] IMediator mediator,
        CreateAirlineInvoiceInput input,
        CancellationToken ct)
        => await mediator.Send(new CreateAirlineInvoiceCommand
        {
            AirTicketId = input.AirTicketId,
            BookCnfId = input.BookCnfId,
            TicketNumber = input.TicketNumber,
            PnrNumber = input.PnrNumber,
            AirlineVendorId = input.AirlineVendorId,
            InvoiceNumber = input.InvoiceNumber,
            InvoiceDate = input.InvoiceDate,
            InvoiceCost = input.InvoiceCost,
            EnteredBy = input.EnteredBy,
            DebitCredit = input.DebitCredit,
            Cgst = input.Cgst,
            Sgst = input.Sgst,
            Igst = input.Igst,
            VendorGstNumber = input.VendorGstNumber
        }, ct);
}

// ── GraphQL Input Types ──────────────────────────────────────────────────────

public record CreateEmployeeJVInput(
    long JvBatchId, long JvTpId, string JvType,
    DateTime JvDate, long JvEmpSysId, string JvTrnType,
    decimal JvNetAmt, long JvPayUnitId, long CreatedBy);

public record CreateSupplierJVInput(
    long JvId, string JvType, DateTime JvDate,
    long JvVendorId, long JvPayUnitId, string JvRefInvNo,
    decimal JvNetAmt, string JvTrnType, long JvOraVendorId,
    long JvAdminId, long JvInvBatchId, long JvOraSiteId,
    string JvCenvatApplicable, string JvDocKeyNo, long CreatedBy);

public record CreateTravelBatchInput(
    string BatchId, string AdminId, string PayUnitId,
    string VendorId, string? InvNum, string? InvAmount,
    string? BatchType, string CreatedBy);

public record CreateEmployeePaymentInput(
    long PayId, long PayTpId, string PayTrnType,
    long PayEmpSysId, long PayUnitId, string PayMode,
    string PayType, decimal PayAmount, long PayRefId,
    long PayBatchId, long PayJvId, long CreatedBy);

public record CreateAirlineInvoiceInput(
    string AirTicketId, string BookCnfId, string TicketNumber,
    string? PnrNumber, string AirlineVendorId, string InvoiceNumber,
    DateTime InvoiceDate, string InvoiceCost, string EnteredBy,
    string? DebitCredit, string? Cgst, string? Sgst,
    string? Igst, string? VendorGstNumber);
