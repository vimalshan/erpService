using AutoMapper;
using MasterService.Application.DTOs;
using MasterService.Domain.Entities;

namespace MasterService.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Skill, SkillDto>()
            .ConstructUsing(s => new SkillDto(
                s.SkillCode, s.SkillName, s.SkillType,
                s.WeightNum, s.Remark, s.EffectiveDate, s.CloseDate, s.IsActive));

        CreateMap<TrainingProvider, TrainingProviderDto>()
            .ConstructUsing(t => new TrainingProviderDto(
                t.TrainingCode, t.TrainingName, t.Address1,
                t.ContactName1, t.PhoneNum1, t.EmailAddress1,
                t.GroupCode, t.VendorRating, t.EffectiveDate, t.IsActive));

        CreateMap<JobMaster, JobMasterDto>()
            .ConstructUsing(j => new JobMasterDto(j.JobCode, j.JobName, j.CategoryCode, j.SerialNumber));

        CreateMap<Category, CategoryDto>()
            .ConstructUsing(c => new CategoryDto(c.CategoryCode, c.CategoryName, c.SerialNumber));

        CreateMap<CompanyFinancialYear, FinancialYearDto>()
            .ConstructUsing(f => new FinancialYearDto(
                f.SerialNumber, f.StartDate, f.EndDate, f.CloseFlag, f.IsOpen));

        CreateMap<Benefit, BenefitDto>()
            .ConstructUsing(b => new BenefitDto(b.BenefitCode, b.BenefitDescription));

        CreateMap<Goal, GoalDto>()
            .ConstructUsing(g => new GoalDto(g.GoalCode, g.GoalName));

        CreateMap<Mode, ModeDto>()
            .ConstructUsing(m => new ModeDto(m.ModeCode, m.ModeDescription));

        CreateMap<Source, SourceDto>()
            .ConstructUsing(s => new SourceDto(s.SourceCode, s.SourceName));

        CreateMap<SkillGroup, SkillGroupDto>()
            .ConstructUsing(sg => new SkillGroupDto(sg.GroupCode, sg.GroupName));

        CreateMap<CostMaster, CostMasterDto>()
            .ConstructUsing(c => new CostMasterDto(c.CostCode, c.CostName));
    }
}
