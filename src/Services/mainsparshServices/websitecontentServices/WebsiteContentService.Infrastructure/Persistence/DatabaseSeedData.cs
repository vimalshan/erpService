namespace WebsiteContentService.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using WebsiteContentService.Domain.Entities;

public static class DatabaseSeedData
{
    public static async Task SeedAsync(WebsiteContentDbContext context)
    {
        if (await context.WebsitePages.AnyAsync())
            return;

        var pages = new List<WebsitePage>
        {
            WebsitePage.Create("HOME", "Home Page", "<h1>Welcome to SPARSH</h1><p>Your portal for all services.</p>",
                "SPARSH Home Page", "sparsh,home,portal", 1, null, 1),
            WebsitePage.Create("ABOUT", "About Us", "<h1>About SPARSH</h1><p>Learn about our mission and values.</p>",
                "About SPARSH", "about,mission,values", 2, null, 1),
            WebsitePage.Create("CONTACT", "Contact Us", "<h1>Contact</h1><p>Reach out to us for any queries.</p>",
                "Contact SPARSH", "contact,support,help", 3, null, 1),
            WebsitePage.Create("FAQ", "Frequently Asked Questions", "<h1>FAQ</h1><p>Common questions and answers.</p>",
                "FAQ", "faq,questions,help", 4, null, 1),
            WebsitePage.Create("PRIVACY", "Privacy Policy", "<h1>Privacy Policy</h1><p>Our data handling practices.</p>",
                "Privacy Policy", "privacy,policy,data", 5, null, 1)
        };

        await context.WebsitePages.AddRangeAsync(pages);

        var newsItems = new List<WebsiteNews>
        {
            WebsiteNews.Create("New SPARSH Portal Launch", "We are excited to announce the launch of the new SPARSH portal with enhanced features and improved user experience.",
                "New portal launch announcement", "ANNOUNCEMENT", null, DateTime.UtcNow, DateTime.UtcNow.AddMonths(6), 1),
            WebsiteNews.Create("System Maintenance Scheduled", "Scheduled maintenance will be performed on the portal this weekend. Services may be temporarily unavailable.",
                "Maintenance notification", "MAINTENANCE", null, DateTime.UtcNow, DateTime.UtcNow.AddDays(7), 1),
            WebsiteNews.Create("Employee Benefits Update", "Updated benefits packages are now available for all employees. Please review the changes in your profile.",
                "Benefits update notice", "HR", null, DateTime.UtcNow, DateTime.UtcNow.AddMonths(3), 1),
            WebsiteNews.Create("Quarterly Town Hall Meeting", "Join us for the quarterly town hall meeting to discuss organizational updates and future plans.",
                "Quarterly meeting invitation", "EVENT", null, DateTime.UtcNow.AddDays(14), DateTime.UtcNow.AddDays(15), 1),
            WebsiteNews.Create("Security Best Practices", "Review our updated security guidelines to protect your account and sensitive information.",
                "Security guidelines", "SECURITY", null, DateTime.UtcNow, null, 1)
        };

        await context.WebsiteNews.AddRangeAsync(newsItems);
        await context.SaveChangesAsync();
    }
}
