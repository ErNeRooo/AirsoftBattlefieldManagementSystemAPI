using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.BattleHelper;

public class BattleHelper(IBattleManagementSystemDbContext dbContext) : IBattleHelper
{
    public Battle FindById(int? id)
    {
        Battle? battle = dbContext.Battle.FirstOrDefault(t => t.BattleId == id);

        if(battle is null) throw new NotFoundException($"Battle with id {id} not found");
            
        return battle;
    }
}