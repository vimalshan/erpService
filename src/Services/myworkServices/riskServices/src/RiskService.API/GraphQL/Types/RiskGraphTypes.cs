using RiskService.Application.DTOs;

namespace RiskService.API.GraphQL.Types;

public class RiskGraphType : ObjectType<RiskDto>
{
    protected override void Configure(IObjectTypeDescriptor<RiskDto> descriptor)
    {
        descriptor.Name("Risk");
        descriptor.Field(r => r.Id).Type<NonNullType<IdType>>();
        descriptor.Field(r => r.EventTitle).Type<NonNullType<StringType>>();
        descriptor.Field(r => r.Description).Type<NonNullType<StringType>>();
        descriptor.Field(r => r.TypeName).Type<StringType>();
        descriptor.Field(r => r.ApprovalStatus).Type<NonNullType<StringType>>();
        descriptor.Field(r => r.Causes).Type<ListType<RiskCauseGraphType>>();
        descriptor.Field(r => r.Controls).Type<ListType<RiskControlGraphType>>();
        descriptor.Field(r => r.Mitigations).Type<ListType<MitigationGraphType>>();
    }
}

public class RiskCauseGraphType : ObjectType<RiskCauseDto>
{
    protected override void Configure(IObjectTypeDescriptor<RiskCauseDto> descriptor)
    {
        descriptor.Name("RiskCause");
    }
}

public class RiskControlGraphType : ObjectType<RiskControlDto>
{
    protected override void Configure(IObjectTypeDescriptor<RiskControlDto> descriptor)
    {
        descriptor.Name("RiskControl");
    }
}

public class MitigationGraphType : ObjectType<MitigationDto>
{
    protected override void Configure(IObjectTypeDescriptor<MitigationDto> descriptor)
    {
        descriptor.Name("Mitigation");
        descriptor.Field(m => m.Actions).Type<ListType<MitigationActionGraphType>>();
    }
}

public class MitigationActionGraphType : ObjectType<MitigationActionDto>
{
    protected override void Configure(IObjectTypeDescriptor<MitigationActionDto> descriptor)
    {
        descriptor.Name("MitigationAction");
    }
}

public class SelfAssessmentGraphType : ObjectType<SelfAssessmentDto>
{
    protected override void Configure(IObjectTypeDescriptor<SelfAssessmentDto> descriptor)
    {
        descriptor.Name("SelfAssessment");
    }
}

public class RiskTypeGraphType : ObjectType<RiskTypeDto>
{
    protected override void Configure(IObjectTypeDescriptor<RiskTypeDto> descriptor)
    {
        descriptor.Name("RiskType");
    }
}
