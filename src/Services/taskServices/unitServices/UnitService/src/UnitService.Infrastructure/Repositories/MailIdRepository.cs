using Microsoft.EntityFrameworkCore;
using UnitService.Domain.Entities;
using UnitService.Domain.Interfaces;
using UnitService.Infrastructure.Data;

namespace UnitService.Infrastructure.Repositories;

public class MailIdRepository : IMailIdRepository
{
    private readonly UnitDbContext _context;

    public MailIdRepository(UnitDbContext context) => _context = context;

    public async Task<MailIdMaster?> GetByIdAsync(int mailId, CancellationToken ct = default)
        => await _context.MailIdMasters.FirstOrDefaultAsync(m => m.MailId == mailId, ct);

    public async Task<IEnumerable<MailIdMaster>> GetByUnitCodeAsync(string unitCode, CancellationToken ct = default)
        => await _context.MailIdMasters
            .Where(m => m.UnitCode == Domain.ValueObjects.UnitCode.From(unitCode))
            .ToListAsync(ct);

    public async Task AddAsync(MailIdMaster mail, CancellationToken ct = default)
        => await _context.MailIdMasters.AddAsync(mail, ct);

    public void Update(MailIdMaster mail)
        => _context.MailIdMasters.Update(mail);
}
