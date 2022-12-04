using Colabora.Application.Commons;
using Colabora.Application.Organizations.CreateSocialAction.Models;

namespace Colabora.Application.Organizations.CreateSocialAction;

public class CreateSocialActionCommandHandler : ICreateSocialActionCommandHandler
{
    public Task<Result<CreateSocialActionResponse>> Handle(CreateSocialActionCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}