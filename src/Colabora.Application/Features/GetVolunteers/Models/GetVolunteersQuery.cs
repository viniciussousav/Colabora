using Colabora.Application.Commons;
using MediatR;

namespace Colabora.Application.Features.GetVolunteers.Models;

public record GetVolunteersQuery : IRequest<Result<GetVolunteersResponse>>;