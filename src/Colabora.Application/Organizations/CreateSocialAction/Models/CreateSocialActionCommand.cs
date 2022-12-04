using Colabora.Application.Commons;
using MediatR;

namespace Colabora.Application.Organizations.CreateSocialAction.Models;

public record CreateSocialActionCommand() : IRequest<Result<CreateSocialActionResponse>>;