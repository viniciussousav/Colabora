using Colabora.Application.Commons;
using MediatR;

namespace Colabora.Application.Features.GetVolunteerById.Models;

public record GetVolunteerByIdQuery(int Id) : IRequest<Result<GetVolunteerByIdResponse>>;