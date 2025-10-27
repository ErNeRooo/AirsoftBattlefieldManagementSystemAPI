using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.PlayerLocationHelper;

public class PlayerLocationHelper(IBattleManagementSystemDbContext dbContext) : IPlayerLocationHelper
{
    public PlayerLocation FindById(int? id)
    {
        PlayerLocation? playerLocation = dbContext.PlayerLocation.FirstOrDefault(p => p.LocationId == id);

        if (playerLocation is null) throw new NotFoundException($"Location with id {id} not found");

        return playerLocation;
    }
    
    public List<PlayerLocation> FindAllOfPlayerIncludingLocation(Player player)
    {
        List<PlayerLocation> locations = 
            dbContext.PlayerLocation
                .Include(pl => pl.Location)
                .Where(playerLocation => playerLocation.PlayerId == player.PlayerId)
                .ToList();
        
        return locations;
    }
}