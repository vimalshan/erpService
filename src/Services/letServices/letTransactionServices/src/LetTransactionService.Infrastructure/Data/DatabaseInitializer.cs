using LetTransactionService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LetTransactionService.Infrastructure.Data;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider, ILogger logger)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<LetTransactionDbContext>();

        logger.LogInformation("Ensuring LetTransactionService database schema...");
        await context.Database.EnsureCreatedAsync();
        logger.LogInformation("Database schema verified.");

        if (!await context.LetMain.AnyAsync())
        {
            logger.LogInformation("Seeding initial LET transaction data...");
            await SeedAsync(context);
            logger.LogInformation("Seed complete.");
        }
    }

    private static async Task SeedAsync(LetTransactionDbContext context)
    {
        // Seed LET_MAIN
        var letMain = LetMain.Create(
            requestNumber: 500001,
            financialYearSerialNo: 1,
            employeeUserId: "EMP001",
            supervisorUserId: "SUP001",
            requestDate: new DateTime(2026, 4, 1, 8, 0, 0));

        letMain.AddSubEntry(
            serialNumber: 1,
            preferredModeDev: 'T',
            actionTaken: "Completed initial assessment",
            courseId: 1001,
            trainingProgramBhr: "Leadership Development Program",
            impactBenefitProcess: "Improved team management and project delivery",
            measureCompetency: "360-degree feedback and performance metrics",
            competencyToDevelop: 1,
            domainKnowledgeDev: "Project Management",
            domainKnowledgeDevDetail: "Advanced project planning and risk management",
            processDev: "Agile Methodology",
            processDevDetail: "Scrum and Kanban practices",
            letSubCode: 'A',
            reviewType: "Annual");

        await context.LetMain.AddAsync(letMain);

        // Seed COURSE_FEEDBACKMAIN
        var feedback = CourseFeedbackMain.Create(
            feedbackNumber: 600001,
            nominationNumber: 1,
            requestNumber: 500001,
            overallRating: 4,
            remarks1: "Excellent training content",
            remarks2: "Well-structured sessions",
            remarks3: "Practical approach to learning",
            totalManHours: 24);

        feedback.AddDetail(1, 4, "Good theoretical foundation");
        feedback.AddDetail(2, 5, "Excellent practical exercises");
        feedback.AddDetail(3, 4, "Well-paced delivery");

        await context.CourseFeedbackMain.AddAsync(feedback);

        // Seed REVIEW_MAIN
        var review = ReviewMain.Create(
            reviewSerialNumber: 700001,
            feedbackNumber: 600001,
            implementationGoal: "Apply project management techniques to current projects",
            keyLearning: "Risk assessment frameworks and mitigation strategies",
            keyStepsImplementation: "1. Conduct risk assessment 2. Implement mitigation plan 3. Monitor progress",
            keyOutputsExpected: "Reduced project delays by 20%",
            measurementProcess: "Monthly project status reviews",
            helpRequiredFromHr: "Mentoring support for 3 months",
            nextReviewDate: new DateTime(2026, 7, 1));

        review.AddReviewDetail(
            reviewNumber: 1,
            nextRequired: 'Y',
            reviewDate: new DateTime(2026, 4, 15),
            reviewBy: 1,
            remarks: "Initial review - implementation plan approved",
            progressRemarks: "On track with initial milestones");

        await context.ReviewMain.AddAsync(review);

        // Clear domain events from seeded entities
        letMain.ClearDomainEvents();
        feedback.ClearDomainEvents();
        review.ClearDomainEvents();

        await context.SaveChangesAsync();
    }
}
