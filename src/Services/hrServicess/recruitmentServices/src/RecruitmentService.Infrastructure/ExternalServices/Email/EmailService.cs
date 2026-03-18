using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RecruitmentService.Application.Interfaces;

namespace RecruitmentService.Infrastructure.ExternalServices.Email;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IConfiguration _configuration;

    public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default)
    {
        // In production, wire to SendGrid / SMTP. Currently logs for dev.
        _logger.LogInformation("[EMAIL] To: {To} | Subject: {Subject}", to, subject);
        await Task.CompletedTask;
    }

    public async Task SendApplicationReceivedAsync(string to, decimal appId, decimal vacancyId, CancellationToken ct = default)
        => await SendAsync(to,
            "Application Received - Recruitment System",
            $"<p>Your application <b>{appId}</b> for vacancy <b>{vacancyId}</b> has been received successfully.</p>",
            ct);

    public async Task SendStatusChangeAsync(string to, decimal appId, string newStatus, CancellationToken ct = default)
        => await SendAsync(to,
            "Application Status Update",
            $"<p>Your application <b>{appId}</b> status has been updated to: <b>{newStatus}</b>.</p>",
            ct);
}
