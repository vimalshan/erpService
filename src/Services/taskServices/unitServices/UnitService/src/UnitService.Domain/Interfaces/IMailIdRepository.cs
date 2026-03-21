using UnitService.Domain.Entities;

namespace UnitService.Domain.Interfaces;

public interface IMailIdRepository
{
    Task<MailIdMaster?> GetByIdAsync(int mailId, CancellationToken ct = default);
    Task<IEnumerable<MailIdMaster>> GetByUnitCodeAsync(string unitCode, CancellationToken ct = default);
    Task AddAsync(MailIdMaster mail, CancellationToken ct = default);
    void Update(MailIdMaster mail);
}
