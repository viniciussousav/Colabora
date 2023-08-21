using Colabora.Application.Commons;
using MediatR;

namespace Colabora.Application.Features.Volunteer.GetVolunteerById.Models;

public record GetVolunteerByIdQuery(Guid Id) : IRequest<Result<GetVolunteerByIdResponse>>;