using AutoMapper;
using MediatR;
using ProductionManagement.Application.DTOs;
using ProductionManagement.Domain.Interfaces;

namespace ProductionManagement.Application.Queries.Norms;

public class GetAllNormsHandler : IRequestHandler<GetAllNormsQuery, IReadOnlyList<NormsMainDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllNormsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<NormsMainDto>> Handle(GetAllNormsQuery request, CancellationToken cancellationToken)
    {
        var norms = await _unitOfWork.Norms.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<NormsMainDto>>(norms);
    }
}

public class GetNormByIdHandler : IRequestHandler<GetNormByIdQuery, NormsMainDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetNormByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<NormsMainDto?> Handle(GetNormByIdQuery request, CancellationToken cancellationToken)
    {
        var norm = await _unitOfWork.Norms.GetByIdAsync(request.NormNo, cancellationToken);
        return norm is null ? null : _mapper.Map<NormsMainDto>(norm);
    }
}
