using CSA.Service.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CSA.Service.Infrastructure.Data;

public static class CsaDbContextSeed
{
    public static async Task SeedAsync(CsaDbContext context)
    {
        await context.Database.MigrateAsync();

        var now = DateTime.UtcNow;

        // ── Processes ──
        if (!await context.Processes.AnyAsync())
        {
            context.Processes.AddRange(
                new Process { ProcessId = 1, Name = "Financial Reporting", CreatedBy = 1, CreatedOn = now },
                new Process { ProcessId = 2, Name = "IT Operations", CreatedBy = 1, CreatedOn = now },
                new Process { ProcessId = 3, Name = "Human Resources", CreatedBy = 1, CreatedOn = now },
                new Process { ProcessId = 4, Name = "Procurement", CreatedBy = 1, CreatedOn = now },
                new Process { ProcessId = 5, Name = "Treasury", CreatedBy = 1, CreatedOn = now },
                new Process { ProcessId = 6, Name = "Compliance", CreatedBy = 1, CreatedOn = now }
            );
            await context.SaveChangesAsync();
        }

        // ── Sub-Processes ──
        if (!await context.SubProcesses.AnyAsync())
        {
            context.SubProcesses.AddRange(
                new SubProcess { SubProcessId = 1, ProcessId = 1, Name = "General Ledger", CreatedBy = 1, CreatedOn = now },
                new SubProcess { SubProcessId = 2, ProcessId = 1, Name = "Accounts Payable", CreatedBy = 1, CreatedOn = now },
                new SubProcess { SubProcessId = 3, ProcessId = 1, Name = "Accounts Receivable", CreatedBy = 1, CreatedOn = now },
                new SubProcess { SubProcessId = 4, ProcessId = 1, Name = "Fixed Assets", CreatedBy = 1, CreatedOn = now },
                new SubProcess { SubProcessId = 5, ProcessId = 2, Name = "Access Management", CreatedBy = 1, CreatedOn = now },
                new SubProcess { SubProcessId = 6, ProcessId = 2, Name = "Change Management", CreatedBy = 1, CreatedOn = now },
                new SubProcess { SubProcessId = 7, ProcessId = 2, Name = "Incident Management", CreatedBy = 1, CreatedOn = now },
                new SubProcess { SubProcessId = 8, ProcessId = 2, Name = "Backup & Recovery", CreatedBy = 1, CreatedOn = now },
                new SubProcess { SubProcessId = 9, ProcessId = 3, Name = "Payroll Processing", CreatedBy = 1, CreatedOn = now },
                new SubProcess { SubProcessId = 10, ProcessId = 3, Name = "Leave Management", CreatedBy = 1, CreatedOn = now },
                new SubProcess { SubProcessId = 11, ProcessId = 4, Name = "Vendor Management", CreatedBy = 1, CreatedOn = now },
                new SubProcess { SubProcessId = 12, ProcessId = 4, Name = "Purchase Orders", CreatedBy = 1, CreatedOn = now },
                new SubProcess { SubProcessId = 13, ProcessId = 5, Name = "Cash Management", CreatedBy = 1, CreatedOn = now },
                new SubProcess { SubProcessId = 14, ProcessId = 5, Name = "Bank Reconciliation", CreatedBy = 1, CreatedOn = now },
                new SubProcess { SubProcessId = 15, ProcessId = 6, Name = "Regulatory Reporting", CreatedBy = 1, CreatedOn = now },
                new SubProcess { SubProcessId = 16, ProcessId = 6, Name = "AML Compliance", CreatedBy = 1, CreatedOn = now }
            );
            await context.SaveChangesAsync();
        }

        // ── Units ──
        if (!await context.Units.AnyAsync())
        {
            context.Units.AddRange(
                new Unit { UnitId = 1, Name = "Head Office", ShortName = "HO", Code = "HO ", BusinessId = 1, LiveFlag = 'Y', OrgId = 1, CreatedBy = 1, CreatedOn = now },
                new Unit { UnitId = 2, Name = "Branch Office - Dubai", ShortName = "DXB", Code = "DXB", BusinessId = 1, LiveFlag = 'Y', OrgId = 1, CreatedBy = 1, CreatedOn = now },
                new Unit { UnitId = 3, Name = "Branch Office - Abu Dhabi", ShortName = "AUH", Code = "AUH", BusinessId = 1, LiveFlag = 'Y', OrgId = 1, CreatedBy = 1, CreatedOn = now },
                new Unit { UnitId = 4, Name = "Operations Center", ShortName = "OPS", Code = "OPS", BusinessId = 1, LiveFlag = 'Y', OrgId = 1, CreatedBy = 1, CreatedOn = now },
                new Unit { UnitId = 5, Name = "IT Center", ShortName = "ITC", Code = "ITC", BusinessId = 2, LiveFlag = 'Y', OrgId = 1, CreatedBy = 1, CreatedOn = now }
            );
            await context.SaveChangesAsync();
        }

        // ── Controls ──
        if (!await context.Controls.AnyAsync())
        {
            context.Controls.AddRange(
                new Control
                {
                    ControlId = 1, Title = "Journal Entry Review", Description = "Review of all manual journal entries for proper authorization and supporting documentation",
                    ControlType = 'D', ControlMethod = 'M', Risk = "Unauthorized or erroneous journal entries affecting financial statements",
                    Priority = 'H', ProcessId = 1, SubProcessId = 1, Periodicity = 'M', EvidenceFlag = 'Y', ApproverFlag = 'Y',
                    CreatedBy = 1, CreatedOn = now
                },
                new Control
                {
                    ControlId = 2, Title = "Access Control Review", Description = "Periodic review of system access rights and user privileges",
                    ControlType = 'P', ControlMethod = 'M', Risk = "Unauthorized system access leading to data breach or fraud",
                    Priority = 'H', ProcessId = 2, SubProcessId = 5, Periodicity = 'Q', EvidenceFlag = 'Y', ApproverFlag = 'Y',
                    CreatedBy = 1, CreatedOn = now
                },
                new Control
                {
                    ControlId = 3, Title = "Vendor Invoice Verification", Description = "Three-way matching of purchase order, goods receipt, and invoice before payment",
                    ControlType = 'P', ControlMethod = 'A', Risk = "Duplicate or fraudulent payments to vendors",
                    Priority = 'H', ProcessId = 4, SubProcessId = 12, Periodicity = 'M', EvidenceFlag = 'Y', ApproverFlag = 'Y',
                    CreatedBy = 1, CreatedOn = now
                },
                new Control
                {
                    ControlId = 4, Title = "Bank Reconciliation", Description = "Monthly reconciliation of bank statements with general ledger balances",
                    ControlType = 'D', ControlMethod = 'M', Risk = "Unreconciled differences leading to misstatement of cash balances",
                    Priority = 'H', ProcessId = 5, SubProcessId = 14, Periodicity = 'M', EvidenceFlag = 'Y', ApproverFlag = 'Y',
                    CreatedBy = 1, CreatedOn = now
                },
                new Control
                {
                    ControlId = 5, Title = "Payroll Processing Review", Description = "Review and approval of payroll calculations before disbursement",
                    ControlType = 'P', ControlMethod = 'M', Risk = "Incorrect salary payments or ghost employees",
                    Priority = 'M', ProcessId = 3, SubProcessId = 9, Periodicity = 'M', EvidenceFlag = 'Y', ApproverFlag = 'Y',
                    CreatedBy = 1, CreatedOn = now
                },
                new Control
                {
                    ControlId = 6, Title = "Change Management Approval", Description = "All IT changes must follow the change management process with proper CAB approval",
                    ControlType = 'P', ControlMethod = 'M', Risk = "Unauthorized changes causing system outages or security vulnerabilities",
                    Priority = 'H', ProcessId = 2, SubProcessId = 6, Periodicity = 'M', EvidenceFlag = 'Y', ApproverFlag = 'N',
                    CreatedBy = 1, CreatedOn = now
                },
                new Control
                {
                    ControlId = 7, Title = "Backup Verification", Description = "Verify daily backup completion and perform quarterly restoration tests",
                    ControlType = 'D', ControlMethod = 'A', Risk = "Data loss due to backup failures",
                    Priority = 'M', ProcessId = 2, SubProcessId = 8, Periodicity = 'Q', EvidenceFlag = 'Y', ApproverFlag = 'N',
                    CreatedBy = 1, CreatedOn = now
                },
                new Control
                {
                    ControlId = 8, Title = "Fixed Asset Physical Verification", Description = "Annual physical verification of fixed assets against the asset register",
                    ControlType = 'D', ControlMethod = 'M', Risk = "Misappropriation or loss of fixed assets",
                    Priority = 'L', ProcessId = 1, SubProcessId = 4, Periodicity = 'A', EvidenceFlag = 'Y', ApproverFlag = 'Y',
                    CreatedBy = 1, CreatedOn = now
                },
                new Control
                {
                    ControlId = 9, Title = "AML Transaction Monitoring", Description = "Automated and manual monitoring of transactions for suspicious activities",
                    ControlType = 'D', ControlMethod = 'A', Risk = "Non-compliance with AML regulations and potential fines",
                    Priority = 'H', ProcessId = 6, SubProcessId = 16, Periodicity = 'M', EvidenceFlag = 'Y', ApproverFlag = 'Y',
                    CreatedBy = 1, CreatedOn = now
                },
                new Control
                {
                    ControlId = 10, Title = "Vendor Master Data Review", Description = "Periodic review of vendor master data for accuracy and dormant vendor identification",
                    ControlType = 'D', ControlMethod = 'M', Risk = "Fraudulent vendor creation or unauthorized changes to bank details",
                    Priority = 'M', ProcessId = 4, SubProcessId = 11, Periodicity = 'Q', EvidenceFlag = 'N', ApproverFlag = 'N',
                    CreatedBy = 1, CreatedOn = now
                }
            );
            await context.SaveChangesAsync();
        }

        // ── Evidence ──
        if (!await context.Evidences.AnyAsync())
        {
            context.Evidences.AddRange(
                new Evidence { EvidenceId = 1, ControlId = 1, Name = "Journal Entry Approval Report", TempName = "je_approval_report.pdf" },
                new Evidence { EvidenceId = 2, ControlId = 1, Name = "Supporting Documentation Checklist", TempName = "je_supporting_docs.xlsx" },
                new Evidence { EvidenceId = 3, ControlId = 2, Name = "Access Review Sign-Off", TempName = "access_review_signoff.pdf" },
                new Evidence { EvidenceId = 4, ControlId = 2, Name = "User Access Matrix", TempName = "user_access_matrix.xlsx" },
                new Evidence { EvidenceId = 5, ControlId = 3, Name = "Three-Way Match Report", TempName = "three_way_match.pdf" },
                new Evidence { EvidenceId = 6, ControlId = 4, Name = "Bank Reconciliation Statement", TempName = "bank_recon_stmt.pdf" },
                new Evidence { EvidenceId = 7, ControlId = 5, Name = "Payroll Approval Sheet", TempName = "payroll_approval.pdf" },
                new Evidence { EvidenceId = 8, ControlId = 6, Name = "CAB Meeting Minutes", TempName = "cab_minutes.pdf" },
                new Evidence { EvidenceId = 9, ControlId = 7, Name = "Backup Completion Log", TempName = "backup_log.csv" },
                new Evidence { EvidenceId = 10, ControlId = 7, Name = "Restoration Test Report", TempName = "restore_test.pdf" },
                new Evidence { EvidenceId = 11, ControlId = 8, Name = "Fixed Asset Count Sheet", TempName = "fa_count_sheet.xlsx" },
                new Evidence { EvidenceId = 12, ControlId = 9, Name = "STR Filed Report", TempName = "str_report.pdf" }
            );
            await context.SaveChangesAsync();
        }

        // ── Unit Map Details ──
        if (!await context.UnitMapDetails.AnyAsync())
        {
            context.UnitMapDetails.AddRange(
                new UnitMapDetail { MapId = 1, ControlId = 1, UnitId = 1, OwnerId = 1001, ApproverId = 2001, ReportingManager = 'Y', EffectiveDate = new DateTime(2026, 1, 1), DueDate = new DateTime(2026, 1, 31), CreatedBy = 1, CreatedOn = now },
                new UnitMapDetail { MapId = 2, ControlId = 1, UnitId = 2, OwnerId = 1002, ApproverId = 2001, ReportingManager = 'N', EffectiveDate = new DateTime(2026, 1, 1), DueDate = new DateTime(2026, 1, 31), CreatedBy = 1, CreatedOn = now },
                new UnitMapDetail { MapId = 3, ControlId = 2, UnitId = 1, OwnerId = 1003, ApproverId = 2002, ReportingManager = 'Y', EffectiveDate = new DateTime(2026, 1, 1), DueDate = new DateTime(2026, 3, 31), CreatedBy = 1, CreatedOn = now },
                new UnitMapDetail { MapId = 4, ControlId = 3, UnitId = 1, OwnerId = 1004, ApproverId = 2003, ReportingManager = 'Y', EffectiveDate = new DateTime(2026, 1, 1), DueDate = new DateTime(2026, 1, 31), CreatedBy = 1, CreatedOn = now },
                new UnitMapDetail { MapId = 5, ControlId = 4, UnitId = 1, OwnerId = 1005, ApproverId = 2004, ReportingManager = 'Y', EffectiveDate = new DateTime(2026, 1, 1), DueDate = new DateTime(2026, 1, 31), CreatedBy = 1, CreatedOn = now },
                new UnitMapDetail { MapId = 6, ControlId = 4, UnitId = 2, OwnerId = 1006, ApproverId = 2004, ReportingManager = 'N', EffectiveDate = new DateTime(2026, 1, 1), DueDate = new DateTime(2026, 1, 31), CreatedBy = 1, CreatedOn = now },
                new UnitMapDetail { MapId = 7, ControlId = 5, UnitId = 1, OwnerId = 1007, ApproverId = 2005, ReportingManager = 'Y', EffectiveDate = new DateTime(2026, 1, 1), DueDate = new DateTime(2026, 1, 31), CreatedBy = 1, CreatedOn = now },
                new UnitMapDetail { MapId = 8, ControlId = 6, UnitId = 5, OwnerId = 1008, ApproverId = 2006, ReportingManager = 'Y', EffectiveDate = new DateTime(2026, 1, 1), DueDate = new DateTime(2026, 1, 31), CreatedBy = 1, CreatedOn = now },
                new UnitMapDetail { MapId = 9, ControlId = 9, UnitId = 1, OwnerId = 1009, ApproverId = 2007, ReportingManager = 'Y', EffectiveDate = new DateTime(2026, 1, 1), DueDate = new DateTime(2026, 1, 31), CreatedBy = 1, CreatedOn = now },
                new UnitMapDetail { MapId = 10, ControlId = 9, UnitId = 3, OwnerId = 1010, ApproverId = 2007, ReportingManager = 'N', EffectiveDate = new DateTime(2026, 1, 1), DueDate = new DateTime(2026, 1, 31), CreatedBy = 1, CreatedOn = now }
            );
            await context.SaveChangesAsync();
        }

        // ── Surveys ──
        if (!await context.Surveys.AnyAsync())
        {
            context.Surveys.AddRange(
                new Survey
                {
                    SurveyId = 1, Title = "Q1 2026 Control Self-Assessment",
                    DueDate = new DateTime(2026, 1, 1), CloseDate = new DateTime(2026, 3, 31),
                    StartDate = new DateTime(2026, 3, 15), EndDate = new DateTime(2026, 4, 15),
                    Alert1 = 7, Alert2 = 3, CreatedBy = 1, CreatedOn = now
                },
                new Survey
                {
                    SurveyId = 2, Title = "Q2 2026 Control Self-Assessment",
                    DueDate = new DateTime(2026, 4, 1), CloseDate = new DateTime(2026, 6, 30),
                    StartDate = new DateTime(2026, 6, 15), EndDate = new DateTime(2026, 7, 15),
                    Alert1 = 7, Alert2 = 3, CreatedBy = 1, CreatedOn = now
                },
                new Survey
                {
                    SurveyId = 3, Title = "Annual IT Controls Assessment 2026",
                    DueDate = new DateTime(2026, 1, 1), CloseDate = new DateTime(2026, 12, 31),
                    StartDate = new DateTime(2026, 12, 1), EndDate = new DateTime(2027, 1, 15),
                    Alert1 = 14, Alert2 = 7, CreatedBy = 1, CreatedOn = now
                }
            );
            await context.SaveChangesAsync();
        }

        // ── Survey Questions ──
        if (!await context.SurveyQuestions.AnyAsync())
        {
            var surveyDue = new DateTime(2026, 3, 31);
            context.SurveyQuestions.AddRange(
                new SurveyQuestion
                {
                    SurveyQuestionId = 1, SurveyId = 1, ControlId = 1, UnitId = 1, OwnerId = 1001, ApproverId = 2001,
                    OriginalDueDate = surveyDue, DueDate = surveyDue, AssessmentFlag = 'C', ApprovalFlag = 'Y',
                    RemedialFlag = 'N', AssessmentDate = new DateTime(2026, 3, 20), ApprovalDate = new DateTime(2026, 3, 22),
                    DelayDays = 0, RemedialCount = 0, CreatedBy = 1, CreatedOn = now
                },
                new SurveyQuestion
                {
                    SurveyQuestionId = 2, SurveyId = 1, ControlId = 1, UnitId = 2, OwnerId = 1002, ApproverId = 2001,
                    OriginalDueDate = surveyDue, DueDate = surveyDue, AssessmentFlag = 'C', ApprovalFlag = 'P',
                    RemedialFlag = 'N', AssessmentDate = new DateTime(2026, 3, 25),
                    DelayDays = 0, RemedialCount = 0, CreatedBy = 1, CreatedOn = now
                },
                new SurveyQuestion
                {
                    SurveyQuestionId = 3, SurveyId = 1, ControlId = 2, UnitId = 1, OwnerId = 1003, ApproverId = 2002,
                    OriginalDueDate = surveyDue, DueDate = surveyDue, AssessmentFlag = 'P', ApprovalFlag = 'N',
                    RemedialFlag = 'N', DelayDays = 0, RemedialCount = 0, CreatedBy = 1, CreatedOn = now
                },
                new SurveyQuestion
                {
                    SurveyQuestionId = 4, SurveyId = 1, ControlId = 3, UnitId = 1, OwnerId = 1004, ApproverId = 2003,
                    OriginalDueDate = surveyDue, DueDate = surveyDue, AssessmentFlag = 'C', ApprovalFlag = 'Y',
                    RemedialFlag = 'N', AssessmentDate = new DateTime(2026, 3, 18), ApprovalDate = new DateTime(2026, 3, 19),
                    DelayDays = 0, RemedialCount = 0, CreatedBy = 1, CreatedOn = now
                },
                new SurveyQuestion
                {
                    SurveyQuestionId = 5, SurveyId = 1, ControlId = 4, UnitId = 1, OwnerId = 1005, ApproverId = 2004,
                    OriginalDueDate = surveyDue, DueDate = new DateTime(2026, 4, 15),
                    AssessmentFlag = 'C', ApprovalFlag = 'N', RemedialFlag = 'P',
                    RemedialDate = new DateTime(2026, 4, 15), AssessmentDate = new DateTime(2026, 3, 28),
                    DelayDays = 0, RemedialCount = 1, CreatedBy = 1, CreatedOn = now
                },
                new SurveyQuestion
                {
                    SurveyQuestionId = 6, SurveyId = 1, ControlId = 5, UnitId = 1, OwnerId = 1007, ApproverId = 2005,
                    OriginalDueDate = surveyDue, DueDate = surveyDue, AssessmentFlag = 'C', ApprovalFlag = 'Y',
                    RemedialFlag = 'N', AssessmentDate = new DateTime(2026, 3, 15), ApprovalDate = new DateTime(2026, 3, 16),
                    DelayDays = 0, RemedialCount = 0, CreatedBy = 1, CreatedOn = now
                },
                new SurveyQuestion
                {
                    SurveyQuestionId = 7, SurveyId = 1, ControlId = 9, UnitId = 1, OwnerId = 1009, ApproverId = 2007,
                    OriginalDueDate = surveyDue, DueDate = surveyDue, AssessmentFlag = 'P', ApprovalFlag = 'N',
                    RemedialFlag = 'N', DelayDays = 0, RemedialCount = 0, CreatedBy = 1, CreatedOn = now
                },
                new SurveyQuestion
                {
                    SurveyQuestionId = 8, SurveyId = 1, ControlId = 9, UnitId = 3, OwnerId = 1010, ApproverId = 2007,
                    OriginalDueDate = surveyDue, DueDate = surveyDue, AssessmentFlag = 'C', ApprovalFlag = 'Y',
                    RemedialFlag = 'N', AssessmentDate = new DateTime(2026, 3, 10), ApprovalDate = new DateTime(2026, 3, 12),
                    DelayDays = 0, RemedialCount = 0, CreatedBy = 1, CreatedOn = now
                }
            );
            await context.SaveChangesAsync();
        }

        // ── Survey Feedback ──
        if (!await context.SurveyFeedbacks.AnyAsync())
        {
            context.SurveyFeedbacks.AddRange(
                new SurveyFeedback
                {
                    FeedbackId = 1, SurveyQuestionId = 1, EmployeeSysId = 1001,
                    Status = 'P', Type = 'C', RemedialFlag = 'N', Remarks = "All journal entries reviewed and approved. No exceptions noted.",
                    EnteredOn = new DateTime(2026, 3, 20), EvidenceFlag = 'Y', ApprovalFlag = 'Y',
                    ApproverRemarks = "Verified. Satisfactory.", ApprovalDate = new DateTime(2026, 3, 22), ApprovedBy = 2001,
                    EntryDate = new DateTime(2026, 3, 20)
                },
                new SurveyFeedback
                {
                    FeedbackId = 2, SurveyQuestionId = 1, EmployeeSysId = 2001,
                    Status = 'P', Type = 'A', RemedialFlag = 'N', Remarks = "Approver review completed.",
                    EnteredOn = new DateTime(2026, 3, 22), EvidenceFlag = 'N', ApprovalFlag = 'Y',
                    ApproverRemarks = "Approved", ApprovalDate = new DateTime(2026, 3, 22), ApprovedBy = 2001,
                    EntryDate = new DateTime(2026, 3, 22)
                },
                new SurveyFeedback
                {
                    FeedbackId = 3, SurveyQuestionId = 2, EmployeeSysId = 1002,
                    Status = 'P', Type = 'C', RemedialFlag = 'N', Remarks = "Journal entries reviewed for Branch Office. Minor delays in documentation.",
                    EnteredOn = new DateTime(2026, 3, 25), EvidenceFlag = 'Y', ApprovalFlag = 'P',
                    ApproverRemarks = "", EntryDate = new DateTime(2026, 3, 25)
                },
                new SurveyFeedback
                {
                    FeedbackId = 4, SurveyQuestionId = 4, EmployeeSysId = 1004,
                    Status = 'P', Type = 'C', RemedialFlag = 'N', Remarks = "Three-way matching completed for all invoices above threshold.",
                    EnteredOn = new DateTime(2026, 3, 18), EvidenceFlag = 'Y', ApprovalFlag = 'Y',
                    ApproverRemarks = "Good. No exceptions.", ApprovalDate = new DateTime(2026, 3, 19), ApprovedBy = 2003,
                    EntryDate = new DateTime(2026, 3, 18)
                },
                new SurveyFeedback
                {
                    FeedbackId = 5, SurveyQuestionId = 5, EmployeeSysId = 1005,
                    Status = 'F', Type = 'C', RemedialFlag = 'Y', RemedialDate = new DateTime(2026, 4, 15),
                    Remarks = "Reconciliation incomplete for 3 accounts. Remediation initiated.",
                    EnteredOn = new DateTime(2026, 3, 28), EvidenceFlag = 'Y', ApprovalFlag = 'P',
                    ApproverRemarks = "", EntryDate = new DateTime(2026, 3, 28)
                },
                new SurveyFeedback
                {
                    FeedbackId = 6, SurveyQuestionId = 6, EmployeeSysId = 1007,
                    Status = 'P', Type = 'C', RemedialFlag = 'N', Remarks = "Payroll calculations verified for all employees. No discrepancies found.",
                    EnteredOn = new DateTime(2026, 3, 15), EvidenceFlag = 'Y', ApprovalFlag = 'Y',
                    ApproverRemarks = "Verified and approved.", ApprovalDate = new DateTime(2026, 3, 16), ApprovedBy = 2005,
                    EntryDate = new DateTime(2026, 3, 15)
                },
                new SurveyFeedback
                {
                    FeedbackId = 7, SurveyQuestionId = 8, EmployeeSysId = 1010,
                    Status = 'P', Type = 'C', RemedialFlag = 'N', Remarks = "AML monitoring completed. 2 STRs filed during the quarter.",
                    EnteredOn = new DateTime(2026, 3, 10), EvidenceFlag = 'Y', ApprovalFlag = 'Y',
                    ApproverRemarks = "Satisfactory. STR filing timely.", ApprovalDate = new DateTime(2026, 3, 12), ApprovedBy = 2007,
                    EntryDate = new DateTime(2026, 3, 10)
                }
            );
            await context.SaveChangesAsync();
        }

        // ── Survey Attachments ──
        if (!await context.SurveyAttachments.AnyAsync())
        {
            context.SurveyAttachments.AddRange(
                new SurveyAttachment { AttachmentId = 1, FeedbackId = 1, ControlEvidenceId = 1, Attachment = "je_review_jan2026.pdf" },
                new SurveyAttachment { AttachmentId = 2, FeedbackId = 1, ControlEvidenceId = 2, Attachment = "je_checklist_jan2026.xlsx" },
                new SurveyAttachment { AttachmentId = 3, FeedbackId = 3, ControlEvidenceId = 1, Attachment = "je_review_branch_feb2026.pdf" },
                new SurveyAttachment { AttachmentId = 4, FeedbackId = 4, ControlEvidenceId = 5, Attachment = "three_way_match_q1.pdf" },
                new SurveyAttachment { AttachmentId = 5, FeedbackId = 5, ControlEvidenceId = 6, Attachment = "bank_recon_partial_q1.pdf" },
                new SurveyAttachment { AttachmentId = 6, FeedbackId = 6, ControlEvidenceId = 7, Attachment = "payroll_approval_q1.pdf" },
                new SurveyAttachment { AttachmentId = 7, FeedbackId = 7, ControlEvidenceId = 12, Attachment = "str_report_q1_2026.pdf" }
            );
            await context.SaveChangesAsync();
        }

        // ── CSA Users (keyless entity – use raw SQL) ──
        var userCount = await context.Database.SqlQueryRaw<int>("SELECT COUNT(*) AS [Value] FROM CSA_USERS").FirstOrDefaultAsync();
        if (userCount == 0)
        {
            await context.Database.ExecuteSqlRawAsync(@"
                INSERT INTO CSA_USERS (USER_EMPNO, USER_PINNUM, USER_NAME, USER_SYSID, USER_EMAIL) VALUES
                (1001, 1001, 'Ahmed Al Rashid', 1001, 'ahmed.rashid@company.com'),
                (1002, 1002, 'Sara Al Mahmoud', 1002, 'sara.mahmoud@company.com'),
                (1003, 1003, 'Khalid Hassan', 1003, 'khalid.hassan@company.com'),
                (1004, 1004, 'Fatima Al Zaabi', 1004, 'fatima.zaabi@company.com'),
                (1005, 1005, 'Omar Nasser', 1005, 'omar.nasser@company.com'),
                (1006, 1006, 'Layla Ibrahim', 1006, 'layla.ibrahim@company.com'),
                (1007, 1007, 'Yusuf Al Mansoori', 1007, 'yusuf.mansoori@company.com'),
                (1008, 1008, 'Noor Al Shamsi', 1008, 'noor.shamsi@company.com'),
                (1009, 1009, 'Rashid Al Ketbi', 1009, 'rashid.ketbi@company.com'),
                (1010, 1010, 'Mariam Al Suwaidi', 1010, 'mariam.suwaidi@company.com'),
                (2001, 2001, 'Ali Al Dhaheri', 2001, 'ali.dhaheri@company.com'),
                (2002, 2002, 'Hessa Al Nuaimi', 2002, 'hessa.nuaimi@company.com'),
                (2003, 2003, 'Sultan Al Kaabi', 2003, 'sultan.kaabi@company.com'),
                (2004, 2004, 'Moza Al Rumaithi', 2004, 'moza.rumaithi@company.com'),
                (2005, 2005, 'Hamdan Al Falasi', 2005, 'hamdan.falasi@company.com'),
                (2006, 2006, 'Shamma Al Mazrouei', 2006, 'shamma.mazrouei@company.com'),
                (2007, 2007, 'Saeed Al Qubaisi', 2007, 'saeed.qubaisi@company.com')");
        }
    }
}
