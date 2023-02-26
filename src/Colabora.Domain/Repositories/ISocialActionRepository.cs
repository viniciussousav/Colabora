using Colabora.Domain.Entities;

namespace Colabora.Domain.Repositories;

public interface ISocialActionRepository
{
    Task<SocialAction> CreateSocialAction(SocialAction socialAction);
    Task<List<SocialAction>> GetAllSocialActions();
    Task<SocialAction> GetSocialActionById(int id, CancellationToken cancellationToken);
    Task UpdateSocialAction(SocialAction socialAction);
}