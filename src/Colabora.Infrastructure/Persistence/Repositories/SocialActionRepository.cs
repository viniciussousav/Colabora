using Colabora.Domain.Entities;
using Colabora.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Colabora.Infrastructure.Persistence.Repositories;

public class SocialActionRepository : ISocialActionRepository
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public SocialActionRepository(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task CreateSocialAction(SocialAction socialAction)
    {
        var ctx = await _dbContextFactory.CreateDbContextAsync();
        await ctx.SocialActions.AddAsync(socialAction);
    }
}