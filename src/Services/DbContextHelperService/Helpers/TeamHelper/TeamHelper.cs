using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.TeamHelper;

public class TeamHelper(IBattleManagementSystemDbContext dbContext) : ITeamHelper
{
    public Team FindById(int? id)
    {
        Team? team = dbContext.Team.FirstOrDefault(t => t.TeamId == id);

        if(team is null) throw new NotFoundException($"Team with id {id} not found");
            
        return team;
    }

    public Team FindByIdIncludingRoom(int? id)
    {
        Team? team = dbContext.Team
            .Include(t => t.Room)
            .FirstOrDefault(t => t.TeamId == id);

        if(team is null) throw new NotFoundException($"Team with id {id} not found");
            
        return team;
    }
}