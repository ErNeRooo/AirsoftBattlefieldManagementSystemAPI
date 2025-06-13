using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.KillHelper;

public class KillHelper(IBattleManagementSystemDbContext dbContext) : IKillHelper
{
    public Kill FindById(int? id)
    {
        Kill? kill = dbContext.Kill.Include(k => k.Location).FirstOrDefault(t => t.KillId == id);

        if(kill is null) throw new NotFoundException($"Kill with id {id} not found");
            
        return kill;
    }
    
    public List<Kill> FindAllOfPlayer(Player player)
    {
        var kills = dbContext.Kill
            .Include(k => k.Location)
            .Where(kill => kill.PlayerId == player.PlayerId && kill.RoomId == player.RoomId).ToList();

        return kills;
    }
}