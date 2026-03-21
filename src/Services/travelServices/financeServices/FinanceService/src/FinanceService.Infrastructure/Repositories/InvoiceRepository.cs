using Dapper;
using FinanceService.Application.Common.Interfaces;
using FinanceService.Application.DTOs;
using FinanceService.Domain.Entities;

namespace FinanceService.Infrastructure.Repositories;

public class InvoiceRepository
{
    private readonly IDapperContext _dapperContext;

    public InvoiceRepository(IDapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync()
    {
        using var connection = _dapperContext.CreateConnection();
        const string sql = @"
            SELECT INVOICE_ID AS InvoiceId, INVOICE_NUM AS InvoiceNum, 
                   INVOICE_TYPE_LOOKUP_CODE AS InvoiceTypeLookupCode,
                   INVOICE_DATE AS InvoiceDate, VENDOR_ID AS VendorId,
                   INVOICE_AMOUNT AS InvoiceAmount, INVOICE_CURRENCY_CODE AS InvoiceCurrencyCode,
                   DESCRIPTION AS Description, STATUS AS Status, AGENCY_ID AS AgencyId
            FROM AP_INVOICES_INTERFACE";
        return await connection.QueryAsync<InvoiceDto>(sql);
    }

    public async Task<InvoiceDto?> GetInvoiceByIdAsync(long invoiceId)
    {
        using var connection = _dapperContext.CreateConnection();
        const string sql = @"
            SELECT INVOICE_ID AS InvoiceId, INVOICE_NUM AS InvoiceNum,
                   INVOICE_TYPE_LOOKUP_CODE AS InvoiceTypeLookupCode,
                   INVOICE_DATE AS InvoiceDate, VENDOR_ID AS VendorId,
                   INVOICE_AMOUNT AS InvoiceAmount, INVOICE_CURRENCY_CODE AS InvoiceCurrencyCode,
                   DESCRIPTION AS Description, STATUS AS Status, AGENCY_ID AS AgencyId
            FROM AP_INVOICES_INTERFACE
            WHERE INVOICE_ID = @InvoiceId";
        return await connection.QueryFirstOrDefaultAsync<InvoiceDto>(sql, new { InvoiceId = invoiceId });
    }
}
