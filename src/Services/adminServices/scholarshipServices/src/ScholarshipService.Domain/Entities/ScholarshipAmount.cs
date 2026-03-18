using ScholarshipService.Domain.Common;

namespace ScholarshipService.Domain.Entities;

/// <summary>Represents eligible scholarship amounts for a grade/exam/year combination (maps to SCHOLARSHIP_AMOUNT).</summary>
public class ScholarshipAmount : BaseEntity, IAuditableEntity
{
    public long Id { get; private set; }           // SCH_AMTID
    public long OrgId { get; private set; }        // SCH_ORGID
    public string GradeCategory { get; private set; } = string.Empty; // SCH_GRADECAT
    public string EligibleExam { get; private set; } = string.Empty;  // SCH_ELGIBLEEXAM (10 or 12)
    public string ApplicableAllGrade { get; private set; } = "0";     // SCH_APPLICABLEALLGRADE (0=All, 1=Specific)
    public decimal GradeId { get; private set; }  // SCH_GRADEID
    public decimal FromYear { get; private set; } // SCH_FROMYEAR
    public decimal? CloseYear { get; private set; } // SCH_CLOSEYEAR
    public long EligibleAmount { get; private set; } // SCH_ELGIBLEAMOUNT
    public int EligibleYear { get; private set; } // SCH_ELGIBLEYEAR (max course year for scholarship)
    public int CutoffMarks { get; private set; }  // SCH_CUTOFFMARKS
    public DateTime CreatedOn { get; private set; }  // SCH_CREATEDON
    public long CreatedBy { get; private set; }      // SCH_CREATEDBY
    public DateTime? UpdatedOn { get; private set; } // SCH_UPDATEDON
    public long? UpdatedBy { get; private set; }     // SCH_UPDATEDBY

    protected ScholarshipAmount() { }

    public static ScholarshipAmount Create(
        long id, long orgId, string gradeCategory, string eligibleExam,
        string applicableAllGrade, decimal gradeId, decimal fromYear, decimal? closeYear,
        long eligibleAmount, int eligibleYear, int cutoffMarks, long createdBy)
    {
        return new ScholarshipAmount
        {
            Id = id,
            OrgId = orgId,
            GradeCategory = gradeCategory,
            EligibleExam = eligibleExam,
            ApplicableAllGrade = applicableAllGrade,
            GradeId = gradeId,
            FromYear = fromYear,
            CloseYear = closeYear,
            EligibleAmount = eligibleAmount,
            EligibleYear = eligibleYear,
            CutoffMarks = cutoffMarks,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = createdBy
        };
    }

    public void Update(long eligibleAmount, decimal? closeYear, int cutoffMarks, long updatedBy)
    {
        EligibleAmount = eligibleAmount;
        CloseYear = closeYear;
        CutoffMarks = cutoffMarks;
        UpdatedOn = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }
}
