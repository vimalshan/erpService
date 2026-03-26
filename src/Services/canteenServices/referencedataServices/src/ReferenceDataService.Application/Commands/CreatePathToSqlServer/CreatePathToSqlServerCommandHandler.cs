using AutoMapper;
using MediatR;
using ReferenceDataService.Application.DTOs;
using ReferenceDataService.Domain.Entities;
using ReferenceDataService.Domain.Interfaces;

namespace ReferenceDataService.Application.Commands.CreatePathToSqlServer;

public class CreatePathToSqlServerCommandHandler : IRequestHandler<CreatePathToSqlServerCommand, PathToSqlServerDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePathToSqlServerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PathToSqlServerDto> Handle(CreatePathToSqlServerCommand request, CancellationToken cancellationToken)
    {
        var entity = new PathToSqlServer(
            request.CompanyCode, request.ServerName,
            request.DatabaseName, request.UserId, request.DbPassword);

        await _unitOfWork.PathToSqlServers.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PathToSqlServerDto>(entity);
    }
}
