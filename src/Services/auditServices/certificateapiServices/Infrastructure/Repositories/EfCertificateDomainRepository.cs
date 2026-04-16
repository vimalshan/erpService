using CertificateService.Domain.Entities;
using CertificateService.Domain.Interfaces;
using CertificateService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CertificateService.Infrastructure.Repositories;

public class EfCertificateDomainRepository : ICertificateDomainRepository
{
    private readonly CertificateDomainDbContext _ctx;
    public EfCertificateDomainRepository(CertificateDomainDbContext ctx) => _ctx = ctx;

    public async Task<Certificate?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _ctx.Certificates.Include(c => c.CertificateServices).Include(c => c.CertificateSites)
            .Include(c => c.AdditionalScopes).FirstOrDefaultAsync(c => c.CertificateId == id, ct);

    public async Task<IEnumerable<Certificate>> GetAllAsync(CancellationToken ct = default)
        => await _ctx.Certificates.ToListAsync(ct);

    public async Task<Certificate> AddAsync(Certificate entity, CancellationToken ct = default)
    { _ctx.Certificates.Add(entity); await _ctx.SaveChangesAsync(ct); return entity; }

    public async Task UpdateAsync(Certificate entity, CancellationToken ct = default)
    { _ctx.Certificates.Update(entity); await _ctx.SaveChangesAsync(ct); }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var e = await _ctx.Certificates.FindAsync(new object[] { id }, ct);
        if (e is not null) { _ctx.Certificates.Remove(e); await _ctx.SaveChangesAsync(ct); }
    }
}
