using Colabora.Application.Commons;
using MediatR;

namespace Colabora.Application.UseCases.GetSocialActions.Models;

public record GetSocialActionsQuery : IRequest<Result<GetSocialActionsResponse>> { }