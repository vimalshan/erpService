using AutoMapper;
using TrustService.Application.DTOs;
using TrustService.Domain.Entities;

namespace TrustService.Application.Common.Mappings;

public class TrustMappingProfile : Profile
{
    public TrustMappingProfile()
    {
        CreateMap<TrustMaster, TrustMasterDto>();
        CreateMap<TrustFundType, TrustFundTypeDto>();
        CreateMap<TrustRole, TrustRoleDto>();
        CreateMap<TrustApprover, TrustApproverDto>();
        CreateMap<TrustConfiguration, TrustConfigurationDto>();
        CreateMap<TrustUnit, TrustUnitDto>();
        CreateMap<TrustAuditLog, TrustAuditLogDto>();
    }
}
