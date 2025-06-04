using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;

public class DbContextHelperService(IBattleManagementSystemDbContext dbContext) : IDbContextHelperService
{
    public Location FindLocationById(int id)
    {
        Location? location = dbContext.Location.FirstOrDefault(p => p.LocationId == id);

        if (location is null) throw new NotFoundException($"Location with id {id} not found");

        return location;
    }
    
    public List<Location> FindAllLocationsOfPlayer(Player player)
    {
        var locationIDs = 
            dbContext.PlayerLocation
                .Where(playerLocation => playerLocation.PlayerId == player.PlayerId && playerLocation.RoomId == player.RoomId)
                .Select(playerLocation => playerLocation.LocationId);

        var locations = dbContext.Location
            .Where(l => locationIDs.Contains(l.LocationId)).ToList();
        
        return locations;
    }
        
    public PlayerLocation FindPlayerLocationById(int id)
    {
        PlayerLocation? playerLocation = dbContext.PlayerLocation.FirstOrDefault(p => p.LocationId == id);

        if (playerLocation is null) throw new NotFoundException($"Location with id {id} not found");

        return playerLocation;
    }
    
    public Player FindPlayerById(int id)
    {
        Player? player = dbContext.Player.FirstOrDefault(p => p.PlayerId == id);

        if (player is null) throw new NotFoundException($"Player with id {id} not found");

        return player;
    }
    
    public Team FindTeamById(int id)
    {
        Team? team = dbContext.Team.FirstOrDefault(t => t.TeamId == id);

        if(team is null) throw new NotFoundException($"Team with id {id} not found");
            
        return team;
    }
    
    public Kill FindKillById(int id)
    {
        Kill? kill = dbContext.Kill.Include(k => k.Location).FirstOrDefault(t => t.KillId == id);

        if(kill is null) throw new NotFoundException($"Kill with id {id} not found");
            
        return kill;
    }
    
    public List<Kill> FindAllKillsOfPlayer(Player player)
    {
        var kills = dbContext.Kill
            .Include(k => k.Location)
            .Where(kill => kill.PlayerId == player.PlayerId && kill.RoomId == player.RoomId).ToList();

        return kills;
    }
    
    public Death FindDeathById(int id)
    {
        Death? death = dbContext.Death.Include(k => k.Location).FirstOrDefault(t => t.DeathId == id);

        if(death is null) throw new NotFoundException($"Death with id {id} not found");
            
        return death;
    }
    
    public List<Death> FindAllDeathsOfPlayer(Player player)
    {
        var deaths = dbContext.Death
            .Include(k => k.Location)
            .Where(death => death.PlayerId == player.PlayerId && death.RoomId == player.RoomId).ToList();

        return deaths;
    }
    
    public Account FindAccountById(int id)
    {
        Account? account = dbContext.Account.FirstOrDefault(t => t.AccountId == id);

        if(account is null) throw new NotFoundException($"Account with id {id} not found");
            
        return account;
    }
    
    public Account FindAccountByEmail(string email)
    {
        Account? account = dbContext.Account.FirstOrDefault(t => t.Email == email);

        if(account is null) throw new NotFoundException($"Account with email {email} not found");
            
        return account;
    }
    
    public Battle FindBattleById(int id)
    {
        Battle? battle = dbContext.Battle.FirstOrDefault(t => t.BattleId == id);

        if(battle is null) throw new NotFoundException($"Battle with id {id} not found");
            
        return battle;
    }
    
    public Room FindRoomById(int id)
    {
        Room? room = dbContext.Room.FirstOrDefault(r => r.RoomId == id);

        if(room is null) throw new NotFoundException($"Room with id {id} not found");
            
        return room;
    }
        
    public Room FindRoomByIJoinCode(string joinCode)
    {
        Room? room = dbContext.Room.FirstOrDefault(r => r.JoinCode == joinCode);

        if(room is null) throw new NotFoundException($"Room with join code {joinCode} not found");
            
        return room;
    }
}