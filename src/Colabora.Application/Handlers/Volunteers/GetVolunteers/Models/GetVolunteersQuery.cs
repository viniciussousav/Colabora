using Colabora.Application.Commons;
using MediatR;

namespace Colabora.Application.Handlers.Volunteers.GetVolunteers.Models;

public record GetVolunteersQuery : IRequest<Result<GetVolunteersResponse>>;