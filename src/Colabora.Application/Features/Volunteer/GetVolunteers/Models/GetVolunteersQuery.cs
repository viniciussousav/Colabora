using Colabora.Application.Commons;
using MediatR;

namespace Colabora.Application.Features.Volunteer.GetVolunteers.Models;

public record GetVolunteersQuery : IRequest<Result<GetVolunteersResponse>>;