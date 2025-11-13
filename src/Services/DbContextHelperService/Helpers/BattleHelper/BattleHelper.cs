using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.BattleHelper;

public class BattleHelper(IBattleManagementSystemDbContext dbContext) : IBattleHelper
{
    public Battle FindById(int? id)
    {
        Battle? battle = dbContext.Battle.FirstOrDefault(t => t.BattleId == id);

        if(battle is null) throw new NotFoundException($"Battle with id {id} not found");
            
        return battle;
    }

    public Battle FindByIdIncludingRoom(int? id)
    {
        Battle? battle = dbContext.Battle
            .Include(battle => battle.Room)
            .FirstOrDefault(t => t.BattleId == id);

        if(battle is null) throw new NotFoundException($"Battle with id {id} not found");
            
        return battle;
    }

    public Battle FindByIdIncludingRelated(int? id)
    {
        Battle? battle = dbContext.Battle
            .Include(battle => battle.Room)
            .Include(battle => battle.PlayerLocations)
            .Include(battle => battle.Kills)
            .Include(battle => battle.Deaths)
            .Include(battle => battle.MapPings)
            .Include(battle => battle.Orders)
            .Include(battle => battle.Zones)
            .FirstOrDefault(t => t.BattleId == id);

        if(battle is null) throw new NotFoundException($"Battle with id {id} not found");
            
        return battle;
    }
}