using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.DeathHelper;

public class DeathHelper(IBattleManagementSystemDbContext dbContext) : IDeathHelper
{
    public Death FindById(int? id)
    {
        Death? death = dbContext.Death.Include(death => death.Location).FirstOrDefault(death => death.DeathId == id);

        if(death is null) throw new NotFoundException($"Death with id {id} not found");
            
        return death;
    }
    
    public Death FindByIdIncludingBattle(int? id)
    {
        Death? death = dbContext.Death
            .Include(death => death.Location)
            .Include(death => death.Battle)
            .FirstOrDefault(death => death.DeathId == id);

        if(death is null) throw new NotFoundException($"Death with id {id} not found");
            
        return death;
    }
    
    public List<Death> FindAllOfPlayer(Player player)
    {
        var deaths = dbContext.Death
            .Include(death => death.Location)
            .Where(death => death.PlayerId == player.PlayerId).ToList();

        return deaths;
    }
}