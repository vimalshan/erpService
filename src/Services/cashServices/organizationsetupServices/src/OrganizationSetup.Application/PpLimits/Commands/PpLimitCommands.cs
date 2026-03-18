using MediatR;
using OrganizationSetup.Application.DTOs;

namespace OrganizationSetup.Application.PpLimits.Commands;

public sealed record CreatePpLimitCommand(long LimitId, long OrgId, string TranType, long BaseCurr, decimal? LimitAmt, int FinYear) : IRequest<PpLimitDto>;
public sealed record UpdatePpLimitCommand(long LimitId, decimal? NewLimitAmt, decimal? NewLimitAct) : IRequest<PpLimitDto>;
public sealed record UploadPpCertificateCommand(long LimitId, Stream CertificateStream, string FileName) : IRequest<string>;
