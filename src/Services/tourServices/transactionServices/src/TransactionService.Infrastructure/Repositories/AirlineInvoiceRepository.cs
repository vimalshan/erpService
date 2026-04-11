using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Interfaces;
using TransactionService.Infrastructure.Persistence;

namespace TransactionService.Infrastructure.Repositories;

public sealed class AirlineInvoiceRepository : IAirlineInvoiceRepository
{
    private readonly TransactionDbContext _context;

    public AirlineInvoiceRepository(TransactionDbContext context) => _context = context;

    public async Task<AirlineInvoice?> GetByIdAsync(string airTicketId, CancellationToken cancellationToken = default)
        => await _context.AirlineInvoices
            .FirstOrDefaultAsync(a => a.AirTicketId == airTicketId, cancellationToken);

    public async Task<IEnumerable<AirlineInvoice>> GetByBookingConfirmationIdAsync(string bookCnfId, CancellationToken cancellationToken = default)
        => await _context.AirlineInvoices
            .Where(a => a.BookCnfId == bookCnfId)
            .OrderByDescending(a => a.InvoiceDate)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<AirlineInvoice>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        => await _context.AirlineInvoices
            .OrderByDescending(a => a.InvoiceDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(AirlineInvoice invoice, CancellationToken cancellationToken = default)
        => await _context.AirlineInvoices.AddAsync(invoice, cancellationToken);

    public void Update(AirlineInvoice invoice)
        => _context.AirlineInvoices.Update(invoice);
}
