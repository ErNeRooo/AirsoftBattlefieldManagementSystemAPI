using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.PlayerHelper;

public class PlayerHelper(IBattleManagementSystemDbContext dbContext, IClaimsHelperService claimsHelper) : IPlayerHelper
{
    public Player FindSelf(ClaimsPrincipal user)
    {
        int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);
        Player? player = dbContext.Player.FirstOrDefault(p => p.PlayerId == playerId);

        if (player is null) throw new NotFoundException("You do not exist player");

        return player;
    }
    
    public Player FindById(int? id)
    {
        Player? player = dbContext.Player.FirstOrDefault(p => p.PlayerId == id);

        if (player is null) throw new NotFoundException($"Player with id {id} not found");

        return player;
    }
    
    public Player FindByIdIncludingAccount(int? id)
    {
        Player? player = dbContext.Player.Include(p => p.Account).FirstOrDefault(p => p.PlayerId == id);

        if (player is null) throw new NotFoundException($"Player with id {id} not found");
        if (player.Account is null) throw new NotFoundException($"Player with id {id} is not logged into account");
        
        return player;
    }

    public Player FindByIdIncludingRoom(int? id)
    {
        Player? player = dbContext.Player
            .Include(p => p.Room)
            .FirstOrDefault(p => p.PlayerId == id);

        if (player is null) throw new NotFoundException($"Player with id {id} not found");
        if (player.Account is null) throw new NotFoundException($"Player with id {id} is not logged into account");
        
        return player;
    }
}