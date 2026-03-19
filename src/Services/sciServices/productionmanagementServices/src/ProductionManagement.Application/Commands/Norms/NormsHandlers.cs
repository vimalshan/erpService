using AutoMapper;
using MediatR;
using ProductionManagement.Application.DTOs;
using ProductionManagement.Domain.Entities;
using ProductionManagement.Domain.Interfaces;

namespace ProductionManagement.Application.Commands.Norms;

public class CreateNormsMainHandler : IRequestHandler<CreateNormsMainCommand, NormsMainDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateNormsMainHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<NormsMainDto> Handle(CreateNormsMainCommand request, CancellationToken cancellationToken)
    {
        var norm = new NormsMain(request.Dto.NormNo, request.Dto.NormEffDate);
        var result = await _unitOfWork.Norms.AddAsync(norm, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<NormsMainDto>(result);
    }
}

public class CloseNormsMainHandler : IRequestHandler<CloseNormsMainCommand, NormsMainDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CloseNormsMainHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<NormsMainDto> Handle(CloseNormsMainCommand request, CancellationToken cancellationToken)
    {
        var norm = await _unitOfWork.Norms.GetByIdAsync(request.NormNo, cancellationToken)
            ?? throw new KeyNotFoundException($"Norm {request.NormNo} not found.");

        norm.Close();
        await _unitOfWork.Norms.UpdateAsync(norm, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<NormsMainDto>(norm);
    }
}

public class AddNormsMasterHandler : IRequestHandler<AddNormsMasterCommand, NormsMasterDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddNormsMasterHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<NormsMasterDto> Handle(AddNormsMasterCommand request, CancellationToken cancellationToken)
    {
        var norm = await _unitOfWork.Norms.GetByIdAsync(request.Dto.NormNo, cancellationToken)
            ?? throw new KeyNotFoundException($"Norm {request.Dto.NormNo} not found.");

        norm.AddNormsMaster(request.Dto.NormId, request.Dto.NormInputCode, request.Dto.NormOutputCode, request.Dto.NormRate);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var master = norm.NormsMasters.Last();
        return _mapper.Map<NormsMasterDto>(master);
    }
}
