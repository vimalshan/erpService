using CertificateService.Application.Commands;
using CertificateService.Application.DTOs;
using MediatR;

namespace CertificateService.GraphQL.Mutations;

public class Mutation
{
    public async Task<CertificateDto> CreateCertificate([Service] IMediator mediator, CreateCertificateDto input) => await mediator.Send(new CreateCertificateCommand(input));
    public async Task<CertificateDto> UpdateCertificate([Service] IMediator mediator, UpdateCertificateDto input) => await mediator.Send(new UpdateCertificateCommand(input));
    public async Task<bool> DeleteCertificate([Service] IMediator mediator, int id) => await mediator.Send(new DeleteCertificateCommand(id));
}
