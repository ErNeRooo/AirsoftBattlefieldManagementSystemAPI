using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.PlayerLocationHelper;

public class PlayerLocationHelper(IBattleManagementSystemDbContext dbContext) : IPlayerLocationHelper
{
    public PlayerLocation FindById(int? id)
    {
        PlayerLocation? playerLocation = dbContext.PlayerLocation.FirstOrDefault(p => p.LocationId == id);

        if (playerLocation is null) throw new NotFoundException($"Location with id {id} not found");

        return playerLocation;
    }
}