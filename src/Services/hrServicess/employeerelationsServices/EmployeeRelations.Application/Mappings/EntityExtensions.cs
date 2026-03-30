using EmployeeRelations.Application.DTOs;
using EmployeeRelations.Domain.Aggregates;

namespace EmployeeRelations.Application.Mappings;

public static class EntityExtensions
{
    public static DisciplinaryMainDto ToDto(this DisciplinaryMain src) => new(
        src.Id, src.UnitId, src.Date, src.Details, src.CreatedBy, src.CreatedOn,
        src.Employees.Select(e => new DisciplinaryEmpDto(e.MainId, e.EmpSysId)),
        src.Actions.Select(a => new DisciplinaryActionDto(
            a.ActionId, a.MainId, a.EmpSysId, a.TypeId,
            a.ActionDate, a.Remarks, a.DocPath, a.EntryStatus)));

    public static EwsMainDto ToDto(this EwsMain src) => new(
        src.Id, src.EmpSysId, src.PeriodNo,
        src.Status.Value,
        src.HrFlag != null ? src.HrFlag.Value : null,
        src.HrRemarks,
        src.AprFlag != null ? src.AprFlag.Value : null,
        src.AprRemarks,
        src.Final != null ? src.Final.Value : null,
        src.HrEntryDate);

    public static SurveyMasterDto ToDto(this SurveyMaster src) => new(
        src.Id, src.Name, src.Image, src.StartDate, src.EndDate,
        src.ClosureDate, src.AutoLock, src.Flag, src.TemplateId,
        src.Questions.Select(q => new SurveyQuestionDto(
            q.QuestId, q.SurveyId, q.QuestName, q.QuestType, q.SectionId,
            q.Mandatory, q.SortOrder,
            q.Options.Select(o => new SurveyOptionDto(o.OptionId, o.QuestionId, o.Description)))));
}
