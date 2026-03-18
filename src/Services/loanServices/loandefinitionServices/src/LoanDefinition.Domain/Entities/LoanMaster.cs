using LoanDefinition.Domain.ValueObjects;
using LoanDefinition.SharedKernel;

namespace LoanDefinition.Domain.Entities;

public class LoanMaster : AggregateRoot<long>
{
    public string LoanName { get; private set; } = string.Empty;
    public string LoanPurpose { get; private set; } = string.Empty;
    public int ApplyToUnit { get; private set; }
    public long OrgId { get; private set; }
    public long UnitId { get; private set; }
    public long LoanTypeId { get; private set; }
    public string ApplyToConfirmedEmp { get; private set; } = "N";
    public string GradeCategory { get; private set; } = string.Empty;
    public int ApplyToAllGrade { get; private set; }
    public long GradeId { get; private set; }
    public LoanLimit LoanLimit { get; private set; } = null!;
    public string AutoPayOnCompletion { get; private set; } = "N";
    public string AllowForceClose { get; private set; } = "N";
    public string AllowMultipleNos { get; private set; } = "N";
    public string OnConfirmation { get; private set; } = "N";
    public string CheckEntitlement { get; private set; } = "N";
    public string Recoverable { get; private set; } = "Y";
    public int ApplicationNos { get; private set; }
    public string CheckNetPayPercentage { get; private set; } = "N";
    public string BkdInterestRateRevision { get; private set; } = "N";
    public string SubClassAvailable { get; private set; } = "N";
    public string? ItClass { get; private set; }
    public string DocumentRequired { get; private set; } = "N";
    public string DocumentUploadRequired { get; private set; } = "N";
    public string SelfApplicationAllowed { get; private set; } = "N";
    public string EmpSpecificRatesAllowed { get; private set; } = "N";
    public string HrApproval { get; private set; } = "N";
    public DateTime EffectiveDate { get; private set; }
    public DateTime? ClosureDate { get; private set; }
    public string CompoundingFactor { get; private set; } = string.Empty;
    public string InterestFrequency { get; private set; } = string.Empty;
    public string RecoveryType { get; private set; } = string.Empty;
    public string BulkUploadAllowed { get; private set; } = "N";
    public long PrincipalRecoveryEdId { get; private set; }
    public long InterestRecoveryEdId { get; private set; }
    public long PrincipalPaymentEdId { get; private set; }
    public string PolicyFileName { get; private set; } = string.Empty;
    public string GuarantorRequired { get; private set; } = "N";
    public string CheckBasicEntitlement { get; private set; } = "N";
    public string AllowAdditionalLoan { get; private set; } = "N";
    public long AdditionalLoanNo { get; private set; }
    public string CurrentRecovery { get; private set; } = "N";
    public string ReportingUnitApplicable { get; private set; } = "N";
    public int ReportingUnitId { get; private set; }
    public string FlexiFirstInstDate { get; private set; } = "N";

    // Navigation
    public LoanTypeMaster? LoanType { get; private set; }

    private readonly List<LoanSubClass> _subClasses = [];
    public IReadOnlyCollection<LoanSubClass> SubClasses => _subClasses.AsReadOnly();

    private readonly List<LoanInterestRateMaster> _interestRates = [];
    public IReadOnlyCollection<LoanInterestRateMaster> InterestRates => _interestRates.AsReadOnly();

    private readonly List<LoanLimitRangeMaster> _limitRanges = [];
    public IReadOnlyCollection<LoanLimitRangeMaster> LimitRanges => _limitRanges.AsReadOnly();

    private readonly List<LoanFestivalMap> _festivalMaps = [];
    public IReadOnlyCollection<LoanFestivalMap> FestivalMaps => _festivalMaps.AsReadOnly();

    private LoanMaster() { }

    public static LoanMaster Create(
        long loanId, string loanName, string loanPurpose, long loanTypeId,
        long minimumLimit, long maximumLimit, DateTime effectiveDate,
        string recoveryType, string compoundingFactor, string interestFrequency,
        long principalRecoveryEdId, long interestRecoveryEdId, long principalPaymentEdId,
        long createdBy)
    {
        var entity = new LoanMaster
        {
            Id = loanId,
            LoanName = loanName,
            LoanPurpose = loanPurpose,
            LoanTypeId = loanTypeId,
            LoanLimit = new LoanLimit(minimumLimit, maximumLimit),
            EffectiveDate = effectiveDate,
            RecoveryType = recoveryType,
            CompoundingFactor = compoundingFactor,
            InterestFrequency = interestFrequency,
            PrincipalRecoveryEdId = principalRecoveryEdId,
            InterestRecoveryEdId = interestRecoveryEdId,
            PrincipalPaymentEdId = principalPaymentEdId,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow,
            LastModifiedBy = createdBy,
            LastModifiedOn = DateTime.UtcNow
        };
        entity.AddDomainEvent(new Events.LoanMasterCreatedEvent(entity.Id, loanName));
        return entity;
    }

    public void Update(string loanName, string loanPurpose, long minimumLimit, long maximumLimit, long modifiedBy)
    {
        LoanName = loanName;
        LoanPurpose = loanPurpose;
        LoanLimit = new LoanLimit(minimumLimit, maximumLimit);
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
        AddDomainEvent(new Events.LoanMasterUpdatedEvent(Id, loanName));
    }

    public void SetClosureDate(DateTime closureDate, long modifiedBy)
    {
        ClosureDate = closureDate;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }

    public void AddSubClass(LoanSubClass subClass) => _subClasses.Add(subClass);
    public void AddInterestRate(LoanInterestRateMaster rate) => _interestRates.Add(rate);
    public void AddLimitRange(LoanLimitRangeMaster range) => _limitRanges.Add(range);
}
