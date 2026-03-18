using AutoMapper;
using DocumentService.Application.DTOs;
using DocumentService.Domain.Entities;

namespace DocumentService.Application.Mappings;

public class LoanDocumentProfile : Profile
{
    public LoanDocumentProfile()
    {
        CreateMap<LoanDocument, LoanDocumentDto>()
            .ConstructUsing(src => new LoanDocumentDto(
                src.Id, src.LoanId, src.TypeId, src.LastModifiedBy, src.LastModifiedOn));
    }
}
