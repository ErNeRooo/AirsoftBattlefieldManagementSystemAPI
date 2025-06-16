using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.LocationHelper;

public class LocationHelper(IBattleManagementSystemDbContext dbContext) : ILocationHelper
{
    public Location FindById(int? id)
    {
        Location? location = dbContext.Location.FirstOrDefault(p => p.LocationId == id);

        if (location is null) throw new NotFoundException($"Location with id {id} not found");

        return location;
    }
    
    public List<Location> FindAllOfPlayer(Player player)
    {
        var locationIDs = 
            dbContext.PlayerLocation
                .Where(playerLocation => playerLocation.PlayerId == player.PlayerId)
                .Select(playerLocation => playerLocation.LocationId);

        var locations = dbContext.Location
            .Where(l => locationIDs.Contains(l.LocationId)).ToList();
        
        return locations;
    }
}