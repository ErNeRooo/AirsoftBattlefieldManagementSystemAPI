using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.DeathHelper;

public class DeathHelper(IBattleManagementSystemDbContext dbContext) : IDeathHelper
{
    public Death FindById(int? id)
    {
        Death? death = dbContext.Death.Include(k => k.Location).FirstOrDefault(t => t.DeathId == id);

        if(death is null) throw new NotFoundException($"Death with id {id} not found");
            
        return death;
    }
    
    public List<Death> FindAllOfPlayer(Player player)
    {
        var deaths = dbContext.Death
            .Include(k => k.Location)
            .Where(death => death.PlayerId == player.PlayerId && death.RoomId == player.RoomId).ToList();

        return deaths;
    }
}