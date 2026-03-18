using ExitManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExitManagement.Infrastructure.Persistence.Seeds;

public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, ILogger? logger = null)
    {
        // Apply any pending EF migrations (creates tables if they don't exist)
        await context.Database.MigrateAsync();

        if (!await context.ExitQuestions.AnyAsync())
        {
            // Seed EXIT QUESTIONS
            var sql = @"
IF NOT EXISTS (SELECT 1 FROM TT_EXIT_QUESTIONS WHERE QUESTION_ID = 'Q001')
BEGIN
    INSERT INTO TT_EXIT_QUESTIONS (QUESTION_ID, QUESTION_DESC, QUESTION_ORDER)
    VALUES
        ('Q001', 'What was the primary reason for your decision to leave?', 1),
        ('Q002', 'How would you rate your overall experience at the company?', 2),
        ('Q003', 'Were you satisfied with your job responsibilities?', 3),
        ('Q004', 'How would you rate your relationship with your direct supervisor?', 4),
        ('Q005', 'Did you feel your compensation was competitive with the market?', 5),
        ('Q006', 'Were you given adequate opportunities for career growth?', 6),
        ('Q007', 'How would you describe your work environment?', 7),
        ('Q008', 'Would you recommend this company to others?', 8),
        ('Q009', 'Is there anything the company could have done to retain you?', 9),
        ('Q010', 'Any additional comments or suggestions for improvement?', 10)
END";
            await context.Database.ExecuteSqlRawAsync(sql);
        }

        if (!await context.ExitInterviewQuestions.AnyAsync())
        {
            var sql = @"
IF NOT EXISTS (SELECT 1 FROM TT_EXIT_INTERVIEW WHERE QUESTION_ID = 'I01')
BEGIN
    INSERT INTO TT_EXIT_INTERVIEW (QUESTION_ID, QUESTION_DESC, ORDER_ID)
    VALUES
        ('I01', 'What prompted you to start looking for a new job?', 1),
        ('I02', 'What did you enjoy most about your job?', 2),
        ('I03', 'What did you enjoy least about your job?', 3),
        ('I04', 'How could management have improved your experience?', 4),
        ('I05', 'What does your new position offer that influenced your decision?', 5)
END";
            await context.Database.ExecuteSqlRawAsync(sql);
        }
    }
}
