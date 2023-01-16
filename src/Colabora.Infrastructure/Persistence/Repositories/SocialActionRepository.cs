using Colabora.Domain.Entities;
using Colabora.Domain.Repositories;

namespace Colabora.Infrastructure.Persistence.Repositories;

public class SocialActionRepository : ISocialActionRepository
{
    private readonly AppDbContext _dbContext;

    public SocialActionRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SocialAction> CreateSocialAction(SocialAction socialAction)
    {
        var entry = await _dbContext.SocialActions.AddAsync(socialAction);
        await _dbContext.SaveChangesAsync();
        return entry.Entity;
    }
}