using FaqServices.Domain.Entities;
using FaqServices.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FaqServices.Infrastructure.Migrations;

public class DatabaseInitializer
{
    public static async Task InitializeAsync(FaqDbContext context)
    {
        try
        {
            // Apply any pending migrations
            await context.Database.MigrateAsync();

            // Seed data if needed
            await SeedDataAsync(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing database: {ex.Message}");
            throw;
        }
    }

    private static async Task SeedDataAsync(FaqDbContext context)
    {
        // Only seed if there's no data
        if (await context.FaqGrades.AnyAsync())
        {
            return;
        }

        const string systemUser = "SYSTEM";
        
        // Create grades
        var grade1 = FaqGrade.Create("Grade 1", "First Grade FAQ", 1, systemUser);
        var grade2 = FaqGrade.Create("Grade 2", "Second Grade FAQ", 2, systemUser);
        var grade3 = FaqGrade.Create("Grade 3", "Third Grade FAQ", 3, systemUser);

        context.FaqGrades.AddRange(grade1, grade2, grade3);
        await context.SaveChangesAsync();

        // Create questions for Grade 1
        var question1 = FaqQuestion.Create(
            grade1.PK,
            "What is FAQ?",
            "ما هي الأسئلة الشائعة؟",
            1,
            systemUser
        );
        
        var question2 = FaqQuestion.Create(
            grade1.PK,
            "How to use this system?",
            "كيفية استخدام هذا النظام؟",
            2,
            systemUser
        );

        var question3 = FaqQuestion.Create(
            grade1.PK,
            "What are the key features?",
            "ما هي الميزات الرئيسية؟",
            3,
            systemUser
        );

        // Create questions for Grade 2
        var question4 = FaqQuestion.Create(
            grade2.PK,
            "What are the requirements?",
            "ما هي المتطلبات؟",
            1,
            systemUser
        );

        var question5 = FaqQuestion.Create(
            grade2.PK,
            "How to get started?",
            "كيف تبدأ؟",
            2,
            systemUser
        );

        context.FaqQuestions.AddRange(question1, question2, question3, question4, question5);
        await context.SaveChangesAsync();

        // Create answers for Question 1
        var answer1 = FaqAnswer.Create(
            question1.PK,
            "FAQ stands for Frequently Asked Questions. It's a section containing answers to common questions.",
            "FAQ تعني الأسئلة الشائعة. إنها قسم يحتوي على إجابات للأسئلة الشائعة.",
            true,
            1,
            systemUser
        );

        var answer2 = FaqAnswer.Create(
            question1.PK,
            "FAQ is an acronym for Frequently Asked Questions",
            "FAQ هو اختصار يعني الأسئلة الشائعة",
            true,
            2,
            systemUser
        );

        // Create answers for Question 2
        var answer3 = FaqAnswer.Create(
            question2.PK,
            "Follow the documentation and user guides provided in the system",
            "اتبع التوثيق ودليل المستخدم المقدم في النظام",
            true,
            1,
            systemUser
        );

        var answer4 = FaqAnswer.Create(
            question2.PK,
            "Watch the tutorial videos available in the help section",
            "شاهد مقاطع الفيديو التعليمية المتاحة في قسم المساعدة",
            true,
            2,
            systemUser
        );

        // Create answers for Question 3
        var answer5 = FaqAnswer.Create(
            question3.PK,
            "The system includes CQRS pattern, GraphQL API, REST endpoints, and JWT authentication",
            "يتضمن النظام نمط CQRS وواجهة GraphQL وواجهات REST والمصادقة JWT",
            true,
            1,
            systemUser
        );

        // Create answers for Question 4
        var answer6 = FaqAnswer.Create(
            question4.PK,
            ".NET 10.0 or higher, SQL Server 2019 or higher, and 2GB RAM minimum",
            ".NET 10.0 أو أعلى وSQL Server 2019 أو أعلى و2GB RAM على الأقل",
            true,
            1,
            systemUser
        );

        // Create answers for Question 5
        var answer7 = FaqAnswer.Create(
            question5.PK,
            "Clone the repository, install dependencies, configure the database connection, and run migrations",
            "استنسخ المستودع وقم بتثبيت التبعيات وقم بتكوين اتصال قاعدة البيانات وقم بتشغيل الترحيلات",
            true,
            1,
            systemUser
        );

        context.FaqAnswers.AddRange(
            answer1, answer2, answer3, answer4, answer5, answer6, answer7
        );

        await context.SaveChangesAsync();

        Console.WriteLine("Database seeding completed successfully.");
    }
}
