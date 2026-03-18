using AutoMapper;
using MediatR;
using TrainingDevelopment.Application.Common.Exceptions;
using TrainingDevelopment.Application.DTOs;
using TrainingDevelopment.Domain.Interfaces;

namespace TrainingDevelopment.Application.Features.ProgramLov.Queries;

public class GetProgramLovListQueryHandler : IRequestHandler<GetProgramLovListQuery, IEnumerable<ProgramLovDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProgramLovListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProgramLovDto>> Handle(GetProgramLovListQuery request, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.ProgramLovs.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ProgramLovDto>>(entities);
    }
}

public class GetProgramLovByTypeCodeQueryHandler : IRequestHandler<GetProgramLovByTypeCodeQuery, ProgramLovDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProgramLovByTypeCodeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProgramLovDto> Handle(GetProgramLovByTypeCodeQuery request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.ProgramLovs.GetByTypeCodeAsync(request.TypeCode, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.ProgramLovMaster), request.TypeCode);
        return _mapper.Map<ProgramLovDto>(entity);
    }
}
