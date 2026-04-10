using AutoMapper;
using SparshTransactional.Application.DTOs;
using SparshTransactional.Domain.Entities;

namespace SparshTransactional.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ScholarshipMaster, ScholarshipMasterDto>();
        CreateMap<EligibilityCriteria, EligibilityCriteriaDto>();
        CreateMap<ScholarshipApplication, ScholarshipApplicationDto>();
        CreateMap<ScholarshipDisbursement, ScholarshipDisbursementDto>();
    }
}
