﻿using Colabora.Domain.Entities;
using Colabora.Domain.Exceptions;
using Colabora.Domain.Repositories;
using Colabora.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<SocialAction>> GetAllSocialActions()
    {
        return await _dbContext.SocialActions.AsNoTracking().ToListAsync();
    }
    
    public async Task<SocialAction> GetSocialActionById(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.SocialActions
            .AsNoTracking()
            .Include(action => action.Participations)
            .FirstOrDefaultAsync(action => action.SocialActionId == id, cancellationToken) ?? SocialAction.None;
    }

    public async Task CreateParticipation(int socialActionId, Participation participation)
    {
        var socialAction = await _dbContext.SocialActions
                               .Include(action => action.Participations)
                               .FirstOrDefaultAsync(action => action.SocialActionId == socialActionId)
            ?? throw new InvalidSocialActionException($"Social action with id {socialActionId} not exists");

        socialAction.Participations.Add(participation);
        await _dbContext.SaveChangesAsync();
    }
}