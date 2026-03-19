using AutoMapper;
using ProblemManagement.Application.DTOs;
using ProblemManagement.Domain.Entities;

namespace ProblemManagement.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ProblemMain, ProblemDto>();
        CreateMap<ProblemSolution, ProblemSolutionDto>();
        CreateMap<ProblemApproval, ProblemApprovalDto>();
        CreateMap<ProblemAttachment, ProblemAttachmentDto>();
        CreateMap<SolutionApproval, SolutionApprovalDto>();
        CreateMap<SolutionComment, SolutionCommentDto>();
        CreateMap<ProblemFunction, ProblemFunctionDto>();
        CreateMap<ProblemImpact, ProblemImpactDto>();
    }
}
