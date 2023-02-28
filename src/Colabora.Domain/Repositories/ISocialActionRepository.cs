using Colabora.Domain.Entities;
using Colabora.Domain.ValueObjects;

namespace Colabora.Domain.Repositories;

public interface ISocialActionRepository
{
    Task<SocialAction> CreateSocialAction(SocialAction socialAction);
    Task<List<SocialAction>> GetAllSocialActions();
    Task<SocialAction> GetSocialActionById(int id, CancellationToken cancellationToken);
    Task CreateParticipation(int socialAction, Participation participation);
}