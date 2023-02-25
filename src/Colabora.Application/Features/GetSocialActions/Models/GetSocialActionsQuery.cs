using Colabora.Application.Commons;
using MediatR;

namespace Colabora.Application.Features.GetSocialActions.Models;

public record GetSocialActionsQuery : IRequest<Result<GetSocialActionsResponse>> { }