using Colabora.Domain.Entities;

namespace Colabora.Domain.Repositories;

public interface ISocialActionRepository
{
    Task CreateSocialAction(SocialAction socialAction);
}