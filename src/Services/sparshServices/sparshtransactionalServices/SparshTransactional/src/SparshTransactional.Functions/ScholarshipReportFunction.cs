using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SparshTransactional.Domain.Interfaces;

namespace SparshTransactional.Functions;

public class ScholarshipReportFunction(
    IScholarshipMasterRepository scholarshipRepository,
    IScholarshipApplicationRepository applicationRepository,
    IScholarshipDisbursementRepository disbursementRepository,
    ILogger<ScholarshipReportFunction> logger)
{
    [Function("GenerateWeeklyScholarshipReport")]
    public async Task Run([TimerTrigger("0 0 6 * * 1")] TimerInfo timer) // Every Monday at 6 AM
    {
        logger.LogInformation("ScholarshipReport generation started at {Time}", DateTime.UtcNow);

        var scholarships = await scholarshipRepository.GetAllAsync();
        var applications = await applicationRepository.GetAllAsync();
        var disbursements = await disbursementRepository.GetAllAsync();

        var activeScholarships = scholarships.Count(s => s.Status == "A");
        var pendingApplications = applications.Count(a => a.ApplicationStatus == "S");
        var approvedApplications = applications.Count(a => a.ApplicationStatus == "A");
        var rejectedApplications = applications.Count(a => a.ApplicationStatus == "R");
        var pendingDisbursements = disbursements.Count(d => d.DisbursementStatus == "P");
        var completedDisbursements = disbursements.Count(d => d.DisbursementStatus == "C");
        var totalDisbursedAmount = disbursements
            .Where(d => d.DisbursementStatus == "C")
            .Sum(d => d.DisbursementAmount);

        logger.LogInformation(
            """
            === Weekly Scholarship Report ===
            Active Scholarships: {ActiveScholarships}
            Pending Applications: {PendingApps}
            Approved Applications: {ApprovedApps}
            Rejected Applications: {RejectedApps}
            Pending Disbursements: {PendingDisb}
            Completed Disbursements: {CompletedDisb}
            Total Disbursed Amount: {TotalAmount:C}
            ================================
            """,
            activeScholarships, pendingApplications, approvedApplications,
            rejectedApplications, pendingDisbursements, completedDisbursements,
            totalDisbursedAmount);

        logger.LogInformation("ScholarshipReport generation completed at {Time}", DateTime.UtcNow);
    }
}
