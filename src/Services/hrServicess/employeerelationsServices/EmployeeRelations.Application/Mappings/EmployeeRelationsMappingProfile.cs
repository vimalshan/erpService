using AutoMapper;
using EmployeeRelations.Application.DTOs;
using EmployeeRelations.Domain.Aggregates;

namespace EmployeeRelations.Application.Mappings;

public class EmployeeRelationsMappingProfile : Profile
{
    public EmployeeRelationsMappingProfile()
    {
        CreateMap<DisciplinaryMain, DisciplinaryMainDto>()
            .ConstructUsing(src => new DisciplinaryMainDto(
                src.Id, src.UnitId, src.Date, src.Details,
                src.CreatedBy, src.CreatedOn,
                src.Employees.Select(e => new DisciplinaryEmpDto(e.MainId, e.EmpSysId)),
                src.Actions.Select(a => new DisciplinaryActionDto(a.ActionId, a.MainId, a.EmpSysId,
                    a.TypeId, a.ActionDate, a.Remarks, a.DocPath, a.EntryStatus))));

        CreateMap<EwsMain, EwsMainDto>()
            .ConstructUsing(src => new EwsMainDto(
                src.Id, src.EmpSysId, src.PeriodNo,
                src.Status.Value, src.HrFlag != null ? src.HrFlag.Value : null,
                src.HrRemarks, src.AprFlag != null ? src.AprFlag.Value : null,
                src.AprRemarks, src.Final != null ? src.Final.Value : null,
                src.HrEntryDate));

        CreateMap<SurveyMaster, SurveyMasterDto>()
            .ConstructUsing(src => new SurveyMasterDto(
                src.Id, src.Name, src.Image, src.StartDate, src.EndDate,
                src.ClosureDate, src.AutoLock, src.Flag, src.TemplateId,
                src.Questions.Select(q => new SurveyQuestionDto(
                    q.QuestId, q.SurveyId, q.QuestName, q.QuestType, q.SectionId,
                    q.Mandatory, q.SortOrder,
                    q.Options.Select(o => new SurveyOptionDto(o.OptionId, o.QuestionId, o.Description))))));
    }
}
