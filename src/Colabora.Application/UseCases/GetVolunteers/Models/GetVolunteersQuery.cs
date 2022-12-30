using Colabora.Application.Commons;
using MediatR;

namespace Colabora.Application.UseCases.GetVolunteers.Models;

public record GetVolunteersQuery : IRequest<Result<GetVolunteersResponse>>;