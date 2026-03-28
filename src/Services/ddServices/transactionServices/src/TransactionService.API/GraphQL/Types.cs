using HotChocolate.Types;
using TransactionService.Application.DTOs;

namespace TransactionService.API.GraphQL;

public class DemandMasterType : ObjectType<DemandMasterDto>
{
    protected override void Configure(IObjectTypeDescriptor<DemandMasterDto> descriptor)
    {
        descriptor.Field(t => t.Id).Type<NonNullType<LongType>>();
        descriptor.Field(t => t.DemandType).Type<NonNullType<StringType>>();
        descriptor.Field(t => t.DepartmentId).Type<NonNullType<LongType>>();
        descriptor.Field(t => t.DemandDescription).Type<NonNullType<StringType>>();
        descriptor.Field(t => t.RequiredDate).Type<NonNullType<DateTimeType>>();
        descriptor.Field(t => t.Priority).Type<NonNullType<StringType>>();
        // char mapped as StringType for HotChocolate 14.0.0 compatibility
        descriptor.Field(t => t.DemandStatus).Type<NonNullType<StringType>>();
        descriptor.Field(t => t.CreatedBy).Type<NonNullType<LongType>>();
        descriptor.Field(t => t.CreatedOn).Type<NonNullType<DateTimeType>>();
        descriptor.Field(t => t.ApprovalRemarks).Type<StringType>();
        descriptor.Field(t => t.ApprovedBy).Type<LongType>();
        descriptor.Field(t => t.ApprovalDate).Type<DateTimeType>();
        descriptor.Field(t => t.CompletionRemarks).Type<StringType>();
        descriptor.Field(t => t.CompletedBy).Type<LongType>();
        descriptor.Field(t => t.CompletionDate).Type<DateTimeType>();
        descriptor.Field(t => t.CreatedAt).Type<NonNullType<DateTimeType>>();
        descriptor.Field(t => t.UpdatedAt).Type<DateTimeType>();
    }
}

public class SaaBudgetType : ObjectType<SaaBudgetDto>
{
    protected override void Configure(IObjectTypeDescriptor<SaaBudgetDto> descriptor)
    {
        descriptor.Field(t => t.Id).Type<NonNullType<LongType>>();
        descriptor.Field(t => t.BusinessId).Type<NonNullType<LongType>>();
        descriptor.Field(t => t.YearId).Type<NonNullType<LongType>>();
        descriptor.Field(t => t.BudgetAmount).Type<NonNullType<DecimalType>>();
        descriptor.Field(t => t.UpdatedBy).Type<NonNullType<LongType>>();
        descriptor.Field(t => t.UpdatedOn).Type<NonNullType<DateTimeType>>();
        descriptor.Field(t => t.CreatedAt).Type<NonNullType<DateTimeType>>();
        descriptor.Field(t => t.UpdatedAt).Type<DateTimeType>();
    }
}

public class SaaPeriodType : ObjectType<SaaPeriodDto>
{
    protected override void Configure(IObjectTypeDescriptor<SaaPeriodDto> descriptor)
    {
        descriptor.Field(t => t.Id).Type<NonNullType<LongType>>();
        descriptor.Field(t => t.YearId).Type<NonNullType<LongType>>();
        descriptor.Field(t => t.QuarterNo).Type<NonNullType<LongType>>();
        // char mapped as StringType for HotChocolate 14.0.0 compatibility
        descriptor.Field(t => t.Status).Type<NonNullType<StringType>>();
        descriptor.Field(t => t.PeriodOpenDate).Type<NonNullType<DateTimeType>>();
        descriptor.Field(t => t.PeriodCloseDate).Type<NonNullType<DateTimeType>>();
        descriptor.Field(t => t.CircularGenOn).Type<DateTimeType>();
        descriptor.Field(t => t.CircularGenBy).Type<LongType>();
        descriptor.Field(t => t.ReminderLetOn).Type<DateTimeType>();
        descriptor.Field(t => t.FormOpenDate).Type<NonNullType<DateTimeType>>();
        descriptor.Field(t => t.AppraiserLastDate).Type<DateTimeType>();
        descriptor.Field(t => t.ReviewerLastDate).Type<DateTimeType>();
        descriptor.Field(t => t.BhrLastDate).Type<DateTimeType>();
        descriptor.Field(t => t.UhrLastDate).Type<DateTimeType>();
        descriptor.Field(t => t.CreatedAt).Type<NonNullType<DateTimeType>>();
        descriptor.Field(t => t.UpdatedAt).Type<DateTimeType>();
    }
}

public class SaaLevelType : ObjectType<SaaLevelDto>
{
    protected override void Configure(IObjectTypeDescriptor<SaaLevelDto> descriptor)
    {
        descriptor.Field(t => t.Id).Type<NonNullType<LongType>>();
        descriptor.Field(t => t.LevelDesc).Type<NonNullType<StringType>>();
        descriptor.Field(t => t.LevelAmount).Type<NonNullType<StringType>>();
        descriptor.Field(t => t.LevelReason).Type<NonNullType<StringType>>();
        descriptor.Field(t => t.LevelMin).Type<NonNullType<DecimalType>>();
        descriptor.Field(t => t.LevelMax).Type<NonNullType<DecimalType>>();
        descriptor.Field(t => t.LevelEffDate).Type<NonNullType<DateTimeType>>();
        descriptor.Field(t => t.LevelCloseDate).Type<DateTimeType>();
        descriptor.Field(t => t.LevelUpdatedBy).Type<NonNullType<LongType>>();
        descriptor.Field(t => t.LevelUpdatedOn).Type<NonNullType<DateTimeType>>();
        descriptor.Field(t => t.CreatedAt).Type<NonNullType<DateTimeType>>();
        descriptor.Field(t => t.UpdatedAt).Type<DateTimeType>();
    }
}

