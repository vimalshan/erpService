namespace RecruitmentService.Application.Interfaces;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default);
    Task SendApplicationReceivedAsync(string to, decimal appId, decimal vacancyId, CancellationToken ct = default);
    Task SendStatusChangeAsync(string to, decimal appId, string newStatus, CancellationToken ct = default);
}
