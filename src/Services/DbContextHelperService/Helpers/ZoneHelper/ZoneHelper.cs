using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.ZoneHelper;

public class ZoneHelper(IBattleManagementSystemDbContext dbContext) : IZoneHelper
{
    public Zone FindById(int? id)
    {
        Zone? zone = dbContext.Zone
            .Include(zone => zone.Vertices)
            .FirstOrDefault(z => z.ZoneId == id);

        if(zone is null) throw new NotFoundException($"Zone with id {id} not found");
            
        return zone;
    }
    
    public List<Zone> FindAllOfBattle(Battle battle)
    {
        bool doesBattleExist = dbContext.Battle.Any(b => b.BattleId == battle.BattleId);
            
        if(!doesBattleExist) throw new NotFoundException("Battle not found");
        
        List<Zone> zones = dbContext.Zone
            .Include(zone => zone.Vertices)
            .Where(zone => zone.BattleId == battle.BattleId).ToList();

        return zones;
    }
    
    public List<Zone> FindAllOfBattle(int battleId)
    {
        bool doesBattleExist = dbContext.Battle.Any(b => b.BattleId == battleId);
            
        if(!doesBattleExist) throw new NotFoundException("Battle not found");
        
        List<Zone> zones = dbContext.Zone
            .Include(zone => zone.Vertices)
            .Where(zone => zone.BattleId == battleId).ToList();

        return zones;
    }
}