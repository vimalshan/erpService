using AutoMapper;
using TrainingDevelopment.Application.DTOs;
using TrainingDevelopment.Domain.Entities;

namespace TrainingDevelopment.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TrainingDetail, TrainingDetailDto>()
            .ConstructUsing(src => new TrainingDetailDto(
                src.Id,
                src.FinancialYear,
                src.EmployeeSysId,
                src.TrainingNeed,
                src.GapArea,
                src.Mode,
                src.Mode == 1 ? "On-The-Job" : "Classroom",
                src.ProgramId,
                src.ProgramDescription,
                src.PlannedFrom,
                src.PlannedTo,
                src.Status,
                src.Status == "P" ? "Pending" : src.Status == "C" ? "Completed" : "Dropped",
                src.ActualFrom,
                src.ActualTo,
                src.InstituteId,
                src.InstituteDescription,
                src.TrainerId,
                src.TrainerDescription,
                src.PlaceId,
                src.Place,
                src.Cost,
                src.DroppedRemarks,
                src.LastModifiedBy,
                src.LastModifiedOn));

        CreateMap<InstituteMaster, InstituteMasterDto>()
            .ConstructUsing(src => new InstituteMasterDto(
                src.InstituteCode,
                src.InstituteName,
                src.Address1,
                src.Address2,
                src.City,
                src.State,
                src.Pin,
                src.Phone,
                src.Fax,
                src.Email,
                src.Url,
                src.InstituteType,
                src.CampusRecruit,
                src.InstituteClass,
                src.LastModifiedBy,
                src.LastModifiedOn));

        CreateMap<ProgramLovMaster, ProgramLovDto>()
            .ConstructUsing(src => new ProgramLovDto(src.TypeCode, src.Code, src.Name));
    }
}
