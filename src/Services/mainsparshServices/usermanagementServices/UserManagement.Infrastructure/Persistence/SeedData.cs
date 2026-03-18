using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Persistence;

/// <summary>
/// EF Core seed data for USER_POLICY and WEBSITE_CON_MAILID tables.
/// Uses anonymous objects because entity properties have private setters.
/// Negative IDs prevent conflicts with identity-column sequences.
/// </summary>
public static class SeedData
{
    private static readonly DateTime SeedDate = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedUserPolicies(modelBuilder);
        SeedWebsiteContacts(modelBuilder);
    }

    // ------------------------------------------------------------------ //
    //  USER_POLICY  (UQ constraint on UserSysId: one policy per user)     //
    // ------------------------------------------------------------------ //
    private static void SeedUserPolicies(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserPolicy>().HasData(
            new
            {
                PolicyId          = -1L,
                UserSysId         = 1001L,
                PolicyCode        = "SECURITY_DEFAULT",
                PolicyType        = (string?)"SECURITY",
                DataRetentionDays = (int?)365,
                SessionTimeoutMins= (int?)30,
                MaxLoginAttempts  = (int?)5,
                PolicyStatus      = 'A',
                EffectiveFrom     = new DateOnly(2025, 1, 1),
                EffectiveTo       = (DateOnly?)null,
                CreatedBy         = 1L,
                CreatedOn         = SeedDate,
                UpdatedBy         = (long?)null,
                UpdatedOn         = (DateTime?)null
            },
            new
            {
                PolicyId          = -2L,
                UserSysId         = 1002L,
                PolicyCode        = "NOTIFICATION_EMAIL",
                PolicyType        = (string?)"NOTIFICATION",
                DataRetentionDays = (int?)180,
                SessionTimeoutMins= (int?)60,
                MaxLoginAttempts  = (int?)3,
                PolicyStatus      = 'A',
                EffectiveFrom     = new DateOnly(2025, 1, 1),
                EffectiveTo       = (DateOnly?)null,
                CreatedBy         = 1L,
                CreatedOn         = SeedDate,
                UpdatedBy         = (long?)null,
                UpdatedOn         = (DateTime?)null
            },
            new
            {
                PolicyId          = -3L,
                UserSysId         = 1003L,
                PolicyCode        = "PREFERENCES_DARK",
                PolicyType        = (string?)"PREFERENCES",
                DataRetentionDays = (int?)null,
                SessionTimeoutMins= (int?)null,
                MaxLoginAttempts  = (int?)null,
                PolicyStatus      = 'A',
                EffectiveFrom     = new DateOnly(2025, 1, 1),
                EffectiveTo       = (DateOnly?)null,
                CreatedBy         = 1L,
                CreatedOn         = SeedDate,
                UpdatedBy         = (long?)null,
                UpdatedOn         = (DateTime?)null
            },
            new
            {
                PolicyId          = -4L,
                UserSysId         = 1004L,
                PolicyCode        = "ACCESS_CONTROL_ADMIN",
                PolicyType        = (string?)"ACCESS_CONTROL",
                DataRetentionDays = (int?)730,
                SessionTimeoutMins= (int?)15,
                MaxLoginAttempts  = (int?)3,
                PolicyStatus      = 'A',
                EffectiveFrom     = new DateOnly(2025, 1, 1),
                EffectiveTo       = (DateOnly?)null,
                CreatedBy         = 1L,
                CreatedOn         = SeedDate,
                UpdatedBy         = (long?)null,
                UpdatedOn         = (DateTime?)null
            }
        );
    }

    // ------------------------------------------------------------------ //
    //  WEBSITE_CON_MAILID                                                  //
    // ------------------------------------------------------------------ //
    private static void SeedWebsiteContacts(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WebsiteContactEmail>().HasData(
            new
            {
                ContactId       = -1L,
                UserSysId       = 1001L,
                PrimaryEmail    = "admin@sparsh.local",
                SecondaryEmail  = (string?)"admin-backup@sparsh.local",
                Phone           = (string?)null,
                Mobile          = (string?)"+91-9000000001",
                Website         = (string?)"https://sparsh.local",
                SocialMedia     = (string?)null,
                NewsletterOptIn = 'Y',
                ContactStatus   = 'A',
                CreatedBy       = 1L,
                CreatedOn       = SeedDate,
                UpdatedBy       = (long?)null,
                UpdatedOn       = (DateTime?)null
            },
            new
            {
                ContactId       = -2L,
                UserSysId       = 1002L,
                PrimaryEmail    = "user2@sparsh.local",
                SecondaryEmail  = (string?)null,
                Phone           = (string?)"+91-22-12345678",
                Mobile          = (string?)"+91-9000000002",
                Website         = (string?)null,
                SocialMedia     = (string?)null,
                NewsletterOptIn = 'N',
                ContactStatus   = 'A',
                CreatedBy       = 1L,
                CreatedOn       = SeedDate,
                UpdatedBy       = (long?)null,
                UpdatedOn       = (DateTime?)null
            },
            new
            {
                ContactId       = -3L,
                UserSysId       = 1003L,
                PrimaryEmail    = "user3@sparsh.local",
                SecondaryEmail  = (string?)null,
                Phone           = (string?)null,
                Mobile          = (string?)"+91-9000000003",
                Website         = (string?)null,
                SocialMedia     = (string?)"@user3_sparsh",
                NewsletterOptIn = 'Y',
                ContactStatus   = 'A',
                CreatedBy       = 1L,
                CreatedOn       = SeedDate,
                UpdatedBy       = (long?)null,
                UpdatedOn       = (DateTime?)null
            }
        );
    }
}

