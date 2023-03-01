using Colabora.Application.Commons;
using MediatR;

namespace Colabora.Application.Features.GetSocialActionById.Models;

public record GetSocialActionByIdQuery(int Id) : IRequest<Result<GetSocialActionByIdResponse>>; 