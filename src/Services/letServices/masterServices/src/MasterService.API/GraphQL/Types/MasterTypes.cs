using MasterService.Application.DTOs;
using MasterService.Application.Features.Skills.Commands;
using MasterService.Application.Features.Skills.Queries;
using MasterService.Domain.Entities;
using MasterService.Infrastructure.Persistence;
using MediatR;

namespace MasterService.API.GraphQL.Types;

public class SkillType : ObjectType<SkillDto>
{
    protected override void Configure(IObjectTypeDescriptor<SkillDto> descriptor)
    {
        descriptor.Description("A skill in the master data.");
        descriptor.Field(s => s.SkillCode).Description("Unique skill identifier.");
        descriptor.Field(s => s.SkillName).Description("Name of the skill.");
        descriptor.Field(s => s.SkillType).Description("T=Technical, B=Behavioural, F=Functional.");
        descriptor.Field(s => s.IsActive).Description("True if not closed.");
    }
}

public class TrainingProviderType : ObjectType<TrainingProviderDto>
{
    protected override void Configure(IObjectTypeDescriptor<TrainingProviderDto> descriptor)
    {
        descriptor.Description("A training provider in the master data.");
    }
}

public class JobMasterType : ObjectType<JobMasterDto>
{
    protected override void Configure(IObjectTypeDescriptor<JobMasterDto> descriptor)
    {
        descriptor.Description("A job in the master data.");
    }
}

public class CategoryType : ObjectType<CategoryDto>
{
    protected override void Configure(IObjectTypeDescriptor<CategoryDto> descriptor)
    {
        descriptor.Description("A job category.");
    }
}
