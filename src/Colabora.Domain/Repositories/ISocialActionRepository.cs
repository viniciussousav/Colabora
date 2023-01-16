using Colabora.Domain.Entities;

namespace Colabora.Domain.Repositories;

public interface ISocialActionRepository
{
    Task<SocialAction> CreateSocialAction(SocialAction socialAction);
}