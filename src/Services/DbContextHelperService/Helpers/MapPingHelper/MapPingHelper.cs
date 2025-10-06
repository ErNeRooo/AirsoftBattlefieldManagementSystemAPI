using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.MapPingHelper;

public class MapPingHelper(IBattleManagementSystemDbContext dbContext) : IMapPingHelper
{
    public MapPing FindById(int? id)
    {
        MapPing? mapPing = dbContext.MapPing.Include(mapPing => mapPing.Location).FirstOrDefault(t => t.MapPingId == id);

        if(mapPing is null) throw new NotFoundException($"MapPing with id {id} not found");
            
        return mapPing;
    }
    
    public MapPing FindByIdIncludingBattle(int? id)
    {
        MapPing? mapPing = dbContext.MapPing
            .Include(mapPing => mapPing.Location)
            .Include(mapPing => mapPing.Battle)
            .FirstOrDefault(t => t.MapPingId == id);

        if(mapPing is null) throw new NotFoundException($"MapPing with id {id} not found");
            
        return mapPing;
    }
    
    public List<MapPing> FindAllOfPlayer(Player player)
    {
        bool doesTeamExist = dbContext.Team.Any(team => team.TeamId == player.PlayerId);
            
        if(!doesTeamExist) throw new NotFoundException("Team doesn't exist");
        
        List<MapPing> mapPings = dbContext.MapPing
            .Include(mapPing => mapPing.Location)
            .Where(mapPing => mapPing.PlayerId == player.PlayerId).ToList();

        return mapPings;
    }
    
    public List<MapPing> FindAllOfTeam(Team team)
    {
        bool doesTeamExist = dbContext.Team.Any(t => t.TeamId == team.TeamId);
            
        if(!doesTeamExist) throw new NotFoundException("Team doesn't exist");
        
        List<MapPing> mapPings = dbContext.MapPing
            .Include(mapPing => mapPing.Location)
            .Include(mapPing => mapPing.Player)
            .Where(mapPing => mapPing.Player.TeamId == team.TeamId).ToList();

        return mapPings;
    }
    
    public List<MapPing> FindAllOfTeam(int teamId)
    {
        bool doesTeamExist = dbContext.Team.Any(team => team.TeamId == teamId);
            
        if(!doesTeamExist) throw new NotFoundException("Team doesn't exist");
        
        List<MapPing> mapPings = dbContext.MapPing
            .Include(mapPing => mapPing.Location)
            .Include(mapPing => mapPing.Player)
            .Where(mapPing => mapPing.Player.TeamId == teamId).ToList();

        return mapPings;
    }
}