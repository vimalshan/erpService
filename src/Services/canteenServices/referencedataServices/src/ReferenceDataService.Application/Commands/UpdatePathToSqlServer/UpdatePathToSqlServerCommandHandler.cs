using AutoMapper;
using MediatR;
using ReferenceDataService.Application.DTOs;
using ReferenceDataService.Domain.Interfaces;

namespace ReferenceDataService.Application.Commands.UpdatePathToSqlServer;

public class UpdatePathToSqlServerCommandHandler : IRequestHandler<UpdatePathToSqlServerCommand, PathToSqlServerDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePathToSqlServerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PathToSqlServerDto> Handle(UpdatePathToSqlServerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.PathToSqlServers.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"PathToSqlServer with Id '{request.Id}' not found.");

        entity.Update(request.ServerName, request.DatabaseName, request.UserId, request.DbPassword);
        _unitOfWork.PathToSqlServers.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PathToSqlServerDto>(entity);
    }
}
