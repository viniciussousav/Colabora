namespace Colabora.Domain.SocialAction;

public interface ISocialActionRepository
{
    Task<SocialAction> CreateSocialAction(SocialAction socialAction);
    Task<List<SocialAction>> GetAllSocialActions();
    Task<SocialAction> GetSocialActionById(int id, CancellationToken cancellationToken);
    Task CreateParticipation(int socialAction, Participation.Participation participation);
}