using AutoMapper;
using MedicineManagement.Application.DTOs;
using MedicineManagement.Domain.Entities;

namespace MedicineManagement.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<MedicineType, MedicineTypeDto>();
        CreateMap<MedicinePackaging, MedicinePackagingDto>();
        CreateMap<Medicine, MedicineDto>();
        CreateMap<DoctorAttendant, DoctorAttendantDto>();
        CreateMap<MedicineCredit, MedicineCreditDto>();
        CreateMap<MedicineIssue, MedicineIssueDto>();
        CreateMap<PurchaseMain, PurchaseMainDto>();
        CreateMap<PurchaseSub, PurchaseSubDto>();
    }
}
