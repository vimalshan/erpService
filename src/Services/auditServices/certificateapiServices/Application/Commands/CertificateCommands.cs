using CertificateService.Application.DTOs;
using MediatR;

namespace CertificateService.Application.Commands;

public record CreateCertificateCommand(CreateCertificateDto Dto) : IRequest<CertificateDto>;
public record UpdateCertificateCommand(UpdateCertificateDto Dto) : IRequest<CertificateDto>;
public record DeleteCertificateCommand(int Id) : IRequest<bool>;
