using AutoMapper;
using PFTransactionalService.Application.DTOs;
using PFTransactionalService.Domain.Aggregates;
using PFTransactionalService.Domain.Entities;
using PFTransactionalService.Domain.Enums;

namespace PFTransactionalService.Application.Mappings;

public class PFTransactionalMappingProfile : Profile
{
    public PFTransactionalMappingProfile()
    {
        CreateMap<PFAccumulation, PFAccumulationDto>()
            .ForMember(d => d.PfAccStatus, o => o.MapFrom(s => ((char)s.PfAccStatus).ToString()))
            .ForMember(d => d.Contributions, o => o.MapFrom(s => s.Contributions))
            .ForMember(d => d.Certificates, o => o.MapFrom(s => s.Certificates));

        CreateMap<PFContributionTxn, ContributionTxnDto>()
            .ForMember(d => d.PfTxnStatus, o => o.MapFrom(s => ((char)s.PfTxnStatus).ToString()));

        CreateMap<PFSettlement, PFSettlementDto>()
            .ForMember(d => d.PfSettlementStatus, o => o.MapFrom(s => ((char)s.PfSettlementStatus).ToString()))
            .ForMember(d => d.Transactions, o => o.MapFrom(s => s.Transactions));

        CreateMap<PFSettlementTxn, SettlementTxnDto>()
            .ForMember(d => d.PfSettlementTxnStatus, o => o.MapFrom(s => ((char)s.PfSettlementTxnStatus).ToString()));

        CreateMap<PFWithdrawalCertificate, WithdrawalCertificateDto>()
            .ForMember(d => d.CertificateStatus, o => o.MapFrom(s => ((char)s.CertificateStatus).ToString()));

        CreateMap<FinancialYear, FinancialYearDto>()
            .ForMember(d => d.AcClsFlg, o => o.MapFrom(s => ((char)s.AcClsFlg).ToString()));
    }
}