public class SaaRecommendType : ObjectType<SaaRecommendDto>
{
    protected override void Configure(IObjectTypeDescriptor<SaaRecommendDto> descriptor)
    {
        descriptor.Field(t => t.Id).Type<NonNullType<LongType>>();
        descriptor.Field(t => t.YearId).Type<NonNullType<LongType>>();
        descriptor.Field(t => t.PeriodId).Type<NonNullType<LongType>>();
        descriptor.Field(t => t.EmpSysId).Type<NonNullType<LongType>>();
        descriptor.Field(t => t.LevelId).Type<NonNullType<LongType>>();
        descriptor.Field(t => t.CtcAmount).Type<NonNullType<DecimalType>>();
        descriptor.Field(t => t.MaximumCap).Type<NonNullType<DecimalType>>();
        descriptor.Field(t => t.EligibilityAmount).Type<NonNullType<DecimalType>>();
        descriptor.Field(t => t.RecommendAmount).Type<DecimalType>();
        descriptor.Field(t => t.InitiativeTaken).Type<NonNullType<StringType>>();
        descriptor.Field(t => t.Results).Type<NonNullType<StringType>>();
        descriptor.Field(t => t.AddRemarks).Type<StringType>();
        descriptor.Field(t => t.Status).Type<NonNullType<LongType>>();
        descriptor.Field(t => t.RejectionBy).Type<LongType>();
        descriptor.Field(t => t.RejectionOn).Type<DateTimeType>();
        descriptor.Field(t => t.RecommendBy).Type<NonNullType<StringType>>();
        descriptor.Field(t => t.RecommendSubmitBy).Type<LongType>();
        descriptor.Field(t => t.RecommendSubmitOn).Type<DateTimeType>();
        descriptor.Field(t => t.ReviewerSubmitBy).Type<LongType>();
        descriptor.Field(t => t.ReviewerSubmitOn).Type<DateTimeType>();
        descriptor.Field(t => t.BhrSubmitBy).Type<LongType>();
        descriptor.Field(t => t.BhrSubmitOn).Type<DateTimeType>();
        descriptor.Field(t => t.ChrSubmitBy).Type<LongType>();
        descriptor.Field(t => t.ChrSubmitOn).Type<DateTimeType>();
        descriptor.Field(t => t.RejectionRemarks).Type<StringType>();
        descriptor.Field(t => t.FinalLevel).Type<LongType>();
        descriptor.Field(t => t.FinalAmount).Type<DecimalType>();
        descriptor.Field(t => t.InitiativeLetter).Type<StringType>();
        descriptor.Field(t => t.ResultsLetter).Type<StringType>();
        descriptor.Field(t => t.UhrSubmitBy).Type<LongType>();
        descriptor.Field(t => t.UhrSubmitOn).Type<DateTimeType>();
        descriptor.Field(t => t.RecommendSignId).Type<LongType>();
        descriptor.Field(t => t.RecommendSignId2).Type<LongType>();
        descriptor.Field(t => t.CreatedAt).Type<NonNullType<DateTimeType>>();
        descriptor.Field(t => t.UpdatedAt).Type<DateTimeType>();
    }
}

public class SaaSubmitType : ObjectType<SaaSubmitDto>
{
    protected override void Configure(IObjectTypeDescriptor<SaaSubmitDto> descriptor)
    {
        descriptor.Field(t => t.Id).Type<NonNullType<LongType>>();
        descriptor.Field(t => t.PeriodId).Type<NonNullType<LongType>>();
        descriptor.Field(t => t.BusId).Type<NonNullType<LongType>>();
        // char mapped as StringType for HotChocolate 14.0.0 compatibility
        descriptor.Field(t => t.BhrFlag).Type<NonNullType<StringType>>();
        descriptor.Field(t => t.ChrFlag).Type<NonNullType<StringType>>();
        descriptor.Field(t => t.BhrUpdBy).Type<NonNullType<LongType>>();
        descriptor.Field(t => t.BhrUpdOn).Type<NonNullType<DateTimeType>>();
        descriptor.Field(t => t.BhrAmount).Type<DecimalType>();
        descriptor.Field(t => t.ChrUpdBy).Type<LongType>();
        descriptor.Field(t => t.ChrUpdOn).Type<DateTimeType>();
        descriptor.Field(t => t.ChrAmount).Type<DecimalType>();
        descriptor.Field(t => t.CreatedAt).Type<NonNullType<DateTimeType>>();
        descriptor.Field(t => t.UpdatedAt).Type<DateTimeType>();
    }
}

public class SaaMailTriggerType : ObjectType<SaaMailTriggerDto>
{
    protected override void Configure(IObjectTypeDescriptor<SaaMailTriggerDto> descriptor)
    {
        descriptor.Field(t => t.Id).Type<NonNullType<LongType>>();
        descriptor.Field(t => t.QuarterId).Type<NonNullType<LongType>>();
        descriptor.Field(t => t.EmpSysId).Type<NonNullType<LongType>>();
        descriptor.Field(t => t.MailId).Type<NonNullType<StringType>>();
        descriptor.Field(t => t.TriggeredBy).Type<NonNullType<LongType>>();
        descriptor.Field(t => t.TriggeredOn).Type<NonNullType<DateTimeType>>();
        descriptor.Field(t => t.CreatedAt).Type<NonNullType<DateTimeType>>();
        descriptor.Field(t => t.UpdatedAt).Type<DateTimeType>();
    }
}
