using MediatR;
using ProductionManagement.Application.DTOs;

namespace ProductionManagement.Application.Commands.Norms;

public record CreateNormsMainCommand(CreateNormsMainDto Dto) : IRequest<NormsMainDto>;
public record CloseNormsMainCommand(long NormNo) : IRequest<NormsMainDto>;
public record AddNormsMasterCommand(CreateNormsMasterDto Dto) : IRequest<NormsMasterDto>;
