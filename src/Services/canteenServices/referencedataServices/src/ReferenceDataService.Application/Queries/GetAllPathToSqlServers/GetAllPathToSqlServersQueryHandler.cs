using AutoMapper;
using MediatR;
using ReferenceDataService.Application.DTOs;
using ReferenceDataService.Domain.Interfaces;

namespace ReferenceDataService.Application.Queries.GetAllPathToSqlServers;

public class GetAllPathToSqlServersQueryHandler : IRequestHandler<GetAllPathToSqlServersQuery, IEnumerable<PathToSqlServerDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllPathToSqlServersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PathToSqlServerDto>> Handle(GetAllPathToSqlServersQuery request, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.PathToSqlServers.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<PathToSqlServerDto>>(entities);
    }
}
