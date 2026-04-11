using TransactionService.Domain.Aggregates;
using TransactionService.Domain.Entities;

namespace TransactionService.Domain.Interfaces;

public interface IEmployeeJVRepository
{
    Task<EmployeeJournalVoucher?> GetByIdAsync(long jvBatchId, CancellationToken cancellationToken = default);
    Task<IEnumerable<EmployeeJournalVoucher>> GetByEmployeeIdAsync(long empSysId, CancellationToken cancellationToken = default);
    Task<IEnumerable<EmployeeJournalVoucher>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);
    Task AddAsync(EmployeeJournalVoucher jv, CancellationToken cancellationToken = default);
    void Update(EmployeeJournalVoucher jv);
    Task<bool> ExistsAsync(long jvBatchId, CancellationToken cancellationToken = default);
}

public interface ISupplierJVRepository
{
    Task<SupplierJournalVoucher?> GetByIdAsync(long jvId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SupplierJournalVoucher>> GetByVendorIdAsync(long vendorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SupplierJournalVoucher>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);
    Task AddAsync(SupplierJournalVoucher jv, CancellationToken cancellationToken = default);
    void Update(SupplierJournalVoucher jv);
    Task<bool> ExistsAsync(long jvId, CancellationToken cancellationToken = default);
}

public interface ITravelBatchRepository
{
    Task<TravelBatch?> GetByIdAsync(string batchId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TravelBatch>> GetByVendorIdAsync(string vendorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TravelBatch>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<IEnumerable<TravelBatch>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);
    Task AddAsync(TravelBatch batch, CancellationToken cancellationToken = default);
    void Update(TravelBatch batch);
    Task<bool> ExistsAsync(string batchId, CancellationToken cancellationToken = default);
}

public interface IEmployeePaymentRepository
{
    Task<EmployeePayment?> GetByIdAsync(long payId, CancellationToken cancellationToken = default);
    Task<IEnumerable<EmployeePayment>> GetByEmployeeIdAsync(long empSysId, CancellationToken cancellationToken = default);
    Task<IEnumerable<EmployeePayment>> GetByTourPlanIdAsync(long tpId, CancellationToken cancellationToken = default);
    Task AddAsync(EmployeePayment payment, CancellationToken cancellationToken = default);
}

public interface IAirlineInvoiceRepository
{
    Task<AirlineInvoice?> GetByIdAsync(string airTicketId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AirlineInvoice>> GetByBookingConfirmationIdAsync(string bookCnfId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AirlineInvoice>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task AddAsync(AirlineInvoice invoice, CancellationToken cancellationToken = default);
    void Update(AirlineInvoice invoice);
}
