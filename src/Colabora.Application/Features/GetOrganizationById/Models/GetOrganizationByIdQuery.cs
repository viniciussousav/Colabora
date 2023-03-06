using Colabora.Application.Commons;
using MediatR;

namespace Colabora.Application.Features.GetOrganizationById.Models;

public record GetOrganizationByIdQuery(int Id) : IRequest<Result<GetOrganizationByIdResponse>>;