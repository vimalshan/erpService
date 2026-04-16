using CertificateService.Domain.Entities;

namespace CertificateService.Domain.Interfaces;

public interface ICertificateDomainRepository
{
    Task<Certificate?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<Certificate>> GetAllAsync(CancellationToken ct = default);
    Task<Certificate> AddAsync(Certificate entity, CancellationToken ct = default);
    Task UpdateAsync(Certificate entity, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
