using MediatR;
using AdminService.Application.DTOs;

namespace AdminService.Application.Queries;

public record GetAdminMasterByIdQuery(string AdminId) : IRequest<AdminMasterDto?>;
public record GetAllAdminMastersQuery : IRequest<IReadOnlyList<AdminMasterDto>>;
public record GetAdminUserMapByIdQuery(string MapId) : IRequest<AdminUserMapDto?>;
public record GetAdminUserMapsByAdminIdQuery(string AdminId) : IRequest<IReadOnlyList<AdminUserMapDto>>;
public record GetAllAdminUserMapsQuery : IRequest<IReadOnlyList<AdminUserMapDto>>;
public record GetAdminFinUserMapByIdQuery(string FinanceMapId) : IRequest<AdminFinUserMapDto?>;
public record GetAllAdminFinUserMapsQuery : IRequest<IReadOnlyList<AdminFinUserMapDto>>;
public record GetAccessRightsByIdQuery(string RightsId) : IRequest<AdminAccessRightsDto?>;
public record GetAccessRightsByLocationQuery(string LocationId) : IRequest<IReadOnlyList<AdminAccessRightsDto>>;
public record GetAllAccessRightsQuery : IRequest<IReadOnlyList<AdminAccessRightsDto>>;
public record GetAccessRightsLogsByRightsIdQuery(string RightsId) : IRequest<IReadOnlyList<AdminAccessRightsLogDto>>;
