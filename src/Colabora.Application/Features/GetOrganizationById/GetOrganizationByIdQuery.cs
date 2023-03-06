using Colabora.Application.Commons;
using MediatR;

namespace Colabora.Application.Features.GetOrganizationById;

public record GetOrganizationByIdQuery(int Id) : IRequest<Result<GetOrganizationByIdResponse>>;