using Microsoft.Extensions.Logging;
using EmailNotification.Domain.Aggregates;
using EmailNotification.Domain.ValueObjects;
using EmailNotification.Domain.Entities;
using EmailNotification.Infrastructure.Data;

namespace EmailNotification.Infrastructure.Data;

/// <summary>
/// Service for seeding database with initial data
/// </summary>
public interface IDataSeeder
{
    /// <summary>Seeds the database with sample data</summary>
    Task SeedAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Implementation of data seeder
/// </summary>
public class EmailNotificationDataSeeder : IDataSeeder
{
    private readonly EmailNotificationDbContext _context;
    private readonly ILogger<EmailNotificationDataSeeder> _logger;

    public EmailNotificationDataSeeder(EmailNotificationDbContext context, ILogger<EmailNotificationDataSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>Seeds the database with sample email types and recipients</summary>
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if data already exists
            if (_context.EmailTypes.Any())
            {
                _logger.LogInformation("Database already contains data, skipping seed");
                return;
            }

            _logger.LogInformation("Starting database seeding...");

            // Create sample email types
            var dailyTreasuryEmail = new EmailTypeAggregate(
                emailName: "Daily Treasury Report",
                emailType: EmailTypeEnum.Daily,
                emailProcName: "usp_GenerateTreasuryReport",
                createdBy: 1);

            var tradeConfirmationEmail = new EmailTypeAggregate(
                emailName: "Trade Confirmation",
                emailType: EmailTypeEnum.Event,
                emailProcName: "usp_SendTradeConfirmation",
                createdBy: 1);

            var settlementReportEmail = new EmailTypeAggregate(
                emailName: "Daily Settlement Report",
                emailType: EmailTypeEnum.Daily,
                emailProcName: "usp_GenerateSettlementReport",
                createdBy: 1);

            var eventAlertEmail = new EmailTypeAggregate(
                emailName: "Critical Event Alert",
                emailType: EmailTypeEnum.Event,
                emailProcName: "usp_GenerateEventAlert",
                createdBy: 1);

            // Add email types to context
            _context.EmailTypes.AddRange(
                dailyTreasuryEmail,
                tradeConfirmationEmail,
                settlementReportEmail,
                eventAlertEmail);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Created {Count} email types", 4);

            // Create sample recipients for daily treasury email (ID: from context)
            var treasuryEmailTypeId = dailyTreasuryEmail.Id;

            // Global recipients (null org/business = all)
            var globalRecipients = new[]
            {
                new MailAccess(
                    mailTypeId: treasuryEmailTypeId,
                    mailEmail: new EmailAddress("treasury@bankxyz.com"),
                    createdBy: 1,
                    mailName: "Treasury Team"),

                new MailAccess(
                    mailTypeId: treasuryEmailTypeId,
                    mailEmail: new EmailAddress("cfo@bankxyz.com"),
                    createdBy: 1,
                    mailName: "CFO")
            };

            // Organization 1 recipients
            var org1Recipients = new[]
            {
                new MailAccess(
                    mailTypeId: treasuryEmailTypeId,
                    mailEmail: new EmailAddress("manager.org1@bankxyz.com"),
                    createdBy: 1,
                    mailOrgId: 1,
                    mailName: "Organization 1 Manager"),

                new MailAccess(
                    mailTypeId: treasuryEmailTypeId,
                    mailEmail: new EmailAddress("treasury.org1@bankxyz.com"),
                    createdBy: 1,
                    mailOrgId: 1,
                    mailBusinessId: 1,
                    mailName: "Organization 1 , Business Unit 1 Treasury")
            };

            // Organization 2 recipients
            var org2Recipients = new[]
            {
                new MailAccess(
                    mailTypeId: treasuryEmailTypeId,
                    mailEmail: new EmailAddress("manager.org2@bankxyz.com"),
                    createdBy: 1,
                    mailOrgId: 2,
                    mailName: "Organization 2 Manager"),

                new MailAccess(
                    mailTypeId: treasuryEmailTypeId,
                    mailEmail: new EmailAddress("treasury.org2.bu1@bankxyz.com"),
                    createdBy: 1,
                    mailOrgId: 2,
                    mailBusinessId: 1,
                    mailName: "Organization 2, Business Unit 1 Treasury"),

                new MailAccess(
                    mailTypeId: treasuryEmailTypeId,
                    mailEmail: new EmailAddress("treasury.org2.bu2@bankxyz.com"),
                    createdBy: 1,
                    mailOrgId: 2,
                    mailBusinessId: 2,
                    mailName: "Organization 2, Business Unit 2 Treasury")
            };

            // Add all recipients
            _context.MailAccesses.AddRange(globalRecipients);
            _context.MailAccesses.AddRange(org1Recipients);
            _context.MailAccesses.AddRange(org2Recipients);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Created {Count} recipients for Daily Treasury Report", 
                globalRecipients.Length + org1Recipients.Length + org2Recipients.Length);

            // Create recipients for trade confirmation email
            var tradeEmailTypeId = tradeConfirmationEmail.Id;
            var tradeRecipients = new[]
            {
                new MailAccess(
                    mailTypeId: tradeEmailTypeId,
                    mailEmail: new EmailAddress("trading@bankxyz.com"),
                    createdBy: 1,
                    mailName: "Trading Desk"),

                new MailAccess(
                    mailTypeId: tradeEmailTypeId,
                    mailEmail: new EmailAddress("compliance@bankxyz.com"),
                    createdBy: 1,
                    mailName: "Compliance Team")
            };

            _context.MailAccesses.AddRange(tradeRecipients);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Created {Count} recipients for Trade Confirmation", tradeRecipients.Length);

            _logger.LogInformation("Database seeding completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while seeding database");
            throw;
        }
    }
}
