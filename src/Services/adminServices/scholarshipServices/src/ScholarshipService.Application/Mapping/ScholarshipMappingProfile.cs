using AutoMapper;
using ScholarshipService.Application.DTOs;
using ScholarshipService.Domain.Entities;

namespace ScholarshipService.Application.Mapping;

public class ScholarshipMappingProfile : Profile
{
    public ScholarshipMappingProfile()
    {
        CreateMap<ScholarshipMain, ScholarshipMainDto>()
            .ConstructUsing((src, ctx) => new ScholarshipMainDto(
                src.Id, src.EmployeeSysId, src.GradeId, src.DependentId,
                src.ChildName, src.LastSchool, src.LastYearOfSchool, src.LastExam,
                src.CgpaFlag, src.MarksPercentage, src.MarksGpa, src.MarksFile,
                src.CourseName, src.CourseJoinYear, src.CourseJoinMonth, src.CourseDuration,
                src.AdmissionReceiptFile, src.PaymentMode, src.ChildAccountNumber,
                src.ChildBankIfsc, src.ChildBankMicr, src.EntryStatus,
                src.Source, src.DisbursementAmount, src.DisbursementFrequency,
                src.LiveStatus, src.CreatedOn, src.CreatedBy,
                src.UpdatedOn, src.UpdatedBy, src.IsOffline, src.OfflineYear,
                ctx.Mapper.Map<IEnumerable<ScholarshipDetailDto>>(src.Details)));

        CreateMap<ScholarshipDetail, ScholarshipDetailDto>()
            .ConstructUsing(src => new ScholarshipDetailDto(
                src.Id, src.MainId, src.Year, src.MarksFile, src.MarksStatus,
                src.PayStatus, src.CreatedOn, src.CreatedBy,
                src.ApprovedOn, src.ApprovedBy, src.PayDate, src.PayAmount));

        CreateMap<ScholarshipAmount, ScholarshipAmountDto>()
            .ConstructUsing(src => new ScholarshipAmountDto(
                src.Id, src.OrgId, src.GradeCategory, src.EligibleExam,
                src.ApplicableAllGrade, src.GradeId, src.FromYear, src.CloseYear,
                src.EligibleAmount, src.EligibleYear, src.CutoffMarks));
    }
}
