using AutoMapper;
using MediatR;
using TrainingDevelopment.Application.Common.Exceptions;
using TrainingDevelopment.Application.DTOs;
using TrainingDevelopment.Domain.Interfaces;

namespace TrainingDevelopment.Application.Features.Institutes.Queries;

public class GetInstituteListQueryHandler : IRequestHandler<GetInstituteListQuery, IEnumerable<InstituteMasterDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetInstituteListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InstituteMasterDto>> Handle(GetInstituteListQuery request, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.Institutes.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<InstituteMasterDto>>(entities);
    }
}

public class GetInstituteByCodeQueryHandler : IRequestHandler<GetInstituteByCodeQuery, InstituteMasterDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetInstituteByCodeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<InstituteMasterDto> Handle(GetInstituteByCodeQuery request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Institutes.GetByCodeAsync(request.Code, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.InstituteMaster), request.Code);
        return _mapper.Map<InstituteMasterDto>(entity);
    }
}
