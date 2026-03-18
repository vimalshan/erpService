using AutoMapper;
using BankService.Application.DTOs;
using BankService.Domain.Entities;

namespace BankService.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BankMaster, BankMasterDto>();
        CreateMap<ChequeDetail, ChequeDetailDto>();
        CreateMap<BankAccount, BankAccountDto>();
        CreateMap<ChequeRegister, ChequeRegisterDto>();
        CreateMap<PaymentReconciliation, PaymentReconciliationDto>();
    }
}
