using MediatR;

namespace CertificateService.Domain.Events;

public interface IDomainEvent { DateTime OccurredOn { get; } }

public class CertificateIssuedEvent : IDomainEvent, INotification
{
    public int CertificateId { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public CertificateIssuedEvent(int certificateId) => CertificateId = certificateId;
}

public class CertificateExpiredEvent : IDomainEvent, INotification
{
    public int CertificateId { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public CertificateExpiredEvent(int certificateId) => CertificateId = certificateId;
}

public class CertificateRenewedEvent : IDomainEvent, INotification
{
    public int CertificateId { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public CertificateRenewedEvent(int certificateId) => CertificateId = certificateId;
}
