using AuditService.Domain.Common;

namespace AuditService.Domain.Entities;

/// <summary>
/// AUDIT_PROCESS_MASTER - Audit process/category master.
/// </summary>
public sealed class AuditProcessMaster : BaseEntity
{
    public decimal AuditProcessId { get; set; }
    public string? AuditProcessName { get; set; }
    public long? AuditProcessCreatedBy { get; set; }
    public DateTime? AuditProcessCreatedOn { get; set; }
}

/// <summary>
/// AUDIT_USERACCESS - Controls which employees can access which business units.
/// </summary>
public sealed class AuditUserAccess : BaseEntity
{
    public decimal AucId { get; set; }
    public decimal AucEmpSysId { get; set; }
    public decimal? AucBusinessId { get; set; }
    public decimal? AucUnitId { get; set; }
    public decimal? AucCreatedBy { get; set; }
    public DateTime? AucCreatedOn { get; set; }
    public decimal? AucModifiedBy { get; set; }
    public DateTime? AucModifiedOn { get; set; }
}

/// <summary>
/// AUDIT_USERMASTER - Audit user master record.
/// </summary>
public sealed class AuditUserMaster : BaseEntity
{
    public decimal AumEmpSysId { get; set; }
    public char? AumLiveStatus { get; set; }
    public decimal? AumLastModifiedBy { get; set; }
    public DateTime? AumLastModifiedOn { get; set; }
    public char? AumMailStatus { get; set; }
    public char? AumUserType { get; set; }
    public char? AumHrmsOpted { get; set; }
}

/// <summary>
/// AUDIT_YEARMASTER - Financial year master for audit planning.
/// </summary>
public sealed class AuditYearMaster : BaseEntity
{
    public decimal AymYearId { get; set; }
    public DateTime AymFrom { get; set; }
    public DateTime AymTo { get; set; }
    public decimal AymLastModifiedBy { get; set; }
    public DateTime AymLastModifiedOn { get; set; }
}

/// <summary>
/// IA_HTML_EMAIL - Email log table.
/// </summary>
public sealed class IaHtmlEmail : BaseEntity
{
    public string? ObvId { get; set; }
    public string? MFrom { get; set; }
    public string? MTo { get; set; }
    public string? MCc { get; set; }
    public string? MBcc { get; set; }
    public string? Subject { get; set; }
    public string? Message { get; set; }
    public string? MServer { get; set; }
    public string? MPort { get; set; }
    public string? OnDate { get; set; }
}

/// <summary>
/// IAESCALATION_MAILS - Escalation mail log.
/// </summary>
public sealed class IaEscalationMail : BaseEntity
{
    public decimal MailId { get; set; }
    public decimal MailObservationAuditId { get; set; }
    public decimal MailAuditeeSysId { get; set; }
    public decimal MailEscalatoSysId { get; set; }
    public string MailSubject { get; set; } = string.Empty;
    public string MailContent { get; set; } = string.Empty;
    public string MailTo { get; set; } = string.Empty;
    public string MailCc { get; set; } = string.Empty;
    public decimal MailSentBy { get; set; }
    public DateTime MailSentOn { get; set; }
}
